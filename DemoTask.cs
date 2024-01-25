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
        static int runtime=0;
        static bool firstRun = true;

        //thoi diem bat dau chay theo cau hinh
        static  DateTime startAt;

        //thoi diem thuc chay job 
        static DateTime runAt;
        /// <summary>
        /// 1/lan dau tien chay qua day
        /// se cho ra thoi gian can thiet de bat dau chay
        /// thi du : TG chay trong cau hinh la 11:30
        /// TG chay qua la 11:25=> 5*60 phut nua se chay
        /// 
        /// 2/Lan thu 2 se dc call sau khi lan 1
        /// thi du : TG chay trong lan 1 la 11:30 va interval la 5p=> TG can chay cho lan tiep=> 11:35
        /// Neu so sanh thoi gian hien tai voi 11:35 => so giay x*60 tiep theo
        /// 
        /// </summary>
        /// <param name="myTask"></param>
        /// <returns></returns>
        private static int SecondsUntilFireTime(IMyTask myTask)
        {
            if (myTask != null && myTask.RepeatedType== ERepeatedType.Minute) 
            {
                //dk1
                
                if (DateTime.Now.Date>=myTask.BeginDate && DateTime.Now.Date <=myTask.EndDate)
                {
                    runtime++;
                    int totalSecondFromFireTime = 0;
                    if (firstRun)
                    {
                        //thoi diem bat dau chay so voi vau hinh
                        //ex: Cau hinh la chau task vao 9g35 hang ngay
                        
                        DateTime today = DateTime.Today;

                        DateTime startAt = DateTime.Today.AddTicks(myTask.StartAt.Ticks);
                         totalSecondFromFireTime = (int)(startAt - DateTime.Now).TotalSeconds;
                        
                        
                    }
                    else
                    {
                         totalSecondFromFireTime = (int)(runAt - DateTime.Now).TotalSeconds;
                    }
                    
                   
                    //return (int)(DateTime.Today.AddMinutes(myTask.RepeatInterval) - DateTime.Now).TotalSeconds;
                    //Debug.WriteLine($"lan chay thu: {runtime}, vao luc: {runAt}");
                    return totalSecondFromFireTime;

                }
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
                var countdown = SecondsUntilFireTime(task);

                while (!stoppingToken.IsCancellationRequested)
                {
                    if (countdown-- <= 0)
                    {
                        try
                        {
                            if (firstRun)
                            {
                                runAt = DateTime.Today.AddTicks(task.StartAt.Ticks);
                                string mess = $"lan dau chay: {runAt}";

                                //Console.WriteLine(mess);
                                _logger.LogDebug(mess);
                            }
                            else
                            {
                                DateTime secondRunAt = runAt.AddMinutes(task.RepeatInterval);
                                string mess = $"lan 2nd chay: {secondRunAt}";
                                //Console.WriteLine(mess);
                                _logger.LogDebug(mess);

                                runAt = secondRunAt;
                            }
                            await OnTimerFiredAsync(stoppingToken, task);
                            firstRun = false;
                        }
                        catch (Exception ex)
                        {
                            string mess =ex.Message;
                            // TODO: log exception
                            _logger.LogDebug(mess);
                        }
                        finally
                        {
                            countdown = SecondsUntilFireTime(task);
                        }
                    }
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }

        private async Task OnTimerFiredAsync(CancellationToken stoppingToken, IMyTask task)
        {
            // do your work here
            //Debug.WriteLine("Simulating heavy I/O bound work");
            //await Task.Delay(2000, stoppingToken);
            IJob job= task.ToDoJob;
            await    job.DoJob();
        }
    }
}
