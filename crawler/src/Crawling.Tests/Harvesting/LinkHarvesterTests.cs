using Crawling.Harvesting;

namespace Crawler.Tests.Harvesting;

[TestFixture]
public sealed class LinkHarvesterTests
{
    [Test]
    public void HarvestsHrefValue()
    {
        var expected = new Uri[]
        {
            new Uri("https://www.contoso.com"),
            new Uri("https://www.contoso-anchor.com")
        };

        var html = """
                   <link href="https://www.contoso.com">
                   <a href="https://www.contoso-anchor.com">text<a/>
                   """;
        var actual = LinkHarvester.Harvest(html);
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}
