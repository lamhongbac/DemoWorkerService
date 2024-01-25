using DemoWorkerService;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args).ConfigureAppConfiguration();
builder.Logging.ClearProviders();

builder.
builder.Services.AddHostedService<MinuteTask>();

var host = builder.Build();
host.Run();
