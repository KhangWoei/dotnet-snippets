namespace Crawler.Tests.Trie;

[TestFixture]
public sealed class TrieTests
{
    [Test]
    public void TryInsert_WhenBaseUriIsIncompatible_ReturnsFalse()
    {
        var uri = new Uri("http://www.contoso.com");
        var tree = TrieTree.Trie.Create(uri);

        var insertedUri = new Uri("http://www.google.com");

        Assert.That(tree.TryInsert(insertedUri), Is.False);
    }

    [Test]
    public void TryInsert_WhenBaseUriIsCompatible_ReturnsTrue()
    {
        var uri = new Uri("http://www.contoso.com");
        var tree = TrieTree.Trie.Create(uri);

        var insertedUri = new Uri("http://www.contoso.com/test");

        Assert.That(tree.TryInsert(insertedUri), Is.True);
    }

    // This test is testing a lot of things, multiple points of failure.
    [TestCase("http://www.contoso.com", "http://www.contoso.com/test", "http://www.contoso.com/test", true)]
    [TestCase("http://www.contoso.com", "", "http://www.contoso.com/test", false)]
    [TestCase("http://www.contoso.com/test", "", "http://www.contoso.com/test", true)]
    [TestCase("http://www.contoso.com", "", "http://www.contoso.com", true)]
    [TestCase("http://www.contoso.com", "", "http://www.google.com", false)]
    public void Contains(string baseUri, string insert, string uriToCheck, bool expected)
    {
        var tree = TrieTree.Trie.Create(new Uri(baseUri));

        if (!string.IsNullOrEmpty(insert))
        {
            tree.TryInsert(new Uri(insert));
        }
        
        Assert.That(tree.Contains(new Uri(uriToCheck)), Is.EqualTo(expected));
    }
}