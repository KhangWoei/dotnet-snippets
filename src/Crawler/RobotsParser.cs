using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Crawler;

internal static class RobotsParser
{
    private static readonly Regex _userAgentPattern = new Regex(@"^User agent: (?<agent>\*)$");
    private static readonly Regex _disallowPattern = new Regex(@"^disallow:(?<value>.+)$", RegexOptions.IgnoreCase);


    public static async IAsyncEnumerable<string> ParseAsync(Stream stream, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var userAgent = string.Empty;
        using var reader = new StreamReader(stream);
        string line;

        while ((line = await reader.ReadLineAsync()) is not null)
        {
            var agentMatch = _userAgentPattern.Match(line);
            if (_userAgentPattern.Match(line).Success)
            {
                userAgent = agentMatch.Groups["agent"].Value.Trim();
            }

            if (TryParseLine(line, out var output))
            {
                yield return output;
            }
        }
    }

    public static IEnumerable<string> Parse(string content)
    {
        var userAgent = string.Empty;
        var lines = content.Split(Environment.NewLine);

        foreach (var line in lines)
        {
            var agentMatch = _userAgentPattern.Match(line);
            if (_userAgentPattern.Match(line).Success)
            {
                userAgent = agentMatch.Groups["agent"].Value.Trim();
            }

            if (userAgent == "*" && TryParseLine(line, out var output))
            {
                yield return output;
            }
        }
    }

    private static bool TryParseLine(string line, [MaybeNullWhen(false)] out string output)
    {
        output = null;

        var match = _disallowPattern.Match(line);
        if (match.Success)
        {
            output = match.Groups["value"].Value.Trim();
            return true;
        }

        return false;
    }
}
