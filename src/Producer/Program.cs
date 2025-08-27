using FinnhubClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.Services.UseClient(builder.Configuration);

using var host = builder.Build();
var client = host.Services.GetRequiredService<Client>();
var result = await client.GetQuoteAsync("MSFT", CancellationToken.None);

Console.WriteLine(result);
