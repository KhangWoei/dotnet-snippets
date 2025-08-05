namespace TrieData;

public sealed class Trie(Uri uri) : ITrie
{
    private readonly Node _root = new ();

    public static ITrie Create(Uri uri)
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
    
    private bool IsBaseOf(Uri value) => uri.IsBaseOf(value);
}