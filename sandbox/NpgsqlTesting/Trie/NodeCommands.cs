using Npgsql;

namespace NpgsqlTesting.Trie;

public class NodeCommands(string connectionString)
{
    public async Task CreateAsync(CreateNodeRequest createNodeRequest)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO nodes (tree_id, parent_id, full_path, is_terminal)
                              VALUES (@tree, @parent, @path, @terminal);
                              """;
        
        command.Parameters.AddWithValue("tree", createNodeRequest.TreeId);
        command.Parameters.AddWithValue("parent", createNodeRequest.ParentId);
        command.Parameters.AddWithValue("path", createNodeRequest.Path);
        command.Parameters.AddWithValue("terminal", createNodeRequest.IsTerminal);

        await command.ExecuteNonQueryAsync();
    }
}