using ContextSpider.Graphing.Core;

namespace ContextSpider.Spidering.Core;

public sealed class Spider(Graph graph)
{
    private readonly Frontier _frontier = new Frontier(graph);
    
    public void Traverse(Vertex starting)
    {
        var vertex = graph.Vertices.Single(v => v.Name == starting.Name);
        _frontier.Add(vertex);

        while (_frontier.HasNext())
        {
            var next = _frontier.Next();
            Visit(next);
        }
    }

    public void Visit(Vertex vertex)
    {
        Console.WriteLine($"Visiting {vertex.Name}");
        
        // TODO lots of vertices are going to be revisited from this
        foreach (var neighbor in graph.GetNeighbors(vertex))
        {
            _frontier.Add(neighbor);
        }
    }
}

// TODO: Not a fan of this because it is coupled to graph, and using HashSets like a queue
internal sealed class Frontier(Graph graph)
{
    private readonly HashSet<Vertex> _visited = [];
    private readonly HashSet<Vertex> _frontier = [];

    public void Add(Vertex vertex)
    {
        if (!_visited.Contains(vertex))
        {
            _frontier.Add(vertex);
        }
    }
    
    public Vertex Next()
    {
        var next = _frontier.Select(v =>
            {
                var neighbors = graph.GetNeighbors(v).ToList();
                var visited = neighbors.Count(visited => _visited.Contains(visited));
                var unvisited = neighbors.Count - visited;

                return new
                {
                    Vertex = v,
                    Visited = visited,
                    Unvisited = unvisited
                };
            })
            .OrderBy(v => v.Visited + v.Unvisited)
            .ThenBy(v => v.Visited)
            .ThenBy(v => v.Unvisited)
            .ThenBy(v => v.Vertex.Name)
            .First()
            .Vertex;

        _visited.Add(next);
        _frontier.Remove(next);
        
        return next;
    }

    public bool HasNext() => _frontier.Count > 0;
}
