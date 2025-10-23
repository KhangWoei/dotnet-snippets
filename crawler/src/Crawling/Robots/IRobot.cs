namespace Crawling.Robots;

public interface IRobot
{
    HashSet<Uri> Disallowed { get; }
    
    int DelayMs { get; }
}