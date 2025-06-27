namespace Crawling.TrieTree;

public interface ITrie
{
    bool TryInsert(Uri value);

    bool Contains(Uri value);
}