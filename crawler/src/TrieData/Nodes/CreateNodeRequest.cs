namespace TrieData.Nodes;

public sealed record CreateNodeRequest(int TreeId, long? ParentId, string Path, bool IsTerminal);
