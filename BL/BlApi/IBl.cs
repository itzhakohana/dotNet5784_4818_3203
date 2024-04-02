using BlImplementation;

namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }
    public IDatesControl DateControl { get; }
    public IUser User { get; }
    public DateTime Clock { get; }
    public void ClockAddHour();
    public void ClockAddDay();
    public void ClockAddMonth();
    public void SaveClock();
    public void InitializeDataBase();
    /// <summary>
    /// Resets all data in the system (tasks, engineers, dependencies, dates)
    /// </summary>
    public void Reset();
}
