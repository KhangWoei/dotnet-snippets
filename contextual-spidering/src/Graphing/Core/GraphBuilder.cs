namespace ContextSpider.Graphing.Core;

public class GraphBuilder
{
    private readonly Graph _graph = new Graph();
    private readonly Dictionary<string, Vertex> _vertices = [];

    public GraphBuilder AddVertex(string name)
    {
        if (!_vertices.ContainsKey(name))
        {
            var vertex = new Vertex(name);
            _vertices[name] = vertex;
            _graph.AddVertex(vertex);
        }

        return this;
    }
    
    public GraphBuilder AddDirectedEdge(string source, string target)
    {
        if (!_vertices.TryGetValue(source, out var sourceVertex))
        {
            sourceVertex = new Vertex(source);
            _vertices[source] = sourceVertex;
        }
        
        if (!_vertices.TryGetValue(target, out var targetVertex))
        {
            targetVertex = new Vertex(target);
            _vertices[target] = targetVertex;
        }
        
        _graph.AddEdge(new Edge(sourceVertex, targetVertex));

        return this;
    }

    public GraphBuilder AddBidirectedEdge(string vertexName, string otherVertexName)
    {
        if (!_vertices.TryGetValue(vertexName, out var vertex))
        {
            vertex = new Vertex(vertexName);
            _vertices[vertexName] = vertex;
        }
        
        if (!_vertices.TryGetValue(otherVertexName, out var otherVertex))
        {
            otherVertex = new Vertex(otherVertexName);
            _vertices[otherVertexName] = otherVertex;
        }
        
        _graph.AddEdge(new Edge(vertex, otherVertex));
        _graph.AddEdge(new Edge(otherVertex, vertex));

        return this;
    }

    public Graph Build() => _graph;
}
