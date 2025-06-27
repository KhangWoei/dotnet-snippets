namespace Crawling.LinkVisiting;

public interface ILinkVisitor 
{
    public Task<string?> VisitAsync(HttpClient client, Uri uri, CancellationToken cancellationToken);
}
