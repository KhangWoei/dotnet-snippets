namespace Crawling.LinkVisiting;

public interface ILinkVisitor 
{
    public Task<string> VisitAsync(Uri uri, CancellationToken cancellationToken);
}
