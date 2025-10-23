# Npsql Library

## Connections and Data Sources
Starting with version 7.0 data sources should be the starting point of any database operation. A data source is represents the PostgreSQL database and can hand out connections to it. It deals with connection pooling, resource management, configuration, and performance.

Typically expected to build a single data source and reuse that instance throughout an application. Data sources are thread-safe.

``` C#
var connectionString = "Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase";
await using var dataSource = NpgsqlDataSource.Create(connectionString);
```

## Un-managed vs Managed connection lifecycle

### NpgsqlDataSource.OpenConnectionAsync()
- This returns a connection
- Allows more granular, explicit control over the connection lifecycle 
- Allows for multiple commands under one connection

### NpgsqlDataSource.CreateCommand()
- Borrows connection from the pool and returns when done
