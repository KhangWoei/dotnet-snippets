namespace Crawling;

internal sealed class VisitationPolicy(IHttpClientFactory factory) : IVisitationPolicy
{
    public async Task<bool> ShouldVisit(Uri uri, CancellationToken cancellationToken)
    {
        using var client = factory.CreateClient(uri.Host);
        var response = await client.GetAsync(uri, cancellationToken);

        return response.IsSuccessStatusCode && response.Content.Headers.ContentType?.MediaType == "text/html";
    }

    public async Task<bool> ShouldVisit(string seed, CancellationToken cancellationToken)
    {
        try
        {
            var uri = new UriBuilder(seed);

            return await ShouldVisit(uri.Uri, cancellationToken);
        }
        catch
        {
            return false;
        }
    }
}
