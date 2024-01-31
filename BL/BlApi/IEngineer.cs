namespace BlApi;

public interface IEngineer
{

    /// <summary>
    /// Reads all Engineers from data-base that fill the given condition. read all if no condition is given
    /// </summary>
    /// <param name="filter">Optional delegate filter</param>
    /// <returns>Collection of Engineers that meet the given condition. all if a condition is not given</returns>
    public IEnumerable<BO.Engineer>? ReadAll(Func<BO.Engineer, bool>? filter = null);

    /// <summary>
    /// Search for Engineer in the data-base by ID
    /// </summary>
    /// <param name="Id"></param>
    /// <returns>Engineer that matches the given ID</returns>
    public BO.Engineer? Read(int Id);

    /// <summary>
    /// Adds the given Engineer to the data-base
    /// </summary>
    /// <param name="engineer"></param>
    public void Add(BO.Engineer engineer);

    /// <summary>
    /// Deletes Engineer from the Data-base that 1. matches the given ID number 
    /// 2.Engineer isnt currently working on a task and hasnt completed a task in the past
    /// </summary>
    /// <param name="engineer"></param>
    public void Delete(int Id);

    /// <summary>
    /// Updates Engineer in the data-base according to the values recieved as parmeter
    /// </summary>
    /// <param name="engineer"></param>
    public void Update(BO.Engineer engineer);


}
