namespace Crawler;

internal static class Robots
{
    public static async Task<HashSet<Uri>> GetDisallowedSites(Uri uri, CancellationToken cancellationToken)
    {
        var baseUri = new Uri(uri.GetLeftPart(UriPartial.Authority));
        var robotsUri = new Uri(baseUri, "/robots.txt");
        
        var client = new HttpClient();
        var robots = await client.GetStringAsync(robotsUri, cancellationToken);
        var disallowed = new HashSet<Uri>();

        foreach (var endpoint in RobotsParser.Parse(robots))
        {
            var disallowedUri = new Uri(baseUri, endpoint);
            
            disallowed.Add(disallowedUri);
        }

        return disallowed;
    }
}
