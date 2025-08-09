using System.Collections;

namespace TrieData;

public sealed class Trie(Uri uri) : IEnumerable<Node>
{
    private readonly Node _root = new (path: "/");

    public string Name { get; } = uri.Host;

    public static Trie Create(Uri uri)
    {
        var uriBuilder = new UriBuilder(uri)
        {
            Fragment = string.Empty,
            Query = string.Empty,
            Path = string.Empty,
        };

        var tree = new Trie(uriBuilder.Uri);
        tree.TryInsert(uri);

        return tree;
    }

    public bool TryInsert(Uri value)
    {
        if (!IsBaseOf(value))
        {
            return false;
        }

        var current = _root;
        var parts = value.AbsolutePath.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts)
        {
            if (!current.Children.TryGetValue(part, out var child))
            {
                child = new Node(part, parent: current);
                current.Children[part] = child;
            }

            current = child;
        }

        current.IsTerminal = true;

        return true;
    }

    public bool Contains(Uri value)
    {
        if (!IsBaseOf(value))
        {
            return false;
        }
        
        var current = _root;
        var parts = value.AbsolutePath.Split('/', StringSplitOptions.TrimEntries);

        foreach (var part in parts)
        {
            if (!current.Children.TryGetValue(part, out var child))
            {
                current = null;
                break;
            }

            current = child;
        }
        
        return current?.IsTerminal ?? false;
    }
    
    private bool IsBaseOf(Uri value) => uri.IsBaseOf(value);
    
    public IEnumerator<Node> GetEnumerator()
    {
        return new TreeIterator(_root);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}