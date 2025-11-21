using ContextSpider.Graphing.Core;

namespace ContextSpider.Spidering.Core.Tests;

[TestFixture]
public class SpiderTests
{
    /*
     *                                    ┌─────┐
     *                                    │     │
     *                                ┌───┤  4  │
     *                                │   │     │
     *                                │   └──▲──┘
     *                                │      │
     *                             ┌──▼──┐   │
     *                             │     ├───┘
     *            ┌─────┐          │  1  │
     *            │     │       ┌──┤     ◄───┐
     *            │  5  │       │  └─────┘   │
     *            │     │       │            │
     *            └──▲──┘    ┌──▼──┐      ┌──┴──┐
     *               │       │     │      │     ◄───────┐
     *               └───────┤  2  │      │  3  │       │
     *                       │     │      │     │    ┌──┴──┐
     * ┌─────┐    ┌─────┐    └──▲──┘      └──┬──┘    │     │
     * │     │    │     │       │            │       │  8  │
     * │  9  ◄────┤  6  ├───────┘            │       │     │
     * │     │    │     │                 ┌──▼──┐    └──▲──┘
     * └─────┘    └──┬──┘                 │     ├───────┘
     *               │       ┌─────┐      │  7  ├───────┐
     *               │       │     │      │     │    ┌──▼──┐
     *               └───────► 10  │      └──▲──┘    │     │
     *                       │     ├─────────┘       │ 11  │
     *                       └─────┘                 │     │
     *                                               └─────┘
     */
    [Test]
    public void Temp()
    {
        var graphBuilder = new GraphBuilder();
        graphBuilder.AddBidirectedEdge("1", "4");
        graphBuilder.AddDirectedEdge("1", "2");
        
        graphBuilder.AddDirectedEdge("2", "5");
        
        graphBuilder.AddDirectedEdge("3", "1");
        graphBuilder.AddDirectedEdge("3", "7");
        
        graphBuilder.AddDirectedEdge("6", "2");
        graphBuilder.AddDirectedEdge("6", "9");
        graphBuilder.AddDirectedEdge("6", "10");
        
        graphBuilder.AddDirectedEdge("7", "8");
        graphBuilder.AddDirectedEdge("7", "11");
        
        graphBuilder.AddDirectedEdge("8", "3");

        graphBuilder.AddDirectedEdge("10", "7");

        var spider = new Spider(graphBuilder.Build());
        
        spider.Traverse(new Vertex("1"));
    }
}
