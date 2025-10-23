using System.Data;
using System.Diagnostics.CodeAnalysis;
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

    public bool TryGet(int treeId, string path, [MaybeNullWhen(false)] out NodeModel? result)
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT id, full_path, is_terminal
                              FROM nodes
                              WHERE tree_id = @tree
                              AND full_path = @path
                              """;
        
        command.Parameters.AddWithValue("tree", treeId);
        command.Parameters.AddWithValue("path", path);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            var id = reader.GetInt64("id");
            var fullPath = reader.GetString("full_path");
            var isTerminal = reader.GetBoolean("is_terminal");

            result = new NodeModel(id, fullPath, isTerminal);
        }
        else
        {
            result = null;
        }

        return result is not null;
    }
}

public class NodeNotFoundError : Exception;