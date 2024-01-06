namespace Dal;
using DalApi;
using DO;


/// <summary>
/// Interface implementation for Task entity
/// </summary>
public class TaskImplementation : ITask
{
    
    public int Create(Task item)
    {
        if(item.Id != 0)
            throw new Exception($"Cannot add task with Id {item.Id} since it already exists in the system");
        int newId = DataSource.Config.NextTaskId;
        Task newTask = item with { Id = newId , StartDate = DataSource.Config.startDate }; 
        DataSource.Tasks.Add(newTask);
        return newId; 
    }

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

    public Task? Read(int id)
    { 
        foreach(Task? task in DataSource.Tasks) 
            if (task!.Id == id)
                return task;
        return null;
    }

    public List<Task?> ReadAll()
    {
        List<DO.Task?> myList = new List<DO.Task?>(DataSource.Tasks);
        return myList;
    }

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
