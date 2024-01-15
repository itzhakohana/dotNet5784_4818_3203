namespace Dal;
using DalApi;
using DO;


/// <summary>
/// Interface implementation for Task entity
/// </summary>
internal class TaskImplementation : ITask
{
    
    /// <summary>
    /// Creats and adds a new task to the list
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Create(Task item)
    {
        if(item.Id != 0)
            throw new DalAlreadyExistException($"Cannot add task with Id {item.Id} since it already exists in the system");
        int newId = DataSource.Config.NextTaskId;
        DataSource.Tasks.Add(item with { Id = newId, StartDate = DataSource.Config.startDate });
        return newId; 
    }

    /// <summary>
    /// Deletes a Task that matches the given ID from the list 
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        Task? task = Read(id);
        if(task != null)
        {
            DataSource.Tasks.Remove(task);
            return;
        }
        throw new DalDoesNotExistException($"Cannot delete Task with Id {id} since it does not exist in the system");
    }

    /// <summary>
    /// Returns the Task with the given ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(item => item!.Id == id);
    }

    /// <summary>
    /// Reads a Task by a given filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task? Read(Func<Task, bool> filter)
    {
        return DataSource.Tasks.FirstOrDefault(item => filter(item!));
    }

    /// <summary>
    /// Returns all the Tasks that satisfy a given condition.
    /// if a condition is not provided, returns all Tasks.
    /// </summary>
    /// <param name="filter">Optional filter condition.</param>
    /// <returns></returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        if (filter != null)
            return DataSource.Tasks.Where(item => filter!(item!));

        return DataSource.Tasks;
    }

    /// <summary>
    /// Updates an existing Task
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Task item)
    {
        Task? task = Read(item.Id);
        if(task != null)
        {
            DataSource.Tasks.Remove(task);
            DataSource.Tasks.Add(item);
            return;
        }
        throw new DalDoesNotExistException($"Cannot update task with Id {item.Id} since it does not exist in the system");
    }
}
