namespace Crawling.Robots;

internal static class RobotsHandler
{
    public static async Task<Robot> GetDisallowedSites(HttpClient client, Uri uri, CancellationToken cancellationToken)
    {
        var baseUri = new Uri(uri.GetLeftPart(UriPartial.Authority));
        var robotsUri = new Uri(baseUri, "/robots.txt");
        var robots = await client.GetStringAsync(robotsUri, cancellationToken);
    
        return RobotsParser.Parse(baseUri, robots);
    }
}
