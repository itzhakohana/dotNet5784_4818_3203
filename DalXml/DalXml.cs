namespace Dal;
using DalApi;
using System;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

/// <summary>
/// Implements the interfaces for all of the data entities
/// </summary>
sealed internal class DalXml : IDal
{
    static public IDal Instance { get; } = new DalXml();

    private DalXml() { }

    public IDependency Dependency => new DependencyImplementation();
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation();
    public IUser User => new UserImplementation();

    public bool IsRealTimeClock 
    { 
        get => Config.IsRealTimeClock; 
        set => Config.IsRealTimeClock = value; 
    }

    public void Reset()
    {
        Task.Reset();
        Engineer.Reset();
        Dependency.Reset();
        User.Reset();
        Config.StartDate = null;
        Config.EndDate = null;
        Config.CurrentDate = null;
    }

    public void SetProjectSchedule(DateTime? start, DateTime? end)
    {
        Config.StartDate = start;
        Config.EndDate = end;
    }

    public DateTime? ReadStartDate()
    {
        return Config.StartDate;
    }

    public DateTime? ReadEndDate()
    {
        return Config.EndDate;
    }

    public void SetCurrentTime(DateTime? current)
    {
        Config.CurrentDate = current;
    }

    public DateTime? ReadCurrentTime()
    {
        return Config.CurrentDate;
    }
}
