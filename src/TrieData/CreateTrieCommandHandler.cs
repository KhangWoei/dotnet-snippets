using MediatR;
using TrieData.Nodes;
using TrieData.Tries;

namespace TrieData;

public class CreateTrieCommandHandler(
    ITriesWriteRepository triesWriteRepository, 
    ITriesReadRepository triesReadRepository, 
    INodesWriteRepository nodesWriteRepository, 
    INodesReadRepository nodesReadRepository) : IRequestHandler<CreateTrieCommandRequest>
{
    public async Task Handle(CreateTrieCommandRequest request, CancellationToken cancellationToken)
    {
        if (!triesReadRepository.TryGet(request.Trie.Name, out var persistedTree))
        {
            persistedTree = await triesWriteRepository.CreateAsync(request.Trie.Name, cancellationToken);
        }

        var nodesLookup = new Dictionary<Node, long>();
        foreach (var node in request.Trie)
        {
            if (!nodesReadRepository.TryGet(persistedTree.Id, node.Path, out var nodeModel))
            {
                // TODO: In other scenarios where we'd update or add a node independently, we need to query the DB for the parent instead using the treeId and the fullPath
                long? parentId = node.Node.Parent is null ? null : nodesLookup.GetValueOrDefault(node.Node.Parent!);
                nodeModel = await nodesWriteRepository.CreateAsync(new CreateNodeRequest(persistedTree.Id, parentId, node.Path, node.Node.IsTerminal), cancellationToken);
            }
            
            nodesLookup.Add(node.Node, nodeModel.Id);
        }
    }
}