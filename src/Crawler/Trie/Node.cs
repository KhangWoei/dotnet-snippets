namespace Crawler.Trie;

internal sealed class Node
{
    public Dictionary<string, Node> Children { get; } = new();

    public bool IsTerminal { get; set; }
}