using System.Data;
using System.Diagnostics.CodeAnalysis;
using Npgsql;

namespace TrieData.Tries;

internal sealed class TriesReadRepository(NpgsqlDataSource dataSource) : ITriesReadRepository
{
    
    public async Task<ITrieModel?> GetAsync(string treeName)
    {
        await using var command = dataSource.CreateCommand();
        command.CommandText = """
                              SELECT id, name, base_url
                              FROM trees
                              WHERE name = @n
                              """;
        
        command.Parameters.AddWithValue("n", treeName);
        
        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var id = reader.GetInt32("id");
            var name = reader.GetString("name");
            var baseUrl = reader.GetString("base_url");

            return new TrieModel(id, name);
        }

        return null;
    }

    public bool TryGet(string treeName, [MaybeNullWhen(false)] out ITrieModel tree)
    {
        using var command = dataSource.CreateCommand();
        command.CommandText = """
                              SELECT id, name, base_url
                              FROM trees
                              WHERE name = @n
                              """;
        
        command.Parameters.AddWithValue("n", treeName);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            var id = reader.GetInt32("id");
            var name = reader.GetString("name");
            var baseUrl = reader.GetString("base_url");

            tree = new TrieModel(id, name);
        }
        else
        {
            tree = null;
        }

        return tree is not null;
    }
}

public interface ITriesReadRepository
{
    Task<ITrieModel?> GetAsync(string triesTreeName);

    bool TryGet(string triesTreeName, [MaybeNullWhen(false)]out ITrieModel tree);
}