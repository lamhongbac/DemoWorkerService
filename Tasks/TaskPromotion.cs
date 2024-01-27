using DemoWorkerService.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService.Tasks
{
    public class TaskPromotion : BaseTask
    {
        //(ILogger<TaskCheckMember> logger, IConfiguration configuration, CheckNewMember job)
        public TaskPromotion(ILogger<TaskPromotion> logger, 
            IConfiguration configuration, IJob job):base(logger, configuration, job) 
        {
            
        }
        BaseTask baseTask;
       
    }
}
