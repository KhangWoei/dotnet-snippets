using System.Data;
using System.Diagnostics.CodeAnalysis;
using Npgsql;

namespace TrieData.Nodes;

internal sealed class NodesReadRepository(NpgsqlDataSource dataSource)
{
    
    public async Task<NodeModel?> GetAsync(int treeId, string path)
    {
        await using var command = dataSource.CreateCommand();
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
            var parentId = reader.GetInt64("parent_id");
            var fullPath = reader.GetString("full_path");
            var isTerminal = reader.GetBoolean("is_terminal");

            return new NodeModel(id, parentId, fullPath, isTerminal);
        }

        return null;
    }

    public bool TryGet(int treeId, string path, [MaybeNullWhen(false)] out NodeModel? result)
    {
        using var command = dataSource.CreateCommand();
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
            var parentId = reader.GetInt64("parent_id");
            var fullPath = reader.GetString("full_path");
            var isTerminal = reader.GetBoolean("is_terminal");

            result = new NodeModel(id, parentId, fullPath, isTerminal);
        }
        else
        {
            result = null;
        }

        return result is not null;
    }
}