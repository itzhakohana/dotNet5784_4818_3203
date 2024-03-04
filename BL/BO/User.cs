namespace BO;

public class User
{
    /// <summary>
    /// Id of user. irrelevant in case of admin
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// User name. required
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// Entrance Password. must be uniqe
    /// </summary>
    public int Password { get; set; }
    /// <summary>
    /// Last date the user logged in
    /// </summary>
    public DateTime LastLoginDate { get; set; }
    /// <summary>
    /// The Date the user profile was created
    /// </summary>
    public DateTime CreationDate { get; set; }
    /// <summary>
    /// Type of user admin/engineer. relevant for access permissions
    /// </summary>
    public BO.UserType UserType { get; set; }
    /// <summary>
    /// (only in case of engineer) Amount of tasks the user completed in the past
    /// </summary>
    public int CompletedTasks { get; set; }
    /// <summary>
    /// (only in case of engineer)User experience level
    /// </summary>
    public BO.EngineerExperience Level { get; set; }
    /// <summary>
    /// Collection of tasks the user completed in the past(only past tasks! not current!). 
    /// if no past tasks exist, will contain null
    /// </summary>
    public IEnumerable<BO.TaskInList>? PastTasks { get; set; }
    /// <summary>
    /// The task the user is currently working on. if user not currently assigned, will contain null
    /// </summary>
    public BO.Task? CurrentTask { get; set; }

}
