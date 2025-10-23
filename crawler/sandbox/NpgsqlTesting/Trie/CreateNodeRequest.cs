namespace NpgsqlTesting.Trie;

public record CreateNodeRequest(int TreeId, long? ParentId, string Path, bool IsTerminal);