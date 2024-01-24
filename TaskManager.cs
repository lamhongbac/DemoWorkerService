using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoWorkerService
{
    public class MyTaskManager
    {
        public DateOnly BeginDate { get; set; } //ngay bat dau
        public string RepeatedType { get; set; } //minutely, hourly, daily, weekly, monthly
        public int RepeatInterval { get; set; } //30p, 22g, 1day, 1week
        public decimal StartAt { get; set; } //thoi diem chay task
        public DateOnly EndDate { get; set; } //ngay ket thuc
        public IJob ToDoJob { get; set; }
    }
}
