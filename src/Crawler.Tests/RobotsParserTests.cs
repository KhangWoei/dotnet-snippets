namespace Crawler.Tests;

[TestFixture]
public class RobotsParserTests
{
    [Test]
    public void DisallowListForAllBots_Parsed()
    {
        var robots = """
                     User-agent: *
                     
                     Disallow: test 
                     """;

        var expected = new string[] { "test" };

        var actual = RobotsParser.Parse(robots).ToArray();

        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void Comments_Ignored()
    {
        var robots = """
                     User-agent: *
                     #Disallow: test
                     """;

        var expected = Array.Empty<string>();

        var actual = RobotsParser.Parse(robots).ToArray();

        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void DisallowListForSpecificBots_Ignored()
    {
        var robots = """
                     User-agent: some bot

                     Disallow: test

                     User-agent: *
                     
                     Disallow: included
                     """;

        var expected = new string[] { "included" };

        var actual = RobotsParser.Parse(robots).ToArray();

        Assert.That(actual, Is.EquivalentTo(expected));
    }
}

