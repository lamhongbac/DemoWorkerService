using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService.Jobs
{
    public interface IScopeJob
    {
        Task DoJob();
    }
}
