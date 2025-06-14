using HtmlAgilityPack;

namespace Crawler.Harvesting;

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
            
            if (baseUri.TryCreateRelativeOrAbsolute(value, out var output))
            {
                yield return output;
            }
        }
    }
}
