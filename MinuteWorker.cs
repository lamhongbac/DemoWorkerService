using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DemoWorkerService
{
    /// <summary>
    /// Gia su cu 30 phut kiem tra data sale 1 lan
    /// </summary>
    public class MinuteTask : BackgroundService
    {
        static int count = 0;
        static bool firstRun=true;
        private readonly ILogger<MinuteTask> _logger;
        static DateTime runAt;
        static DateTime startAt;
        IConfiguration _configuration;
        int interval = 0;
        public MinuteTask(ILogger<MinuteTask> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration= configuration;
            var taskSection=configuration.GetSection("ScheduleTasks");

            List<MyTask> tasks = taskSection.Get<List<MyTask>>();

            MyTask task = tasks[0];

            //from config: gia su la 15h:00
            TimeSpan timeSpan = task.GetStartAt();

            startAt = DateTime.Today.AddTicks(timeSpan.Ticks);

            // sau 1 p chay 1 lan
            interval = task.RepeatInterval; 
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            firstRun = (count == 0);
            if (firstRun)
            {
                //thoi diem tuong lai
                if (startAt > DateTime.Now)
                {
                    runAt = startAt;
                }
                else
                {
                    runAt = startAt.AddDays(1);
                }
            }
            // gia su kQ la 4g...
            while (!stoppingToken.IsCancellationRequested)
            {
               
                
                
                
                
                //count down se giam xuong vi Datetime now tang len, run at thi co dinh

                var countdown = SecondsUntilFireTime(runAt);
                //string message = $"Lan: {count} countdown: {countdown} ";
                //Debug.WriteLine(message);
                if (countdown-- <= 0)
                {
                    count++;
                    await RunTask(stoppingToken);

                    firstRun=false;
                    //lan chay ke tiep tang them xxx ke tu lan chay truoc do
                    if (!firstRun)
                    {
                        runAt = runAt.AddMinutes(interval);
                    }


                }
            }
        }
        /// <summary>
        /// mo phong chay task
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        private async Task RunTask(CancellationToken stoppingToken)
        {
            string message = $"Lan: {count} begin  at {DateTime.Now}";
            Console.WriteLine(message);
            await Task.Delay(1000, stoppingToken);
             message = $"Lan: {count}  end  at {DateTime.Now}";
            Console.WriteLine(message);
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
