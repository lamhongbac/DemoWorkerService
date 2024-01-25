using DemoWorkerService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<MinuteTask>();

var host = builder.Build();
host.Run();
