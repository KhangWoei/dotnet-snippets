using Npgsql;

namespace TrieData;

internal sealed class TreesWriteRepository(NpgsqlDataSource dataSource)
{
    public async Task<TrieModel> CreateAsync(string name, string baseUrl, CancellationToken cancellationToken = default)
    {
        await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        
        var command = connection.CreateCommand();
        command.CommandText = """
                              INSERT INTO trees (name, base_url)
                              VALUES (@n, @u)
                              RETURNING id;
                              """;
        
        command.Parameters.AddWithValue("n", name);
        command.Parameters.AddWithValue("u", baseUrl);

        // The query should never return null, if it did we should expect a database exception
        var result = (int)(await command.ExecuteScalarAsync(cancellationToken))!;
        return new TrieModel(result, name, baseUrl);
    }
}