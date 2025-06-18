using HtmlAgilityPack;

namespace Crawler.Harvesting;

internal static class LinkHarvester
{
    public static IEnumerable<Uri> Harvest(string html)
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

            if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
            {
                yield return uri;
            }
        }
    }
}
