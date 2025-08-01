namespace NpgsqlTesting.Trie;

public record Node(Node? Parent, Dictionary<string, Node> Children, bool IsTerminal, string Path)
{
    public bool IsTerminal { get; set; } = IsTerminal;
}