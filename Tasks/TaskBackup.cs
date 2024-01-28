using DemoWorkerService.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService.Tasks
{
    /// <summary>
    /// Mo phong task backup
    /// 
    /// </summary>
    public class TaskBackup : BackgroundService
    {
        bool taskAvailable = false;
        static int count = 0;
        static bool firstRun = true;
        private readonly ILogger<TaskBackup> _logger;
        static DateTime nextRun;
        static DateTime startAt;
        IConfiguration _configuration;
       
        static TaskConfiguration task;
        ERepeatedType repeatedType;
        BackUpData _todoJob;
        public TaskBackup(ILogger<TaskBackup> logger, IConfiguration configuration, BackUpData job)
        {
            _todoJob = job;
            _logger = logger;
            _configuration = configuration;
            var taskSection = configuration.GetSection("ScheduleTasks");
            //
            List<TaskConfiguration> configurations = taskSection.Get<List<TaskConfiguration>>();
            //
            task = configurations.FirstOrDefault(x => x.ToDoJob == job.JobID);

            taskAvailable = task != null && task.IsStart;

            if (taskAvailable)
            {
                bool ok = Enum.TryParse(task.RepeatedType, out repeatedType);

                //from config: gia su la 15h:00
                TimeSpan timeSpan = task.GetStartAt();


                DateTime configStartAt = DateTime.Today.AddTicks(timeSpan.Ticks);
                //hardcode startAt
                configStartAt = DateTime.Now.AddSeconds(-10);

                if (configStartAt < DateTime.Now)
                {
                    startAt = GetNextStart(configStartAt);
                }
                else
                {
                    startAt = configStartAt;
                }

               


                // sau 1 p chay 1 lan

                string init_message = $"Constructor... configured start at {configStartAt}, actual startAt: {startAt}  repeatedType: {repeatedType},interval: {task.RepeatInterval}";
                _logger.LogInformation(init_message);
            }
            else
            {
                string init_message = $"Task is not available";
                _logger.LogError(init_message);
            }
        }
        /// <summary>
        /// 
        /// Gia dinh la ngay bat dau va ket thuc thoa man dieu kien
        /// BeginData-EndDate
        /// Cac buoc se nhu sau
        /// 
        /// b1 xac dinh thoi diem chay lan 1/A
        /// b2 xac dinh countdown(b1)
        /// b3 doi den khi count down <0
        /// b4 chay lan thu 1
        /// b5 xac dinh thoi diem chay lan 2/1
        /// b6 xac dinh countdown(b6)
        /// b7 quay lai b3
        /// b8 chay lan 2/2
        /// b9 quay lai b6
        /// b10 chay lan 2/3
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!taskAvailable)
            {
                return;
            }
            //thoi diem chay trng cau hinh truoc thoi diem service start=> move thoi diem chay qua ngay Hom sau
            // neu thoi diem chay tron CH xay ra sau TDiem hien tai
            if (firstRun)
                nextRun = startAt;
            //0:
            string runAtmessage = $"Start ExecuteAsync; count=:{count} ,runAt at: {nextRun}";
            _logger.LogInformation(runAtmessage);

            // gia su kQ la 4g...
            while (!stoppingToken.IsCancellationRequested)
            {

                //count down :Xac dinh TG can phai chờ theo cấu hình ex:hien dang la 3g, KQ tra ra 10, nghia la dang o thoi diem 2g59p50s no se countdown xuong 10 second de bat dau chay
                //count down se giam xuong moi lan chay  Datetime.Now tang len, trong khi RunAt thi co dinh

                var countdown = SecondsUntilFireTime(nextRun);
                //string countdown_message = $"Calcualte firetime Lan: {count}  end  at {DateTime.Now}: {countdown}";
                //_logger.LogInformation(countdown_message);

                if (countdown-- <= 0)
                {
                    await RunTask(stoppingToken);
                    nextRun = GetNextStart(startAt);
                }
            }
        }

        /// <summary>
        /// neu la daily task, bi tre thi chuyen qua x ngay hom sau
        /// neu la hourly task bi tre thi chuyen qua x gio sau
        /// neu la minute ...
        /// </summary>
        /// <returns></returns>
        private DateTime GetNextStart(DateTime prevTime)
        {
            DateTime runAt = DateTime.MinValue;
            switch (repeatedType)
            {
                case ERepeatedType.Minute:
                    runAt = prevTime.AddMinutes(task.RepeatInterval);
                    break;
                case ERepeatedType.Hourly:
                    runAt = prevTime.AddHours(task.RepeatInterval);
                    break;
                case ERepeatedType.Daily:
                default:
                    runAt = prevTime.AddDays(task.RepeatInterval);
                    break;
            }
            return runAt;
        }

        /// <summary>
        /// mo phong chay task
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task RunTask(CancellationToken stoppingToken)
        {
            count++;
            string begmessage; string endmessage;

            if (count == 1)
            {
                begmessage = $"RunTask Lan  {count} begin  at {DateTime.Now}";
            }
            else
            {
                begmessage = $"RunTask Lan 2/ {count - 1} begin  at {DateTime.Now}";
            }

            _logger.LogInformation(begmessage);

            //await Task.Delay(1000, stoppingToken);
            await _todoJob.DoJob();
            if (count == 1)
            {
                endmessage = $"End runTask Lan: {count},    at {DateTime.Now}";
            }
            else
            {
                endmessage = $"End runTask Lan: 2/{count - 1},    at {DateTime.Now}";
            }

            firstRun = false;
            _logger.LogInformation(endmessage);
            //xac dinh thoi diem chay 1 lan sau khi task run

        }

        /// <summary>
        /// tinh tu thoi diem  chay ung dung lan dau tien, hoac thoi diem task hoan tat sau do
        /// Nguyen ly la :
        /// moi 1 khi muon biet khi nao task chay thi phai tinh ra dc tai [thoi diem can chay] so sanh voi [thoi diem hien tai]
        /// 
        /// </summary>
        /// <returns></returns>
        private static int SecondsUntilFireTime(DateTime runAt)
        {
            int totalSecondFromFireTime = (int)(runAt - DateTime.Now).TotalSeconds;

            return totalSecondFromFireTime;
        }
    }
}
