using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService
{
    public interface IMyTask
    {
        DateTime BeginDate { get; set; }
        DateTime EndDate { get; set; }
        ERepeatedType RepeatedType { get; set; }
        int RepeatInterval { get; set; }
        TimeSpan StartAt { get; set; } //new TimeSpan(h,m,s)
        IJob ToDoJob { get; set; }
    }
    public enum ERepeatedType
    {
        Minute, Hourly, Daily, Weekly, Monthly, Yearly
    }

    public class MyTask : IMyTask
    {
        public MyTask()
        {
            
        }
        public DateTime BeginDate { get; set; } //ngay bat dau
        public ERepeatedType RepeatedType { get; set; } //minutely, hourly, daily, weekly, monthly
        public int RepeatInterval { get; set; } //30p, 22g, 1day, 1week
        public TimeSpan StartAt { get; set; } //thoi diem chay task
        public DateTime EndDate { get; set; } //ngay ket thuc
        public IJob ToDoJob { get; set; }
    }
    public class MyTaskManager
    {
        public List<MyTask> MyTasks { get; set; }
        public MyTaskManager()
        {

            MyTasks = new List<MyTask>();
            
            //TimeSpan hours = TimeSpan.FromHours(RepeatInterval);

            MyTask TaskA = new MyTask()
            {
                BeginDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(1),
                RepeatedType = ERepeatedType.Minute,
                RepeatInterval = 2,
                StartAt = new TimeSpan(22,44,59) ,
                ToDoJob = new JobA(),
            };
            MyTask TaskB = new MyTask()
            {
                BeginDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(1),
                RepeatedType = ERepeatedType.Minute,
                RepeatInterval = 2,
                StartAt = new TimeSpan(22, 26, 59),
                ToDoJob = new JobB(),
            };
            MyTasks.Add(TaskA);
            MyTasks.Add(TaskB);
        }
    }
}
