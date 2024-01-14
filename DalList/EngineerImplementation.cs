namespace Dal;
using DalApi;
using DO;

/// <summary>
/// Interface implementation for Engineer entity
/// </summary>
internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// Adds the given Engineer to the list. if not present already 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Create(Engineer item)
    {
        //if a student with the same Id already exists
        if (Read(item.Id) != null) 
            throw new Exception($"A student with Id {item.Id} already exists in the system");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    /// <summary>
    /// Deletes the Engineer that matches the given ID
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        Engineer? engineer = Read(id);
        if (engineer != null)
        {
            DataSource.Engineers.Remove(engineer);
            return;
        }
        throw new Exception($"Cannot delete Engineer with Id {id} since it does not exist in the system");
    }

    /// <summary>
    /// Returns the Engineer with the given ID number, null if not found
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(item => item!.Id == id);
    }

    /// <summary>
    /// Reads an Engineer by a given filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(item => filter(item!));
    }

    /// <summary>
    /// Returns all the Engineers that satisfy a given condition.
    /// if a condition is not provided, returns all Engineers.
    /// </summary>
    /// <param name="filter">Optional filter condition.</param>
    /// <returns></returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter != null)
            return DataSource.Engineers.Where(item => filter!(item!));

        return DataSource.Engineers;
    }

    /// <summary>
    /// Updates an Engineer in the list that matches the Given Engineer's ID
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Engineer item)
    {
        Engineer? engineer = Read(item.Id);
        if (engineer != null)
        {
            DataSource.Engineers.Remove(engineer);
            DataSource.Engineers.Add(item);
            return;
        }
        throw new Exception($"Cannot update Engineer with Id {item.Id} since it does not exist in the system");
    }
}
