using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService
{
    /// <summary>
    /// 
    /// </summary>
    public class Worker : BackgroundService
    {
        static int count = 0;
        static bool firstRun=true;
        private readonly ILogger<Worker> _logger;
        static DateTime runAt=DateTime.Now.AddSeconds(30);
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                firstRun = (count==0);
                var countdown = SecondsUntilFireTime(runAt);
                //string message = $"Lan: {count} countdown: {countdown} ";
                //Debug.WriteLine(message);
                if (countdown-- <= 0)
                {
                    count++;
                    await RunTask(stoppingToken);


                    //lan chay ke tiep tang them xxx ke tu lan chay truoc do

                    runAt= runAt.AddMinutes(5);
                    
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
        }

        /// <summary>
        /// thoi diem hien tai - TG can chay
        /// </summary>
        /// <returns></returns>
        private static int SecondsUntilFireTime(DateTime runAt)
        {
            int totalSecondFromFireTime = (int)(runAt - DateTime.Now).TotalSeconds;
            return totalSecondFromFireTime;
        }
    }
}
