namespace Crawling.Robots;

internal static class RobotsParser
{
    public static Robot Parse(Uri baseUri, string content)
    {
        var lines = content.Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        
        var disallowed = new HashSet<Uri>();
        var isWildCard = false;
        int? delay = null;
        
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith('#'))
            {
                continue;
            }

            if (line.StartsWith("user-agent:", StringComparison.InvariantCultureIgnoreCase))
            {
                isWildCard = ProcessKeyValue(line, "user-agent:") == "*";
                continue;
            }
            
            if (isWildCard)
            {
                if (line.StartsWith("crawl-delay:", StringComparison.InvariantCultureIgnoreCase) )
                {
                    delay = int.Parse(ProcessKeyValue(line, "crawl-delay:"));
                    continue;
                }
                
                var disallowedSite = ProcessKeyValue(line, "disallow:");
                
                if (!string.IsNullOrWhiteSpace(disallowedSite))
                {
                    if (baseUri.TryCreateRelativeOrAbsolute(disallowedSite, out var output))
                    {
                        disallowed.Add(output);
                    }
                }
            }
        }
        
        return new Robot(disallowed, delay);
    }

    private static string ProcessKeyValue(string line, string prefix)
    {
        if (line.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
        {
            var parts = line.Split(":", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2)
            {
                return parts[1];
            }
        }

        return string.Empty;
    }
}
