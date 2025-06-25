namespace Crawling.LinkVisiting;

internal sealed class LinkVisitor(HttpClient client) : ILinkVisitor
{
    public async Task<string> VisitAsync(Uri uri, CancellationToken cancellationToken)
    {   
        var response = await client.GetAsync(uri, cancellationToken);
        
        if (response.IsSuccessStatusCode && response.Content.Headers.ContentType?.MediaType == "text/html")
        {
            Console.WriteLine($"Downloading {uri}");
            return await client.GetStringAsync(uri, cancellationToken);
        }

        return string.Empty;
    }
}
