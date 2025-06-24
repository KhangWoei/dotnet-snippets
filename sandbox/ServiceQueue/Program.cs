using ServiceQueue;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<Controller>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IServiceQueue>(_ => new ServiceQueue.ServiceQueue(100));

var host = builder.Build();

var controller = host.Services.GetRequiredService<Controller>();
controller.Start();

host.Run();
