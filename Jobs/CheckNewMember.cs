using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService.Jobs
{
    public class CheckNewMember : IJob
    {
        public string JobID { get; set; }
        private static int count = 0;
        public string Title { get ; set ; }
        ILogger<CheckNewMember> _logger;
        public CheckNewMember(ILogger<CheckNewMember> logger)
        {
            Title = nameof(CheckNewMember);
            JobID = Title;
            _logger = logger;
        }
        public async Task DoJob()
        {
            count++;
            string beginTask = $"Begin job at: {DateTime.Now}-count: {count}";
            
            string task1 = $"Scan for new members";
            string task2 = "Create promtion transaction for each mem";
            string task3 = "Send notification promtion to each mem";
            _logger.LogInformation(beginTask);
            _logger.LogInformation(task1);
            _logger.LogInformation(task2);
            _logger.LogInformation(task3);

            //task keo dai 2 second
            await Task.Delay(2000);
            string endTask = $"End job at: {DateTime.Now}-count: {count}";
            _logger.LogInformation(endTask);
        }
    }
}
