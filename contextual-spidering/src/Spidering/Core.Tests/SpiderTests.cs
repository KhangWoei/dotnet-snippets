using ContextSpider.Graphing.Core;

namespace ContextSpider.Spidering.Core.Tests;

[TestFixture]
public class SpiderTests
{
    [Test]
    public void SingleVertex()
    {
        var graph = new GraphBuilder()
            .AddVertex("A")
            .Build();

        var order = new Spider(graph).Traverse(new Vertex("A"));

        var actual = order.FirstOrDefault();
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual.Name, Is.EqualTo("A"));
    }
    
    [Test]
    public void DisconnectedVertices_OnlyVisitsReachableNodes()
    {
        var graph = new GraphBuilder()
            .AddVertex("A")
            .AddDirectedEdge("B", "C")
            .Build();
        
        var order = new Spider(graph).Traverse(new Vertex("A"));

        Assert.That(order, Has.Length.EqualTo(1));
        
        var actual = order.First();
        Assert.That(actual.Name, Is.EqualTo("A"));
    }
    
    [Test]
    public void LeastNeighbors_HasHigherPriority()
    {
        var graph = new GraphBuilder()
            .AddBidirectedEdge("A", "B")
            .AddBidirectedEdge("A", "C")
            .AddBidirectedEdge("B", "D")
            .AddBidirectedEdge("C", "D")
            .AddBidirectedEdge("C", "E")
            .Build();
        
        var spider = new Spider(graph);
        var order = spider.Traverse(new Vertex("A"));
        
        Assert.That(order[0].Name, Is.EqualTo("A"));
        Assert.That(order[3].Name, Is.Not.EqualTo("D"));
    }
    
    [Test]
    public void FewestUnvisitedNeighbors_HasHigherPriority()
    {
        var graph = new GraphBuilder()
            .AddBidirectedEdge("A", "B")
            .AddBidirectedEdge("A", "C")
            .AddBidirectedEdge("B", "D")
            .AddBidirectedEdge("C", "D")
            .AddBidirectedEdge("D", "E")
            .AddBidirectedEdge("D", "F")
            .AddBidirectedEdge("E", "G")
            .AddBidirectedEdge("E", "H")
            .Build();
        
        var spider = new Spider(graph);
        var order = spider.Traverse(new Vertex("A"));
        
        Assert.That(order[0].Name, Is.EqualTo("A"));
        Assert.That(order[3].Name, Is.EqualTo("D"));
    }
}
