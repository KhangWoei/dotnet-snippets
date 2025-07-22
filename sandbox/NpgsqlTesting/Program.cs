using System.Data;
using Npgsql;

var connectionString = "Host=localhost,5432;Username=postgres;Password=Password@1;";

await using var connection = new NpgsqlConnection(connectionString);
await connection.OpenAsync();

var command = connection.CreateCommand();
command.CommandText = """
                      INSERT INTO trees (name, base_url) VALUES (@n, @u);
                      """;
command.Parameters.AddWithValue("n", "test3");
command.Parameters.AddWithValue("u", "test");
                      
await command.ExecuteNonQueryAsync();

command.CommandText = """
                      SELECT * FROM trees;
                      """;

await using var reader = await command.ExecuteReaderAsync();

while (await reader.ReadAsync())
{
    var name = reader.GetString("name");
    var baseUrl = reader.GetString("base_url");

    Console.WriteLine($"{name}, {baseUrl}");
}
