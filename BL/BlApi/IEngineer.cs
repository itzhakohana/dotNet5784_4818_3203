using DalApi;

namespace BlApi;

public interface IEngineer
{
    /// <summary>
    /// Adds the given Engineer to the data-base
    /// </summary>
    /// <param name="engineer"></param>
    /// <exception cref="BO.BlInvalidValuesException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public void Add(BO.Engineer engineer);
    /// <summary>
    /// Deletes Engineer by given Id number
    /// </summary>
    /// <param name="Id"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public void Delete(int Id);
    /// <summary>
    /// Search for Engineer in the data-base by ID
    /// </summary>
    /// <param name="Id"></param>
    /// <returns>BO.Engineer if found, null if not found</returns>
    public BO.Engineer? Read(int Id);
    /// <summary>
    /// Searches for engineer in the data base by filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>The found BO.Engineer type, null if not found</returns>
    public BO.Engineer? Read(Func<BO.Engineer, bool> filter);
    /// <summary>
    /// Gives all Engineers from data-base that fill the given condition. 
    /// gives all if no condition is given
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Collection of BO.Engineer types that match the given filter.
    /// if no filter is given, returns all engineers</returns>
    public IEnumerable<BO.Engineer>? ReadAll(Func<BO.Engineer, bool>? filter = null);
    /// <summary>
    /// Updates Engineer in the data-base according to the values recieved as parmeter
    /// </summary>
    /// <param name="engineer"></param>
    /// <exception cref="BO.BlInvalidValuesException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Engineer engineer);
    /// <summary>
    /// Deletes all existing Engineers
    /// </summary>
    public void Reset();
    /// <summary>
    /// Updates the given engineer's assigned task progress.
    /// if engineer has'nt yet started to work on his task, will set task status to "on track"(engineer starts to work).
    /// if engineer already started working on his task, will set task as complete.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="currentTime"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public void UpdateEngineerProgress(int id, DateTime currentTime);
    /// <summary>
    /// Returns the given engineer's assigned task.
    /// will only return tasks which are not yet comleted (status != Done)
    /// </summary>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Task ReadEngineerAssignedTask(int id);
    /// <summary>
    /// Reads all the engineers that can be assigned to the given task (given by id).
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Collection of BO.EngineerInTask of engineers available for assignment</returns>
    public IEnumerable<BO.EngineerInTask>?  ReadEngineersInTask(int id);
    /// <summary>
    /// Reads all the Engineers that can be assigned to a task with the given comlexity.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Collection of BO.EngineerInTask that match the given task comlexity</returns>
    public IEnumerable<BO.EngineerInTask>? ReadEngineersInTask(BO.EngineerExperience level);
}
