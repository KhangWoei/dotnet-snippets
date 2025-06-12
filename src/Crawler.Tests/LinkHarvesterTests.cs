namespace Crawler.Tests;

[TestFixture]
public sealed class LinkHarvesterTests
{
    [Test]
    public void AbsoluteLinks()
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
        var actual = LinkHarvester.Harvest(new Uri("https://base.com"), html);
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void RelativeLinks()
    {
        var baseUri = new Uri("https://www.contoso.com");
        var expected = new Uri[]
        {
            new Uri("https://www.contoso.com/back"),
            new Uri("https://www.contoso.com/front"),
        };

        var html = """
                   <link href="back">
                   <a href="front">text</a>
                   """;
        var actual = LinkHarvester.Harvest(baseUri, html);
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}
