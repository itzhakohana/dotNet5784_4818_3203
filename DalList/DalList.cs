namespace Dal;
using DalApi;


/// <summary>
/// Implements all of our Data-Entities interfaces by calling the 
/// implementation-classes for each individual Data-Entity
/// </summary>
sealed public class DalList : IDal
{
    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    /// <summary>
    /// Empties the Data-Base
    /// </summary>
    public void Reset()
    {
        DataSource.Dependencies.Clear();
        DataSource.Engineers.Clear();
        DataSource.Tasks.Clear();
    }
}
