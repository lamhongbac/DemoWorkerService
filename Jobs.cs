using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService
{
    public class JobA : IJob
    {
        public JobA()
        {
        }

        public async Task DoJob() { }
    }
    public class JobB : IJob
    {
        public JobB()
        {
        }

        public async Task DoJob() { }
    }
    public class JobC : IJob
    {
        public JobC()
        {
        }

        public async Task DoJob() { }
    }
}
