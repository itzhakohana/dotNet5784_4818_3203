namespace BlApi;


/// <summary>
/// Interface implementation of the Logic-layer Task entity
/// </summary>
public interface ITask
{
    /// <summary>
    /// Reads all Tasks from data-base that fill the given condition. read all if no condition is given
    /// </summary>
    /// <param name="filter">Optional delegate filter</param>
    /// <returns>Collection of Tasks that meet the given condition. returns all if a condition is not given</returns>
    public IEnumerable<BO.Task>? ReadAll(Func<BO.Task, bool>? filter = null);

    /// <summary>
    /// Search for Task in the data-base by ID
    /// </summary>
    /// <param name="Id"></param>
    /// <returns>Task that matches the given ID</returns>
    public BO.Task? Read(int Id);

    /// <summary>
    /// Adds the given Task to the data-base
    /// </summary>
    /// <param name="task"></param>
    public void Add(BO.Task task);

    /// <summary>
    /// Deletes the Task by the given ID number only if no tasks exist that are dependent on this task
    /// </summary>
    /// <param name="engineer"></param>
    public void Delete(int Id);

    /// <summary>
    /// Updates Task in the data-base according to the values recieved as parmeter
    /// </summary>
    /// <param name="task"></param>
    public void Update(BO.Task task);

    /// <summary>
    /// Updates the start date of the the given task (recieved by ID).
    /// performs logical checks to ensure the date is valid (in terms of scheduled dependencies).
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="Id"></param>
    public void UpdateTaskStartDate(DateTime startDate, int Id);

    /// <summary>
    /// Reads all the tasks that this task (given by ID) is dependent on
    /// </summary>
    /// <param name="Id"></param>
    /// <returns>Collection of tasks that the given task is dependent on</returns>
    public IEnumerable<BO.TaskInList>? ReadDependsOnTasks(int Id);

    /// <summary>
    /// Assignes the given engineer to the given task.
    /// if no engineer is given, will delete the current engineer working on the task
    /// </summary>
    /// <param name="task"></param>
    /// <param name="engineer"></param>
    public void UpdateAssignedEngineer(BO.Task task, BO.Engineer? engineer = null);

    public BO.TaskInEngineer? ReadTaskInEngineer(int id);
}
