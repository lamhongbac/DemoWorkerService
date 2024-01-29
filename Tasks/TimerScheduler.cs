using DemoWorkerService.Jobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService.Tasks
{
    /// <summary>
    /// b1 : xac dinh tg fired
    /// b2: start vao thoi diem fired
    /// b3 : add periodic
    /// b4 : start
    /// Gia su cu 30 phut kiem tra data sale 1 lan
    /// </summary>
    public class TimerScheduler : BackgroundService
    {
        ERepeatedType repeatedType;
        TimeSpan startTimeConfig; //hh:mm:ss thoi diem bat dau chay task

        int interval;//period or interval
        DateTime startAt;// thoi diem chay

        static bool isFirstRun = true;
        private readonly ILogger<TimerScheduler> _logger;
        
        TaskConfiguration taskConfig;
        IJob _todoJob;
        private Task? _timertask;
        private readonly PeriodicTimer _timer;
        public TimerScheduler(ILogger<TimerScheduler> logger, BackUpData job,
            IConfiguration configuration)
        {
            _todoJob= job;
            _logger = logger;
            //
           
            var taskSection = configuration.GetSection("ScheduleTasks");
            List<TaskConfiguration> configurations = taskSection.Get<List<TaskConfiguration>>();
            taskConfig = configurations.FirstOrDefault(x => x.ToDoJob == _todoJob.JobID);
            ERepeatedType repeatedType;
            startTimeConfig = taskConfig.GetStartAt();
          int  interval = taskConfig.RepeatInterval;

            DateTime configStartAt = DateTime.Today.AddTicks(startTimeConfig.Ticks);
            string begmessage = $"Configuration StartAt  at {configStartAt}";
            _logger.LogInformation(begmessage);

            if (configStartAt < DateTime.Now)
            {
                startAt = GetStartDateTime(configStartAt);
            }
            else
            {
                startAt = configStartAt;
            }
             begmessage = $"processing calculate actual start at {startAt}";
            _logger.LogInformation(begmessage);

            bool ok = Enum.TryParse(taskConfig.RepeatedType, out repeatedType);
            
            TimeSpan period = GetPediod();
            begmessage = $"Interval period in minutes {period.TotalMinutes}";
            _logger.LogInformation(begmessage);

            _timer = new PeriodicTimer(period);
        }

        private TimeSpan GetPediod()
        {
            TimeSpan ts; 
            switch (repeatedType)
            {
                case ERepeatedType.Minute:

                    ts= TimeSpan.FromMinutes(taskConfig.RepeatInterval);
                    break;
                case ERepeatedType.Hourly:
                    ts = TimeSpan.FromHours(taskConfig.RepeatInterval);
                    break;
                case ERepeatedType.Daily:
                default:
                    ts = TimeSpan.FromDays(taskConfig.RepeatInterval);
                    break;
            }
            return ts;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
          string  begmessage = $"ExecuteAsync  at {DateTime.Now}";
            _logger.LogInformation(begmessage);
            while (!stoppingToken.IsCancellationRequested)
            {
                //first run chay theo config time
                if (isFirstRun)
                {                  
                    var countdown = SecondsUntilFireTime(startAt);
                    //string countdown_message = $"Calcualte firetime Lan: {count}  end  at {DateTime.Now}: {countdown}";
                    //_logger.LogInformation(countdown_message);

                    if (countdown-- <= 0)
                    {
                        await RunTask();
                    }                    
                }
                else
                {
                    //second run chay theo next tick (period) tinh tu thoi diem first run
                    //
                    while (await _timer.WaitForNextTickAsync(stoppingToken))
                    {
                        await RunTask();
                    }
                }
                
               
            }
        }
        /// <summary>
        /// mo phong chay task
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task RunTask()
        {
            string begmessage;

                if (isFirstRun)
                {
                    begmessage = $"RunTask Lan  dau begin  at {DateTime.Now}";
                }
                else
                {
                    begmessage = $"RunTask Lan sau  begin  at {DateTime.Now}";
                }

                _logger.LogInformation(begmessage);

                //await Task.Delay(1000, stoppingToken);
                await _todoJob.DoJob();
                
                if (isFirstRun)
                    isFirstRun = false;

               
            
        }
        /// <summary>
        /// viet lai cai nay vi add lan dau co the van nho hon DateTime Now
        /// can so sanh RunAt voi DateTime.Now
        /// </summary>
        /// <param name="configStartDateTime"></param>
        /// <returns></returns>
        private DateTime GetStartDateTime(DateTime configStartDateTime)
        {
            DateTime runAt = configStartDateTime;

            while (runAt<DateTime.Now)
            {
                switch (repeatedType)
                {
                    case ERepeatedType.Minute:
                        runAt = configStartDateTime.AddMinutes(taskConfig.RepeatInterval);
                        break;
                    case ERepeatedType.Hourly:
                        runAt = configStartDateTime.AddHours(taskConfig.RepeatInterval);
                        break;
                    case ERepeatedType.Daily:
                    default:
                        runAt = configStartDateTime.AddDays(taskConfig.RepeatInterval);
                        break;
                }
            }
            return runAt;
        }


        private static int SecondsUntilFireTime(DateTime runAt)
        {
            int totalSecondFromFireTime = (int)(runAt - DateTime.Now).TotalSeconds;

            return totalSecondFromFireTime;
        }
    }
}
