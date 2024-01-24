using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService
{
    public interface IJob
    {
        string Title { get; set; }
        Task DoJob();
    }
    public class JobA : IJob
    {
        private static int time = 0;
        public string Title { get; set; }
        public JobA()
        {
            Title = "Job A";
            
        }
       
        public  async Task DoJob()
        {
            time++;
            string message = string.Format("Job {0} run  {1} time, At{2}",Title, time,DateTime.Now.TimeOfDay);
            Console.WriteLine(message);
           // Debug.WriteLine(message);
            //_logger.LogDebug(message);
        }
    }
    public class JobB : IJob
    {
        private static int time = 0;
        public string Title { get; set; }
        //private readonly ILogger<JobB> _logger;
        public JobB() 
        {
            Title = "Job B";
        }
        public async Task DoJob()
        {
            time++;
            string message = string.Format("Job {0} run  {1} time, At{2}", Title, time, DateTime.Now.TimeOfDay);
            Console.WriteLine(message);
            Debug.WriteLine(message);
            //_logger.LogDebug(message);
        }

    }
   
}
