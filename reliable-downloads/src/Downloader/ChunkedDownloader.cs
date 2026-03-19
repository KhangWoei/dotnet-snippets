using System.Net.Http.Headers;

namespace Downloader;

internal sealed class ChunkedDownloader(IHttpClientFactory clientFactory, long total, ExponentialBackoff backoff) : IDownloader
{
    private const int ChunkSize = 1024 * 1024; // 1 MB

    public async Task<byte[]> DownloadAsync(Uri uri, CancellationToken cancellationToken)
    {
        var chunkCount = (int)Math.Ceiling((double)total / ChunkSize);
        var chunks = new byte[chunkCount][];

        await Parallel.ForEachAsync(
            Enumerable.Range(0, chunkCount),
            cancellationToken,
            async (i, ct) =>
            {
                var start = (long)i * ChunkSize;
                var end = Math.Min(start + ChunkSize - 1, total - 1);

                chunks[i] = await backoff.ExecuteAsync(async () =>
                {
                    var client = clientFactory.CreateClient();
                    var request = new HttpRequestMessage(HttpMethod.Get, uri);
                    request.Headers.Range = new RangeHeaderValue(start, end);
                    var response = await client.SendAsync(request, ct);
                    return await response.Content.ReadAsByteArrayAsync(ct);
                }, ct);
            });

        var result = new byte[total];
        var offset = 0;
        foreach (var chunk in chunks)
        {
            chunk.CopyTo(result, offset);
            offset += chunk.Length;
        }
        return result;
    }
}
