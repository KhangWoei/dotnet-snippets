using System.Data;
using Npgsql;

namespace NpgsqlTesting.Trie;

public class TrieTreeCommands(string connectionString)
{
    public async Task Create(TrieTreeModel tree)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO trees (name, base_url)
                              VALUES (@n, @u);
                              """;
        
        command.Parameters.AddWithValue("n", tree.Name);
        command.Parameters.AddWithValue("u", tree.BaseUrl);

        await command.ExecuteNonQueryAsync();
    }
}