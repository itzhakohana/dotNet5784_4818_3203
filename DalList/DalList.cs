namespace Dal;
using DalApi;
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

    /// <summary>
    /// Empties the Data-Base
    /// </summary>
    public void Reset()
    {
        Dependency.Reset();
        Engineer.Reset();
        Task.Reset();
    }
}
