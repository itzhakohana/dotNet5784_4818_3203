namespace Dal;
using DalApi;
using System;
using System.Reflection.Metadata.Ecma335;



/// <summary>
/// Implements all of our Data-Entities interfaces by calling the 
/// implementation-classes for each individual Data-Entity
/// </summary>
sealed internal class DalList : IDal
{
    static public IDal Instance { get; } = new DalList();

    private DalList() { }

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public IUser User => new UserImplementation();

    public bool IsRealTimeClock 
    { 
        get => throw new NotImplementedException(); 
        set => throw new NotImplementedException(); 
    }

    /// <summary>
    /// Empties the Data-Base
    /// </summary>
    public void Reset()
    {
        Dependency.Reset();
        Engineer.Reset();
        Task.Reset();
        User.Reset();
    }

    public void SetProjectSchedule(DateTime? start, DateTime? end)
    {
        throw new NotImplementedException();
    }

    public DateTime? ReadStartDate()
    {
        throw new NotImplementedException();
    }

    public DateTime? ReadEndDate()
    {
        throw new NotImplementedException();
    }

    public void SetCurrentTime(DateTime? current)
    {
        throw new NotImplementedException();
    }

    public DateTime? ReadCurrentTime()
    {
        throw new NotImplementedException();
    }
}
