using System.CommandLine;
using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;

namespace AzureWithSqlClient;

public static class Program
{
    public static int Main(string[] args)
    {
        var connectionStringOption = new Option<string>("--connection-string")
        {
            Description = "Connection string to a database",
            Required = true,
        };

        var root = new RootCommand("Token authentication");
        root.Options.Add(connectionStringOption);

        var result = root.Parse(args);
        var connectionString = result.GetValue(connectionStringOption);
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        var connection = new SqlConnection(connectionStringBuilder.ConnectionString);

        if (connectionStringBuilder.Authentication == SqlAuthenticationMethod.ActiveDirectoryManagedIdentity)
        {
            Console.WriteLine("This will fail if the application is not being executed on an Azure resource with a configured Manage Identity");
        }

        // If authentication and password is empty, try getting a token?
        if (string.IsNullOrEmpty(connectionStringBuilder.Password) && connectionStringBuilder.Authentication == SqlAuthenticationMethod.NotSpecified)
        {
            var azureCredential = new DefaultAzureCredential();
            var tokenRequestContext = new TokenRequestContext(["https://database.windows.net"]);
            var token = azureCredential.GetToken(tokenRequestContext);
            connection.AccessToken = token.Token;
        }
        
        connection.Open();
        
        var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT TOP 1 * FROM [Bar]
                              """;

        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine(reader.GetString(1));
        }

        return 0;
    }
}
