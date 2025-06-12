using HtmlAgilityPack;

namespace Crawler;

internal static class LinkHarvester
{
    public static IEnumerable<Uri> Harvest(Uri baseUri, string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var nodes = document.DocumentNode.SelectNodes("//*[@href]");

        if (nodes is null)
        {
            yield break;
        }

        foreach (var link in nodes)
        {
            var attribute = link.Attributes["href"];
            var value = attribute.Value;

            if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
            {
                yield return new Uri(value, UriKind.Absolute);
            }

            if (Uri.IsWellFormedUriString(value, UriKind.Relative))
            {
                var relative = new Uri(value, UriKind.Relative);
                yield return new Uri(baseUri, relative);
            }
        }
    }
}
