namespace Crawler.TrieTree;

internal sealed class Trie
{
    private readonly Node _root;
    private readonly string _baseUri;

    private Trie(string baseUri)
    {
        _baseUri = baseUri;
        _root = new();
    }

    public static Trie Create(Uri uri)
    {
        var baseUri = uri.GetLeftPart(UriPartial.Authority);

        var tree = new Trie(baseUri);
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
        var parts = value.AbsolutePath.Split('/', StringSplitOptions.TrimEntries);

        foreach (var part in parts)
        {
            if (!current.Children.TryGetValue(part, out var child))
            {
                child = new Node();
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
    
    private bool IsBaseOf(Uri value)
    {
        return value.GetLeftPart(UriPartial.Authority) == _baseUri;
    }
}