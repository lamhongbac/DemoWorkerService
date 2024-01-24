using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService
{
    public class DemoWorker : BackgroundService
    {
        private static int SecondsUntilMidnight(IMyTask myTask)
        {
            if (myTask != null && myTask.RepeatedType== ERepeatedType.Minute) 
            {
                return (int)(DateTime.Today.AddMinutes(myTask.RepeatInterval) - DateTime.Now).TotalSeconds;
            }
            return 0;
        }

        private readonly ILogger<DemoWorker> _logger;
        MyTaskManager myTaskManager = new MyTaskManager();
        public DemoWorker(ILogger<DemoWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IMyTask task =myTaskManager.MyTasks[0];
            if (task != null)
            {
                var countdown = SecondsUntilMidnight(task);

                while (!stoppingToken.IsCancellationRequested)
                {
                    if (countdown-- <= 0)
                    {
                        try
                        {
                            await OnTimerFiredAsync(stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            // TODO: log exception
                        }
                        finally
                        {
                            countdown = SecondsUntilMidnight(task);
                        }
                    }
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
        private async Task OnTimerFiredAsync(CancellationToken stoppingToken)
        {
            // do your work here
            Debug.WriteLine("Simulating heavy I/O bound work");
            await Task.Delay(2000, stoppingToken);
        }
    }
}
