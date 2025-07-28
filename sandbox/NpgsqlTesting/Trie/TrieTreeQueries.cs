using System.Data;
using Npgsql;

namespace NpgsqlTesting.Trie;

public class TrieTreeQueries
{
    private const string _connectionString = "Host=localhost,5432;Username=postgres;Password=Password@1;";

    public async  Task<TrieTreeModel> Get(string treeName)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
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