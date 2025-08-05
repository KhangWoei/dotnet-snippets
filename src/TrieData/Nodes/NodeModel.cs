namespace TrieData.Nodes;

internal sealed record NodeModel(long Id, long? ParentId, string Path, bool IsTerminal) : INodeModel;

public interface INodeModel
{
    long Id { get; }
    long? ParentId { get; }
    string Path { get; }
    bool IsTerminal { get; }
}