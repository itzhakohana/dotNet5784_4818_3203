namespace BO;


public class TaskInList
{
    /// <summary>
    /// Task ID number
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Description of the task
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Task name
    /// </summary>
    public string Alias { get; set; }
    /// <summary>
    /// Current task status
    /// </summary>
    public BO.Status Status { get; set; }
}
