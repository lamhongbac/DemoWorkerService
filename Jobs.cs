﻿using System;
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
        string JobID { get; set; }
        Task DoJob();
    }
    public class JobA : IJob
    {
        private static int time = 0;
        public string Title { get; set; }
        public string JobID { get; set; }
        public JobA()
        {
            Title = "Job A";
            JobID = Title;
        }
       
        public  async Task DoJob()
        {
            try
            {
                time++;
                var _dateTime = DateTime.Now;
                string message = $"{Title} - lan  {time}; run  at: {_dateTime} ";
                Console.WriteLine(message);

                
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    public class JobB : IJob
    {
        public string JobID { get; set; }
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
            //Console.WriteLine(message);
            Debug.WriteLine(message);
            //_logger.LogDebug(message);
        }

    }
   
}
