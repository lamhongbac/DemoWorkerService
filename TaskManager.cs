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
    /// <summary>
    /// Khong can Weekly, Monthly, Yearly
    /// chi can daily la co the tam thoi giai quyet dc
    /// sau nay se bo xung sau
    /// </summary>
    public enum ERepeatedType
    {
        Minute, Hourly, Daily
    }

    public class MyTask 
    {
        public MyTask()
        {
            
        }
        public string BeginDate { get; set; } //ngay bat dau
        public string RepeatedType { get; set; } //minutely, hourly, daily, weekly, monthly
        public int RepeatInterval { get; set; } //30p, 22g, 1day, 1week
        public string StartAt { get; set; } //thoi diem chay task
        public string EndDate { get; set; } //ngay ket thuc
        public IJob ToDoJob { get; set; }

        public DateTime GetBeginDate(string type)
        {
            DateTime dateTime = DateTime.Now;
            if (type == "BeginDate")
                dateTime= DateTime.Parse(BeginDate);
            if (type == "EndDate")
                dateTime = DateTime.Parse(EndDate);
            return dateTime;
        }
        public TimeSpan GetStartAt()
        {
            int hours=Convert.ToInt32(StartAt.Substring(0,2));
            int minutes = Convert.ToInt32(StartAt.Substring(3, 2));
            return new TimeSpan(hours, minutes,0);
        }
    }
    public class MyTaskManager
    {
        public List<MyTask> MyTasks { get; set; }
        //public MyTaskManager()
        //{

        //    MyTasks = new List<MyTask>();
            
        //    //TimeSpan hours = TimeSpan.FromHours(RepeatInterval);

        //    MyTask TaskA = new MyTask()
        //    {
        //        BeginDate = DateTime.Now.AddDays(-1),
        //        EndDate = DateTime.Now.AddDays(1),
        //        RepeatedType = ERepeatedType.Minute,
        //        RepeatInterval = 2,
        //        StartAt = DateTime.Now.AddSeconds(30).TimeOfDay,            
        //        ToDoJob = new JobA(),
        //    };
        //    MyTask TaskB = new MyTask()
        //    {
        //        BeginDate = DateTime.Now.AddDays(-1),
        //        EndDate = DateTime.Now.AddDays(1),
        //        RepeatedType = ERepeatedType.Minute,
        //        RepeatInterval = 2,
        //        StartAt = DateTime.Now.AddMinutes(1).TimeOfDay,
        //        ToDoJob = new JobB(),
        //    };
        //    MyTasks.Add(TaskA);
        //    MyTasks.Add(TaskB);
        //}
    }
}
