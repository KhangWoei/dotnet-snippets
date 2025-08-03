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

        var nodesLookup = new Dictionary<Node, long>();
        foreach (var node in tree)
        {
            if (!_nodeQueries.TryGet(persistedTree.Id, node.Path, out var nodeModel))
            {
                // TODO: In other scenarios where we'd update or add a node independently, we need to query the DB for the parent instead using the treeId and the fullPath
                long? parentId = node.Node.Parent is null ? null : nodesLookup.GetValueOrDefault(node.Node.Parent!);
                nodeModel = await _nodeCommands.CreateAsync(new CreateNodeRequest(persistedTree.Id, parentId, node.Path, node.Node.IsTerminal));
            }
            
            nodesLookup.Add(node.Node, nodeModel.Id);
        }
    }
}