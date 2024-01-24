using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService
{
    public class MyTask : IMyTask
    {
        public DateOnly BeginDate { get; set; } //ngay bat dau
        public ERepeatedType RepeatedType { get; set; } //minutely, hourly, daily, weekly, monthly
        public int RepeatInterval { get; set; } //30p, 22g, 1day, 1week
        public TimeSpan StartAt { get; set; } //thoi diem chay task
        public DateOnly EndDate { get; set; } //ngay ket thuc
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
                BeginDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                RepeatedType = ERepeatedType.Minute,
                RepeatInterval = 1,
                StartAt = new TimeSpan(17,11,59) ,
                ToDoJob = new JobA(),
            };
            MyTask TaskB = new MyTask()
            {
                BeginDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                RepeatedType = ERepeatedType.Minute,
                RepeatInterval = 2,
                StartAt = new TimeSpan(17, 11, 59),
                ToDoJob = new JobB(),
            };
            MyTasks.Add(TaskA);
            MyTasks.Add(TaskB);
        }
    }
}
