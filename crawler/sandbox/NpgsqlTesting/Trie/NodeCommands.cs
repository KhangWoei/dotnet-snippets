using Npgsql;

namespace NpgsqlTesting.Trie;

public class NodeCommands(string connectionString)
{
    public async Task<NodeModel> CreateAsync(CreateNodeRequest request, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO nodes (tree_id, parent_id, full_path, is_terminal)
                              VALUES (@tree, @parent, @path, @terminal);
                              RETURNING id;
                              """;
        
        command.Parameters.AddWithValue("tree", request.TreeId);
        command.Parameters.AddWithValue("parent", request.ParentId ?? (object) DBNull.Value);
        command.Parameters.AddWithValue("path", request.Path);
        command.Parameters.AddWithValue("terminal", request.IsTerminal);

        // The query should never return null, if it did we should expect a database exception
        var result = (int)(await command.ExecuteScalarAsync(cancellationToken))!;
        return new NodeModel(result, request.Path, request.IsTerminal);
    }
}