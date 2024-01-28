using DemoWorkerService.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService.Tasks
{
    public class BackGroundWithScope : BackgroundService
    {
        ILogger<BackGroundWithScope> logger;
        IServiceProvider serviceProvider;
        public BackGroundWithScope(IServiceProvider serviceProvider,
            ILogger<BackGroundWithScope> logger  )
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    IScopeJob scopejob = scope.ServiceProvider.GetRequiredService<IScopeJob>();
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                    await scopejob.DoJob();
                }
            }
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("StartAsync at {0}", DateTime.Now);
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("StopAsync at {0}", DateTime.Now);
            return base.StopAsync(cancellationToken);
        }
    }
}
