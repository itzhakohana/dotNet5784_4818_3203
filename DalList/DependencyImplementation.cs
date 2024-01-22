namespace Dal;
using DalApi;
using DO;
using System.Runtime.InteropServices;


/// <summary>
/// Interface implementation for Dependency entity
/// </summary>
internal class DependencyImplementation : IDependency
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
            throw new DalAlreadyExistException($"Cannot add Dependency with Id {item.Id} since it already exists in the system");
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
        Dependency? dependency = Read(id);
        if (dependency != null)
        {
            DataSource.Dependencies.Remove(dependency);
            return;
        }
        throw new DalDoesNotExistException($"Cannot delete Dependency with Id {id} since it does not exist in the system");
    }

    /// <summary>
    /// Returns the Dependency with the given ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(item => item!.Id == id); 
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
    /// Reads a Dependency by a given filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencies.FirstOrDefault(item => filter(item!));
    }

    /// <summary>
    /// Returns all the Dependencies that satisfy a given condition.
    /// if a condition is not provided, returns all Dependencies.
    /// </summary>
    /// <param name="filter">Optional filter condition.</param>
    /// <returns></returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter != null)
            return DataSource.Dependencies.Where(item => filter!(item!));

        return DataSource.Dependencies;
    }

    public void Reset()
    {
        DataSource.Dependencies.Clear();
    }

    /// <summary>
    /// Updates an existing Dependency
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Dependency item)
    {
        Dependency? dependency = Read(item.Id);
        if (dependency != null)
        {
            DataSource.Dependencies.Remove(dependency);
            DataSource.Dependencies.Add(item);
            return;
        }
        throw new DalDoesNotExistException($"Cannot update dependency with Id {item.Id} since it does not exist in the system");
    }
}
