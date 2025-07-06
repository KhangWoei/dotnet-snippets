namespace Crawling;

public interface IVisitationPolicy
{
    Task<bool> ShouldVisit(Uri uri, CancellationToken cancellationToken);

    Task<bool> ShouldVisit(string seed, CancellationToken cancellationToken);
}
