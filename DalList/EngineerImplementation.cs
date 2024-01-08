namespace Dal;
using DalApi;
using DO;

/// <summary>
/// Interface implementation for Engineer entity
/// </summary>
public class EngineerImplementation : IEngineer
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
        foreach (Engineer? engineer in DataSource.Engineers)
            if (engineer!.Id == id)
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
        foreach (Engineer? engineer in DataSource.Engineers)
            if (engineer!.Id == id)
                return engineer;
        return null;
    }

    /// <summary>
    /// Returns a copy of the list
    /// </summary>
    /// <returns></returns>
    public List<Engineer> ReadAll()
    {
        List<DO.Engineer?> myList = new List<DO.Engineer?>(DataSource.Engineers);
        return myList;
    }

    /// <summary>
    /// Updates an Engineer in the list that matches the Given Engineer's ID
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Engineer item)
    {
        if (Read(item.Id) != null)
        {
            DataSource.Engineers.Remove(Read(item.Id));
            DataSource.Engineers.Add(item);
            return;
        }
        throw new Exception($"Cannot update Engineer with Id {item.Id} since it does not exist in the system");
    }
}
