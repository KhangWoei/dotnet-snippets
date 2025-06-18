namespace Crawler.Robots;

internal sealed class Robot (HashSet<Uri> disallowed, int? delay)
{
    public HashSet<Uri> Disallowed { get; init; } = disallowed;
    
    public int DelayMs { get; init; } = delay ?? 300;
}
