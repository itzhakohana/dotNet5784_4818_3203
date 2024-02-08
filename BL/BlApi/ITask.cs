using BO;
using DalApi;

namespace BlApi;


/// <summary>
/// Interface implementation of the Logic-layer Task entity
/// </summary>
public interface ITask
{
    /// <summary>
    /// Adds new engineer to the data base. calculates dependencies and status
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BO.BlInvalidValuesException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Add(BO.Task task);
    /// <summary>
    /// Deletes the Task by the given ID number only if no tasks exist that are dependent on this task
    /// </summary>
    /// <param name="Id"></param>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int Id);
    /// <summary>
    /// Search for Task in the data-base by ID
    /// </summary>
    /// <param name="Id"></param>
    /// <returns>BO.Task if found. null if not</returns>
    public BO.Task? Read(int Id);
    /// <summary>
    /// Searchs a task by filter function (condition)
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>BO.Task if found. null if not </returns>
    public BO.Task? Read(Func<BO.Task, bool> filter);
    /// <summary>
    /// Reads all Tasks from data-base that fill the given condition. read all if no condition is given
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Collection of tasks that match the filter. all tasks if filter is not given</returns>
    public IEnumerable<BO.Task>? ReadAll(Func<BO.Task, bool>? filter = null);
    /// <summary>
    /// Updates Task in the data-base according to the values recieved as parmeter
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BO.BlInvalidValuesException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Task task);
    /// <summary>
    /// Assignes the given engineer to the given task.
    /// if no engineer is given, will set assigned engineer to null.
    /// also sets the starting date (to Date.Now if no date is given)
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="engId"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public void UpdateAssignedEngineerAndStartWork(int taskId, int? engId = null, DateTime? startDate = null);
    /// <summary>
    /// Assigns the given task to the given engineer without starting the work (will NOT set starting date)
    /// </summary>
    /// <param name="engId"></param>
    /// <param name="taskId"></param>
    public void UpdateAssignedEngineer(int engId, int taskId);
    /// <summary>
    /// Calculates task status from dateTime fields
    /// </summary>
    /// <param name="CompleteDate"></param>
    /// <param name="StartDate"></param>
    /// <param name="ScheduledDate"></param>
    /// <param name="DeadlineDate"></param>
    /// <param name="ForecastDate"></param>
    /// <returns>BO.Status type according to the given dates</returns>
    public BO.Status CalculateStatus(DateTime? CompleteDate, DateTime? StartDate, DateTime? ScheduledDate,
        DateTime? DeadlineDate, DateTime? ForecastDate = null);
    /// <summary>
    /// Calculates status from given dalTask (also works on milestones)
    /// </summary>
    /// <param name="dalTask"></param>
    /// <returns>BO.status of the given task or milestone</returns>
    public BO.Status CalculateStatus(DO.Task dalTask);
    /// <summary>
    /// Returns the parent Milestone of the given task
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BO.MilestoneInTask type of the parent milestone</returns>
    public BO.MilestoneInTask? CalculateMilestoneInTask(int id);
    /// <summary>
    /// Reads a task by filter function (condition). if found returns 
    /// TaskInList type (a concise vertion of task entity)
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>BO.TaskInList type of the found task. null if not found</returns>
    public BO.TaskInList? ReadTaskInList(Func<TaskInList, bool> filter);
    /// <summary>
    /// Gives a Collection of TaskInList types that match a given filter condition.
    /// all tasks if filter function is not given
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Collection of TaskInList types that match the given filter. if no
    /// filter is given, returns all tasks in the data base</returns>
    public IEnumerable<TaskInList>? ReadAllTasksInList(Func<TaskInList, bool>? filter = null);
    /// <summary>
    /// Updates the given task's scheduled start date (and consequently, ForecastDate) on conditions:
    /// 1 all prior task dependencies are already have scheduled date.
    /// 2 the task's scheduled date must be later then its preceeding tasks(task must start after them)
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="scheduledDate"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public void CalculateScheduledDate(int taskId, DateTime scheduledDate);
    /// <summary>
    /// Gives all task that are available for the given engineer
    /// </summary>
    /// <param name="engineerId"></param>
    /// <returns>Collection of tasks that the given engineer is allowed to work on</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public IEnumerable<BO.TaskInList>? ReadAllAvailableTasks(int engineerId);
    /// <summary>
    /// Determines wether the project has started already by checking that all tasks and milestones
    /// are initialized with scheduled and deadline dates.
    /// </summary>
    /// <returns>True if project has started, false if not</returns>
    public bool ProjectHasStarted();
    /// <summary>
    /// Deletes all existing tasks and dependencies
    /// </summary>
    public void Reset();
    /// <summary>
    /// Updates the given dependencies list to the given task
    /// </summary>
    /// <param name="taskId"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    public void UpdateTaskDependencies(int taskId, IEnumerable<BO.TaskInList>? dependencies);
    /// <summary>
    /// Accepts milestone alias string and gives a collection
    /// of tasks which are dependent on (come after) said milestone
    /// </summary>
    /// <param name="alias"></param>
    /// <returns>Collection of tasks dependent on the given milestone</returns>
    public IEnumerable<BO.Task>? ReadTasksByMilestone(string alias);
}
