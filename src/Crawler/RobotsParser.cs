namespace Crawler;

internal static class RobotsParser
{
    public static IEnumerable<string> Parse(string content)
    {
        var lines = content.Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        var isWildCard = false;
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
                var value = ProcessKeyValue(line, "disallow:");

                if (!string.IsNullOrWhiteSpace(value))
                {
                    yield return value;
                }
            }
        }
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
