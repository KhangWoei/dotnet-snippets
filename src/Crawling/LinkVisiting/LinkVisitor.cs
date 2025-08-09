namespace Crawling.LinkVisiting;

internal sealed class LinkVisitor(IHttpClientFactory factory, IVisitationPolicy visitationPolicy) : ILinkVisitor
{
    public async Task<string?> VisitAsync(Uri uri, CancellationToken cancellationToken)
    {   
        if (await visitationPolicy.ShouldVisit(uri, cancellationToken))
        {
            using var client = factory.CreateClient(uri.Host);
            return await client.GetStringAsync(uri, cancellationToken);
        }

        return null;
    }
}
