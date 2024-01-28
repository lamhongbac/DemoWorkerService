using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService.Jobs
{
    public class ScopeJob : IScopeJob
    {
        ILogger<ScopeJob> logger;

        public ScopeJob(ILogger<ScopeJob> logger)
        {
            jobID = Guid.NewGuid().ToString(); ;
            this.logger = logger;

        }
        string jobID;
        public async Task DoJob()
        {
            logger.LogInformation("Job ID : {0} run at {1}", jobID,DateTime.Now);
            await Task.FromResult(Task.CompletedTask);
        }
    }
}
