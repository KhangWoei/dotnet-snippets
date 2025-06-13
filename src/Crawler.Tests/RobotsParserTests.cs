namespace Crawler.Tests;

[TestFixture]
public class RobotsParserTests
{
    // TODO: I can probably test case this instead.
    [Test]
    public void DisallowListForAllBots_Parsed()
    {
        var baseUri = new Uri("https://www.contoso.com");
        var robots = """
                     User-agent: *
                     
                     Disallow: /test 
                     """;

        var expected = new[] { new Uri("https://www.contoso.com/test") };

        var actual = RobotsParser.Parse(baseUri, robots);

        Assert.That(actual.Disallowed, Is.EquivalentTo(expected));
    }

    [Test]
    public void Comments_Ignored()
    {
        var baseUri = new Uri("https://www.contoso.com");
        var robots = """
                     User-agent: *
                     #Disallow: test
                     """;

        var expected = Array.Empty<Uri>();

        var actual = RobotsParser.Parse(baseUri, robots);

        Assert.That(actual.Disallowed, Is.EquivalentTo(expected));
    }

    [Test]
    public void DisallowListForSpecificBots_Ignored()
    {
        
        var baseUri = new Uri("https://www.contoso.com");
        var robots = """
                     User-agent: some bot

                     Disallow: test

                     User-agent: *
                     
                     Disallow: /included
                     """;

        var expected = new[] { new Uri("https://www.contoso.com/included") };

        var actual = RobotsParser.Parse(baseUri, robots);

        Assert.That(actual.Disallowed, Is.EquivalentTo(expected));
    }
}

