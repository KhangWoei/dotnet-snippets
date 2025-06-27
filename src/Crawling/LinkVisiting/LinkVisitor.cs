namespace Crawling.LinkVisiting;

internal sealed class LinkVisitor : ILinkVisitor
{
    public async Task<string?> VisitAsync(HttpClient client, Uri uri, CancellationToken cancellationToken)
    {   
        var response = await client.GetAsync(uri, cancellationToken);
        
        if (response.IsSuccessStatusCode && response.Content.Headers.ContentType?.MediaType == "text/html")
        {
            Console.WriteLine($"Downloading {uri}");
            return await client.GetStringAsync(uri, cancellationToken);
        }

        return null;
    }
}
