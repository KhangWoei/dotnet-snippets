using System.Diagnostics;

namespace ContextSpider.Graphing.Core;

public sealed class Graph
{
    private readonly HashSet<Vertex> _vertices = [];
    private readonly List<Edge> _edges = [];

    public IEnumerable<Vertex> Vertices => _vertices.AsEnumerable();

    public IEnumerable<Edge> Edges => _edges;

    public IEnumerable<Vertex> GetNeighbors(Vertex vertex) => _edges.Where(e => 
            e.Source == vertex || e.Target == vertex)
        .SelectMany(e => new[] { e.Source, e.Target })
        .Except([vertex]);

    public void AddVertex(Vertex vertex) => _vertices.Add(vertex);

    public void AddVertices(params Vertex[] vertices) => _vertices.UnionWith(vertices);

    public void AddEdge(Edge edge)
    {
        _vertices.Add(edge.Source);
        _vertices.Add(edge.Target);
        _edges.Add(edge);
    }

    public void AddEdges(params Edge[] edges)
    {
        var vertices = _edges.SelectMany(e => new[] { e.Source, e.Target });
        _vertices.UnionWith(vertices);
        _edges.AddRange(edges);
    }
}

[DebuggerDisplay("{Name}")]
public sealed class Vertex(string name)
{
    public string Name { get; } = name;
};

[DebuggerDisplay("{Source}->{Target}")]
public sealed class Edge(Vertex source, Vertex target)
{
    public Vertex Source { get; } = source;
    public Vertex Target { get; } = target;
}
