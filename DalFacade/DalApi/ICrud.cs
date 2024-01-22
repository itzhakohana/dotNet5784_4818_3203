namespace DalApi;
using DO;
public interface ICrud<T> where T : class
{

    /// <summary>
    /// Creates new entity object in DAL
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    int Create(T item); 

    /// <summary>
    /// Reads entity object by its ID 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    T? Read(int id); 

    /// <summary>
    /// Reads Entity object that matches the given filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    T? Read(Func<T, bool> filter);

    /// <summary>
    /// Returns entities that match the given filter. 
    /// returns all entities if a filter is not given 
    /// </summary>
    /// <param name="filter"> Filter predicate. mandatory </param>
    /// <returns></returns>
    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null);

    /// <summary>
    /// Updates entity object
    /// </summary>
    /// <param name="item"></param>
    void Update(T item);

    /// <summary>
    /// Deletes an object by its Id
    /// </summary>
    /// <param name="id"></param>
    void Delete(int id);

    /// <summary>
    /// Reset the data-base
    /// </summary>
    void Reset();
}
