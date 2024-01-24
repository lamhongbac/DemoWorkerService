
namespace DemoWorkerService
{
    public interface IMyTask
    {
        DateOnly BeginDate { get; set; }
        DateOnly EndDate { get; set; }
        ERepeatedType RepeatedType { get; set; }
        int RepeatInterval { get; set; }
        TimeSpan StartAt { get; set; }
        IJob ToDoJob { get; set; }
    }
    public enum ERepeatedType
    {
        Minute, Hourly, Daily, Weekly, Monthly, Yearly
    }
}