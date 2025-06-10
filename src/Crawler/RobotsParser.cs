using System.Runtime.CompilerServices;

namespace Crawler;

internal static class RobotsParser
{   
    public static async IAsyncEnumerable<string> ParseAsync(Stream stream, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(stream);
        string line;

        while ((line = await reader.ReadLineAsync()) is not null)
        {
            if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                continue;

            var parts = line.Split(':', 2);
            if (parts.Length != 2)
                continue;
            var key = parts[0].Trim().ToLower();
            var value = parts[1].Trim();
            
            if (key == "disallow" && !string.IsNullOrEmpty(value))
            {
                yield return value;
            }
        }
    }
}
