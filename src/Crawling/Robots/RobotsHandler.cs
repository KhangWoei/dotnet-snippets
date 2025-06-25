namespace Crawling.Robots;

internal static class RobotsHandler
{
    public static async Task<Robot> GetDisallowedSites(Uri uri, CancellationToken cancellationToken)
    {
        var baseUri = new Uri(uri.GetLeftPart(UriPartial.Authority));
        var robotsUri = new Uri(baseUri, "/robots.txt");
        
        var client = new HttpClient();
        var robots = await client.GetStringAsync(robotsUri, cancellationToken);
    
        return RobotsParser.Parse(baseUri, robots);
    }
}
