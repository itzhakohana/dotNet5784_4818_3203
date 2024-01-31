namespace Dal;
using DalApi;
using System.Diagnostics;
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

    public void Reset()
    {
        Task.Reset();
        Engineer.Reset();
        Dependency.Reset();
    }
}
