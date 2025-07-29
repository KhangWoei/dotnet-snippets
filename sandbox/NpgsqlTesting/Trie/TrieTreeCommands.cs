using Npgsql;

namespace NpgsqlTesting.Trie;

public class TrieTreeCommands(string connectionString)
{
    public async Task CreateAsync(CreateTreeRequest request)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO trees (name, base_url)
                              VALUES (@n, @u);
                              """;
        
        command.Parameters.AddWithValue("n", request.Name);
        command.Parameters.AddWithValue("u", request.BaseUrl);

        await command.ExecuteNonQueryAsync();
    }
}