namespace TrieData.Nodes;

internal sealed record NodeModel(long Id, long? ParentId, string Path, bool IsTerminal);