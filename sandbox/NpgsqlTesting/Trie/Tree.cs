using System.Collections;

namespace NpgsqlTesting.Trie;

public record Tree(string Name, string BaseUrl) : IEnumerable<(Node Node, string Path)>
{
    private readonly Node _root = new(null, [], true, "");

    public bool TryInsert(string value)
    {
        var current = _root;
        var parts = value.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            if (!current.Children.TryGetValue(part, out var child))
            {
                child = new Node(current, [], false, part);
                current.Children[part] = child;
            }

            current = child;
        }

        current.IsTerminal = true;
        return true;
    }
    
    public IEnumerator<(Node Node, string Path)> GetEnumerator()
    {
        return new TreeIterator(_root);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
