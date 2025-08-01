using Npgsql;

namespace NpgsqlTesting.Trie;

public class TrieTreeCommands(string connectionString)
{
    public async Task<TrieTreeModel?> CreateAsync(CreateTreeRequest request, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO trees (name, base_url)
                              VALUES (@n, @u)
                              RETURNING id;
                              """;
        
        command.Parameters.AddWithValue("n", request.Name);
        command.Parameters.AddWithValue("u", request.BaseUrl);

        var result = (int?) await command.ExecuteScalarAsync(cancellationToken);

        return result is null ? null : new TrieTreeModel((int)result, request.Name, request.BaseUrl);
    }
}