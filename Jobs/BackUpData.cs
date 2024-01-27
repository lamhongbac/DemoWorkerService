using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService.Jobs
{
    public class BackUpData : IJob
    {
        public string JobID { get; set; }
        private static int count = 0;
        public string Title { get; set; }
        ILogger<BackUpData> _logger;
        public BackUpData(ILogger<BackUpData> logger)
        {
            Title = nameof(BackUpData);
            JobID = Title;
            _logger = logger;
        }

        public async Task DoJob()
        {
            count++;
            //string beginTask = $"Begin job at: {DateTime.Now}-count: {count}";

            //string task1 = $"Backup data 1";
            //string task2 = $"Backup data 2";
            //string task3 = $"Cleaning task";
            //_logger.LogInformation(beginTask);
            //_logger.LogInformation(task1);
            //_logger.LogInformation(task2);
            //_logger.LogInformation(task3);

            ////task keo dai 2 phut
            await Task.Delay(20000);
            string endTask = $"End job at: {DateTime.Now}-count: {count}";
            _logger.LogInformation(endTask);
        }
    }
}
