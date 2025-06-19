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
        throw new NotImplementedException();
    }

    public bool Contains(Uri value)
    {
        throw new NotImplementedException();
    }
    
}