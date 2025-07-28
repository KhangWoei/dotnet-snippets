using System.Data;
using Npgsql;

namespace NpgsqlTesting.Trie;

public class TrieTreeQueries(string connectionString)
{
    public async Task<TrieTreeModel> GetAsync(string treeName)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT name, base_url
                              FROM trees
                              WHERE name = @n
                              """;
        
        command.Parameters.AddWithValue("n", treeName);
        
        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var name = reader.GetString("name");
            var baseUrl = reader.GetString("base_url");

            return new TrieTreeModel(name, baseUrl);
        }

        throw new TreeNotFoundError();
    }
}

public class TreeNotFoundError : Exception;