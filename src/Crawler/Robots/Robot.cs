namespace Crawler.Robots;

internal sealed class Robot (HashSet<Uri> disallowed, int? delay)
{
    // TODO: This could be a TrieTree too but I don't think Robot files would be sizeable enough that it matters
    public HashSet<Uri> Disallowed { get; init; } = disallowed;
    
    public int DelayMs { get; init; } = delay ?? 300;
}
