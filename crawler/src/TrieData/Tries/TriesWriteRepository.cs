using Npgsql;

namespace TrieData.Tries;

internal sealed class TriesWriteRepository(NpgsqlDataSource dataSource) : ITriesWriteRepository
{
    public async Task<ITrieModel> CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        await using var command = dataSource.CreateCommand();
        command.CommandText = """
                              INSERT INTO trees (name)
                              VALUES (@n)
                              RETURNING id;
                              """;
        
        command.Parameters.AddWithValue("n", name);

        // The query should never return null, if it did we should expect a database exception
        var result = (int)(await command.ExecuteScalarAsync(cancellationToken))!;
        return new TrieModel(result, name);
    }
}

public interface ITriesWriteRepository
{
    Task<ITrieModel> CreateAsync(string name, CancellationToken cancellationToken);
}