using Npgsql;

namespace TrieData.Nodes;

internal sealed class NodesWriteRepository(NpgsqlDataSource dataSource) : INodesWriteRepository
{

    public async Task<INodeModel> CreateAsync(CreateNodeRequest request, CancellationToken cancellationToken = default)
    {
        await using var command = dataSource.CreateCommand();
        command.CommandText = """
                              INSERT INTO nodes (tree_id, parent_id, full_path, is_terminal)
                              VALUES (@tree, @parent, @path, @terminal)
                              RETURNING id;
                              """;

        command.Parameters.AddWithValue("tree", request.TreeId);
        command.Parameters.AddWithValue("parent", request.ParentId ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("path", request.Path);
        command.Parameters.AddWithValue("terminal", request.IsTerminal);

        // The query should never return null, if it did we should expect a database exception
        var result = (int)(await command.ExecuteScalarAsync(cancellationToken))!;
        return new NodeModel(result, request.ParentId, request.Path, request.IsTerminal);
    }
}

public interface INodesWriteRepository
{
    Task<INodeModel> CreateAsync(CreateNodeRequest request, CancellationToken cancellationToken);
}
