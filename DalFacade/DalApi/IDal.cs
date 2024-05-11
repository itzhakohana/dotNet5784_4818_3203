namespace DalApi;
using DO;

public interface IDal
{
    IDependency Dependency { get; }
    IEngineer Engineer { get; }
    ITask Task { get; }
    IUser User { get; }
    bool IsRealTimeClock { get; set; }
    void SetProjectSchedule(DateTime? start, DateTime? end);
    DateTime? ReadStartDate();
    DateTime? ReadEndDate();
    void SetCurrentTime(DateTime? current);
    DateTime? ReadCurrentTime();
    void Reset();
}

