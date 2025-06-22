namespace Crawler.Trie;

internal sealed class Tree
{
    private readonly Node _root;
    private readonly string _baseUri;

    private Tree(string baseUri)
    {
        _baseUri = baseUri;
        _root = new Node();
    }

    public static Tree Create(Uri uri)
    {
        var baseUri = uri.GetLeftPart(UriPartial.Authority);

        var tree = new Tree(baseUri);
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
        var parts = value.AbsolutePath.Split('/');

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
        var parts = value.AbsolutePath.Split('/');

        foreach (var part in parts)
        {
            if (!current.Children.TryGetValue(part, out var child))
            {
                break;
            }

            current = child;
        }
        
        return current.IsTerminal;
    }
    
    private bool IsBaseOf(Uri value)
    {
        return value.GetLeftPart(UriPartial.Authority) == _baseUri;
    }
}