namespace BO;


public class TaskInList
{
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

    public override string ToString()
    {
        return ($"ID: {Id, -6} Name: {Alias, -23} Status: {Status, -12} Task Description: {Description}");
    }
}
