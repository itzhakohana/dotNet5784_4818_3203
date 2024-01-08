namespace Dal;
using DalApi;
using DO;


/// <summary>
/// Interface implementation for Task entity
/// </summary>
public class TaskImplementation : ITask
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
            throw new Exception($"Cannot add task with Id {item.Id} since it already exists in the system");
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
        foreach (Task? task in DataSource.Tasks)
            if (task!.Id == id)
            {       
                DataSource.Tasks.Remove(task);
                return;
            }
        throw new Exception($"Cannot delete Task with Id {id} since it does not exist in the system");
    }

    /// <summary>
    /// Returns the Task with the given ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task? Read(int id)
    { 
        foreach(Task? task in DataSource.Tasks) 
            if (task!.Id == id)
                return task;
        return null;
    }

    /// <summary>
    /// Returns a copy of the Tasks-list
    /// </summary>
    /// <returns></returns>
    public List<Task?> ReadAll()
    {
        return new List<DO.Task?>(DataSource.Tasks);
    }
    /// <summary>
    /// Updates an existing Task
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Task item)
    {
        foreach (Task? task in DataSource.Tasks)
            if (task!.Id == item.Id)
            {
                DataSource.Tasks.Remove(task);
                DataSource.Tasks.Add(item);
                return;
            }
        throw new Exception($"Cannot update task with Id {item.Id} since it does not exist in the system");
    }
}
