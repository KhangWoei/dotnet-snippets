namespace NpgsqlTesting.Trie;

public class TrieService(string connectionString)
{
    private readonly TrieTreeCommands _treeCommands = new(connectionString);
    private readonly TrieTreeQueries _treeQueries = new(connectionString);
    private readonly NodeCommands _nodeCommands = new(connectionString);
    private readonly NodeQueries _nodeQueries = new(connectionString);

    public async Task CreateTree(Tree tree)
    {
        if (!_treeQueries.TryGet(tree.Name, out var persistedTree))
        {
            persistedTree = await _treeCommands.CreateAsync(new CreateTreeRequest(tree.Name, tree.BaseUrl));
        }

        foreach (var node in tree)
        {
            if (!_nodeQueries.TryGet(persistedTree!.Id, node.Path, out _))
            {
                // parent id is making it tricky to write, might just remove it
                await _nodeCommands.CreateAsync(new CreateNodeRequest(persistedTree.Id, null, node.Path, node.Node.IsTerminal));
            }
        }
    }

}