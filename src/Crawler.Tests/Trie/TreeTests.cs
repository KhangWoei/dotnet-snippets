using Crawler.Trie;

namespace Crawler.Tests.Trie;

[TestFixture]
public sealed class TreeTests
{
    [Test]
    public void TryInsert_WhenBaseUriIsIncompatible_ReturnsFalse()
    {
        var uri = new Uri("http://www.contoso.com");
        var tree = Tree.Create(uri);

        var insertedUri = new Uri("http://www.google.com");

        Assert.That(tree.TryInsert(insertedUri), Is.False);
    }

    [Test]
    public void TryInsert_WhenBaseUriIsCompatible_ReturnsTrue()
    {
        var uri = new Uri("http://www.contoso.com");
        var tree = Tree.Create(uri);

        var insertedUri = new Uri("http://www.contoso.com/test");

        Assert.That(tree.TryInsert(insertedUri), Is.True);
    }
}