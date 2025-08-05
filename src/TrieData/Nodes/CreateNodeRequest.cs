namespace TrieData.Nodes;

internal sealed record CreateNodeRequest(int TreeId, long? ParentId, string Path, bool IsTerminal);
