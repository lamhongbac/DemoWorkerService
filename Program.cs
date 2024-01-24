using DemoWorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<DemoWorker>();

var host = builder.Build();
host.Run();
