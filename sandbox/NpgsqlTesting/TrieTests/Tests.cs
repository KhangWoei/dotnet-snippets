using NpgsqlTesting.Trie;
using NUnit.Framework;

namespace NpgsqlTesting.TrieTests;

[TestFixture]
public class Tests
{
    [TestCase]
    public async Task CreateAndGet()
    {
        var testDatabaseHelper = new TestDatabaseHelper("Host=localhost,5432;Username=postgres;Password=Password@1;");
        var databaseConnection = await testDatabaseHelper.CreateDatabaseAsync();
        
        var command = new TrieTreeCommands(databaseConnection);
        var queries = new TrieTreeQueries(databaseConnection);

        var expected = new TrieTreeModel("test", "some_url");
        await command.CreateAsync(expected);

        var actual = await queries.GetAsync(expected.Name);
        
        Assert.Multiple(() =>
        {
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.BaseUrl, Is.EqualTo(expected.BaseUrl));
        });

        await testDatabaseHelper.Cleanup();
    }
}