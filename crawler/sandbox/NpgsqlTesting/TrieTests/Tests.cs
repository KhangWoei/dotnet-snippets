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

    [TestFixture]
    public class NodeTests
    {
        [TestCase]
        public async Task CreateAndGet_NoParent()
        {
            var testDatabaseHelper =
                new TestDatabaseHelper("Host=localhost,5432;Username=postgres;Password=Password@1;");
            var databaseConnection = await testDatabaseHelper.CreateDatabaseAsync();

            var treeCommands = new TrieTreeCommands(databaseConnection);
            var treeQueries = new TrieTreeQueries(databaseConnection);
            var nodeCommands = new NodeCommands(databaseConnection);
            var nodeQueries = new NodeQueries(databaseConnection);

            var createTreeRequest = new CreateTreeRequest("test", "some_url");
            await treeCommands.CreateAsync(createTreeRequest);

            var tree = await treeQueries.GetAsync(createTreeRequest.Name);

            var createNodeRequest = new CreateNodeRequest(tree.Id, null, "/", false);
            await nodeCommands.CreateAsync(createNodeRequest);

            var actual = await nodeQueries.GetAsync(tree.Id, "/");

            Assert.Multiple(() =>
            {
                Assert.That(actual.Path, Is.EqualTo(createNodeRequest.Path));
                Assert.That(actual.IsTerminal, Is.EqualTo(createNodeRequest.IsTerminal));
            });
        }

        [TestCase]
        public async Task CreateAndGet_WithParent()
        {
            var testDatabaseHelper =
                new TestDatabaseHelper("Host=localhost,5432;Username=postgres;Password=Password@1;");
            var databaseConnection = await testDatabaseHelper.CreateDatabaseAsync();

            var treeCommands = new TrieTreeCommands(databaseConnection);
            var treeQueries = new TrieTreeQueries(databaseConnection);
            var nodeCommands = new NodeCommands(databaseConnection);
            var nodeQueries = new NodeQueries(databaseConnection);

            var createTreeRequest = new CreateTreeRequest("test", "some_url");
            await treeCommands.CreateAsync(createTreeRequest);

            var tree = await treeQueries.GetAsync(createTreeRequest.Name);
            var createRootNodeRequest = new CreateNodeRequest(tree.Id, null, "/", false);
            await nodeCommands.CreateAsync(createRootNodeRequest);

            var root = await nodeQueries.GetAsync(tree.Id, createRootNodeRequest.Path);
            var createChildNodeRequest = new CreateNodeRequest(tree.Id, root.Id, "/test", true);
            await nodeCommands.CreateAsync(createChildNodeRequest);
            var actual = await nodeQueries.GetAsync(tree.Id, createChildNodeRequest.Path);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Path, Is.EqualTo(createChildNodeRequest.Path));
                Assert.That(actual.IsTerminal, Is.EqualTo(createChildNodeRequest.IsTerminal));
            });
        }
    }

    [TestFixture]
    public class TrieServiceTests
    {
        [TestCase]
        public async Task CreateTree()
        {
            var testDatabaseHelper =
                new TestDatabaseHelper("Host=localhost,5432;Username=postgres;Password=Password@1;");
            var databaseConnection = await testDatabaseHelper.CreateDatabaseAsync();

            var service = new TrieService(databaseConnection);
            
            var treeQueries = new TrieTreeQueries(databaseConnection);
            var nodeQueries = new NodeQueries(databaseConnection);

            var tree = new Tree("test", "some_url");
            tree.TryInsert("/some/random/path");
            tree.TryInsert("/random/path");
            
            await service.CreateTree(tree);

            using (Assert.EnterMultipleScope())
            {
                var actualTree = await treeQueries.GetAsync(tree.Name);
                Assert.That(actualTree.Name, Is.EqualTo(tree.Name));
                Assert.That(actualTree.BaseUrl, Is.EqualTo(tree.BaseUrl));
                
                foreach (var (node, path) in tree)
                {
                    var actualNode = await nodeQueries.GetAsync(actualTree.Id, path);
                    Assert.That(actualNode.IsTerminal, Is.EqualTo(node.IsTerminal));
                    Assert.That(actualNode.Path, Is.EqualTo(path));
                }
            }
        }
    }
}