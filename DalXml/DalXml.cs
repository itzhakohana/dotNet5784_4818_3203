namespace Dal;
using DalApi;
using System.Xml.Linq;

/// <summary>
/// Implements the interfaces for all of the data entities
/// </summary>
sealed public class DalXml : IDal
{
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
