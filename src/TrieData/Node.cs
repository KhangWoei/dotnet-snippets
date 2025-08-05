namespace TrieData;

public sealed class Node(string path = "", bool isTerminal = false, Node? parent = null)
{
    public string Path { get;  } = path;
    public bool IsTerminal { get; set; } = isTerminal;
    public Node? Parent { get; set; } = parent;
    public Dictionary<string, Node> Children { get; } = new();
    
}