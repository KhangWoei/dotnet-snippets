namespace Crawler.Robots;

internal sealed class Robot (HashSet<Uri> disallowed, int? delay)
{
    public HashSet<Uri> Disallowed { get; } = disallowed;
    
    public int DelayMs { get; } = delay ?? 300;
}
