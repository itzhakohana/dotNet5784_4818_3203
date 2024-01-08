namespace Dal;
using DalApi;
using DO;


/// <summary>
/// Interface implementation for Dependency entity
/// </summary>
public class DependencyImplementation : IDependency
{
    /// <summary>
    /// Adds the given Dependency to the list
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Create(Dependency item)
    {
        if (item.Id != 0)
            throw new Exception($"Cannot add Dependency with Id {item.Id} since it already exists in the system");
        int newId = DataSource.Config.NextDependancyId;
        DataSource.Dependencies.Add(item with { Id = newId });
        return newId;
    }

    /// <summary>
    /// Deletes from the list the dependency that matches the given ID
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        foreach (Dependency? dependency in DataSource.Dependencies)
            if (dependency!.Id == id)
            {
                DataSource.Dependencies.Remove(dependency);
                return;
            }
        throw new Exception($"Cannot delete Dependency with Id {id} since it does not exist in the system");
    }

    /// <summary>
    /// Returns the Dependency with the given ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        foreach (Dependency? dependency in DataSource.Dependencies)
            if (dependency!.Id == id)
                return dependency;
        return null;
    }

    /// <summary>
    /// Looks for a dependency between the two given task IDs
    /// </summary>
    /// <param name="depId">ID of the dependent task </param>
    /// <param name="depOnId">ID of the dependency task </param>
    /// <returns></returns>
    public Dependency? Read(int depId, int depOnId)
    {
        foreach (Dependency? dependency in DataSource.Dependencies)
            //returns the dependency between the two IDs if exists
            if ((dependency!.DependentTask == depId && dependency!.DependsOnTask == depOnId) || (dependency!.DependentTask == depOnId && dependency!.DependsOnTask == depId))
                return dependency;
        return null;
    }

    /// <summary>
    /// Returns a copy of the list
    /// </summary>
    /// <returns></returns>
    public List<Dependency> ReadAll()
    {
        return new List<DO.Dependency> (DataSource.Dependencies);
    }

    /// <summary>
    /// Updates an existing Dependency
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Dependency item)
    {
        foreach (Dependency? dependency in DataSource.Dependencies)
            if (dependency!.Id == item.Id)
            {
                DataSource.Dependencies.Remove(dependency);
                DataSource.Dependencies.Add(item);
                return;
            }
        throw new Exception($"Cannot update dependency with Id {item.Id} since it does not exist in the system");
    }
}
