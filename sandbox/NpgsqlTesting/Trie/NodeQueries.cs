using System.Data;
using Npgsql;

namespace NpgsqlTesting.Trie;

public class NodeQueries(string connectionString)
{
    public async Task<NodeModel> GetAsync(int treeId, string path)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT id, full_path, is_terminal
                              FROM nodes
                              WHERE tree_id = @tree
                              AND full_path = @path
                              """;
        
        command.Parameters.AddWithValue("tree", treeId);
        command.Parameters.AddWithValue("path", path);

        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var id = reader.GetInt64("id");
            var fullPath = reader.GetString("full_path");
            var isTerminal = reader.GetBoolean("is_terminal");

            return new NodeModel(id, fullPath, isTerminal);
        }
        
        throw new NodeNotFoundError();
    }
}

public class NodeNotFoundError : Exception;