using Backynet.Core;
using WorkerService1;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddBackynetServer(s =>
{
    s.UsePostgreSql(y =>
    {
        y.ConnectionString = "";
    });
});

var host = builder.Build();
host.Run();

var backynetClient = host.Services.GetRequiredService<IBackynetClient>();
await backynetClient.EnqueueAsync(() => Console.WriteLine("hello world"));