namespace Crawling.LinkVisiting;

internal sealed class LinkVisitor(IVisitationPolicy visitationPolicy) : ILinkVisitor
{
    public async Task<string?> VisitAsync(HttpClient client, Uri uri, CancellationToken cancellationToken)
    {   
        if (await visitationPolicy.ShouldVisit(uri, cancellationToken))
        {
            Console.WriteLine($"Downloading {uri}");
            return await client.GetStringAsync(uri, cancellationToken);
        }

        return null;
    }
}
