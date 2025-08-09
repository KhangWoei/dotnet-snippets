using System.Collections;
using System.Text;

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
        var parts = value.AbsolutePath.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

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

    public string ToDotNotation()
    {
        var nodes = new StringBuilder();
        foreach (var node in this)
        {
            var children = new StringBuilder();
            foreach (var child in node.Children.Values)
            {
                children.Append($"\"{child.Path}\"");
                children.Append(' ');
            }

            if (children.Length != 0)
            {
                nodes.Append($"\"{node.Path}\" -> {{ {children} }}");
                nodes.AppendLine();
            }
        }

        return $$"""
                 digraph {
                    {{nodes}}
                 }
                 """;
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