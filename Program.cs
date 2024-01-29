using DemoWorkerService;
using DemoWorkerService.Jobs;
using DemoWorkerService.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

var logger = LogManager.GetCurrentClassLogger();

//var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var builder = Host.CreateApplicationBuilder(args);
    //builder.Services.Configure((hostContext, logging) =>
    //{
    //    // ensure just use NLog
    //    logging.Services.Clear();
    //    logging.SetMinimumLevel(LogLevel.Trace);

    //    //logging.AddNLog(hostContext.Configuration);
    //    logging.AddNLog(hostContext.Configuration.GetSection("NLog"));
    //});



    builder.Logging.ClearProviders();

    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
    builder.Logging.AddEventSourceLogger();
    builder.Logging.AddNLog();
    
    //builder.Services.AddScoped<IScopeJob,ScopeJob>();


   // builder.Services.AddSingleton<CheckNewMember>();
    
    builder.Services.AddSingleton<BackUpData>();
    builder.Services.AddHostedService<TimerScheduler>();

    //builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
    //builder.Services.AddHostedService<BackGroundWithScope>();
    //builder.Services.AddHostedService<TaskBackup>();

    var host = builder.Build();
    host.Run();

}
catch(Exception ex)
{
    logger.Error(ex.Message);
}
finally
{
    LogManager.Shutdown();
}