using DalApi;

namespace BlApi;

public interface IMilestone
{

    /// <summary>
    /// Reads milesonte by ID number
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Milesonte in the data base that match the given id, null if not found</returns>
    public BO.Milestone? Read(int id);
    /// <summary>
    /// Reads Milestone by a given condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Milestone that meets the given condition, null if non does</returns>
    public BO.Milestone? Read(Func<BO.Milestone, bool> filter);
    /// <summary>
    /// Reads all Milestones from data-base that fill the given condition. read all if no condition is given
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Collection of Milestones that match the filter. all Milestones if filter is not given</returns>
    public IEnumerable<BO.Milestone>? ReadAll(Func<BO.Milestone, bool>? filter = null);
    /// <summary>
    /// Updates the given milestone's alias, description and remarks fields.
    /// will throw exception if milestone is not found
    /// </summary>
    /// <param name="id"></param>
    /// <param name="alias"></param>
    /// <param name="description"></param>
    /// <param name="remarks"></param>
    /// <returns>The newly updated Milestone</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Milestone Update(int id, string alias, string description, string? remarks);
    /// <summary>
    /// Called once for setting the project on its way.
    /// will calculate and creat milestones from the dependency list
    /// </summary>
    public void CreatMilestones();
    /// <summary>
    /// Recieves start and end date for the project and creats schedule for 
    /// all of the project's tasks.
    /// if given time-scope is too short, will throw exception
    /// </summary>
    /// <param name="projectStart"></param>
    /// <param name="projectEnd"></param>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public void CreatProjectSchedule(DateTime projectStart, DateTime projectEnd);
    /// <summary>
    /// Creats new milestone with the given name string as Alias
    /// </summary>
    /// <param name="name"></param>
    /// <summary>
    /// Deletes milestone by Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int id);
    /// <summary>
    /// Starting the preject. will call the relevant functions for creating
    /// milestone and scheduleing all the tasks.
    /// will throw exeption if date given need is too short, or other error occured. 
    /// one SUCCESSFUL call of this function is needed to start the project.
    /// </summary>
    /// <param name="myStartDate"></param>
    /// <param name="myEndDate"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public void StartProject(DateTime myStartDate, DateTime myEndDate);
    /// <summary>
    /// Calculates the status of a milestone based on the given list of dependent tasks (prior tasks).
    /// Milestone's status will be done only of all of the prior tasks are done, 
    /// Injeopardy if at least one prior task is Injeopardy.
    /// </summary>
    /// <param name="dependencies"></param>
    /// <returns>Calculated BO.Status based on the given prior tasks</returns>
    public BO.Status CalculateMilestoneStatus(List<BO.TaskInList>? dependencies);
}
