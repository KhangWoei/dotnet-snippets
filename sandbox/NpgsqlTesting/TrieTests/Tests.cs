using NpgsqlTesting.Trie;
using NUnit.Framework;

namespace NpgsqlTesting.TrieTests;

[TestFixture]
public class Tests
{
    [TestCase]
    public async Task CreateAndGet()
    {
        var command = new TrieTreeCommands();
        var queries = new TrieTreeQueries();

        var expected = new TrieTreeModel("test", "some_url");
        await command.Create(expected);

        var actual = await queries.Get(expected.Name);
        
        Assert.Multiple(() =>
        {
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.BaseUrl, Is.EqualTo(expected.BaseUrl));
        });
    }
}