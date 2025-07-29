using NpgsqlTesting.Trie;
using NUnit.Framework;

namespace NpgsqlTesting.TrieTests;

public class Tests
{
    [TestFixture]
    public class TreeTests
    {
        [TestCase]
        public async Task CreateAndGet()
        {
            var testDatabaseHelper =
                new TestDatabaseHelper("Host=localhost,5432;Username=postgres;Password=Password@1;");
            var databaseConnection = await testDatabaseHelper.CreateDatabaseAsync();

            var command = new TrieTreeCommands(databaseConnection);
            var queries = new TrieTreeQueries(databaseConnection);

            var request = new CreateTreeRequest("test", "some_value");
            await command.CreateAsync(request);

            var actual = await queries.GetAsync(request.Name);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Name, Is.EqualTo(request.Name));
                Assert.That(actual.BaseUrl, Is.EqualTo(request.BaseUrl));
            });

            await testDatabaseHelper.Cleanup();
        }
    }
    }
}