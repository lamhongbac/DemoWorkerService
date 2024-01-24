using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService
{
    public class JobA : IJob
    {
        private static int time = 0;
        public string Title { get; set; }
        public JobA()
        {
            Title = "Job A";
        }

        public virtual async Task DoJob()
        {
            time++;
            string message = string.Format("Job {0} run  {1} time, At{2}",Title, time,DateTime.Now.TimeOfDay);
            Console.WriteLine(message);
        }
    }
    public class JobB : JobA
    {
        
        public JobB():base() 
        {
            Title = "Job B";
        }

        
    }
    public class JobC : JobA
    {
        private static int time = 0;
        public JobC()
        {
            Title = "Job C";
        }

      
    }
}
