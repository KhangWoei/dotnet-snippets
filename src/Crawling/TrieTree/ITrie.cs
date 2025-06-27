namespace Crawling.TrieTree;

public interface ITrie
{
    static abstract ITrie Create(Uri uri);
    
    bool TryInsert(Uri value);

    bool Contains(Uri value);
}