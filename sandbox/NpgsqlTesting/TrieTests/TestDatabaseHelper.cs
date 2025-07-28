using Npgsql;

namespace NpgsqlTesting.TrieTests;

public class TestDatabaseHelper(string connectionString, string databaseName = "") 
{
    private readonly string _databaseName = string.IsNullOrEmpty(databaseName) ? $"test_{Guid.NewGuid()}" : databaseName;
    
    public async Task<string> CreateDatabaseAsync()
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        var command = connection.CreateCommand();
        command.CommandText = $"""
                              CREATE DATABASE "{_databaseName}"
                              """;

        await command.ExecuteNonQueryAsync();
        
        var databaseConnection = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Database = _databaseName
        }.ToString();
        
        await RunMigrations(databaseConnection);

        return databaseConnection;
    }

    private async Task RunMigrations(string databaseConnection)
    {
        var assembly = typeof(TestDatabaseHelper).Assembly;
        var resourceNames = assembly.GetManifestResourceNames();
        var migrations = resourceNames.Where(n => n.EndsWith(".sql")).Order();

        await using var connection = new NpgsqlConnection(databaseConnection);
        await connection.OpenAsync();
        
        foreach (var migration in migrations)
        {
            await using var stream = assembly.GetManifestResourceStream(migration);
            using var reader = new StreamReader(stream!);

            var sql = await reader.ReadToEndAsync();
            var command = connection.CreateCommand();

            command.CommandText = sql;
            await command.ExecuteNonQueryAsync();
        }
    }

    public async ValueTask Cleanup()
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        var command = connection.CreateCommand();

        command.CommandText = $"""
                               SELECT pg_terminate_backend(pid)
                               FROM pg_stat_activity
                               WHERE datname = '{_databaseName}' AND pid <> pg_backend_pid()
                               """;
        await command.ExecuteNonQueryAsync();

        command.CommandText = $"""
                               DROP DATABASE IF EXISTS "{_databaseName}";
                               """;
        await command.ExecuteNonQueryAsync();
    }
}