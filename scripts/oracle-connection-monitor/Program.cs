using System.Text;
using Oracle.ManagedDataAccess.Client;

var cancellationTokenSource = new CancellationTokenSource();
var connectionString = Console.ReadLine();
var builder = new OracleConnectionStringBuilder(connectionString)
{
    Pooling = false
};

using var connection = new OracleConnection(builder.ToString());

using var command = connection.CreateCommand();
command.CommandText = """
                      SELECT
                          s.username,
                          s.program,
                          s.module,
                          s.status,
                          s.type,
                          TO_CHAR(s.logon_time, 'YYYY-MM-DD HH24:MI:SS') AS logon_datetime
                      FROM
                          v$session s
                          JOIN v$process p ON s.paddr = p.addr
                      WHERE
                          s.type = 'USER'
                          AND s.sid <> sys_context('USERENV', 'SID')
                      ORDER BY
                          s.logon_time DESC
                      """;

connection.Open();

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cancellationTokenSource.Cancel();
};

while (!cancellationTokenSource.IsCancellationRequested)
{
    using var reader = command.ExecuteReader();
    var stringBuilder = new StringBuilder();
    while (reader.Read())
    {
        for (var i = 0; i < reader.FieldCount; i++)
        {
            stringBuilder.Append(reader.GetValue(i));
            stringBuilder.Append(" | ");
        }

        stringBuilder.AppendLine();
    }

    stringBuilder.Append(new string('-', 50));

    Console.WriteLine(stringBuilder.ToString());
    Thread.Sleep(TimeSpan.FromSeconds(5));
}

connection.Close();
