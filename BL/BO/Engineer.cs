namespace BO;


/// <summary>
/// Represents Engineer
/// </summary>
public class Engineer
{
    /// <summary>
    /// ID number of the engineer. uniqe to each engineer
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Engineer's name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Engineer's email-address
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// Engineer's level of expertise
    /// </summary>
    public BO.EngineerExperience Level { get; set; }
    /// <summary>
    /// Daily cost of the engineer including salary, workplace, tools
    /// </summary>
    public double Cost { get; set; }
    /// <summary>
    /// Task assigned to this Engineer. only one task can be assigned to an Engineer at a time
    /// </summary>
    public BO.TaskInEngineer? Task { get; set; }


    public override string ToString()
    {
        return ($"Id: {Id, -10} Name: {Name, -20} Email: {Email, -20} Level: {Level.ToString(), -18} Cost: {Cost, -4} Assigned Task: {(Task is not null ? Task : "Unassigned")}");
    }
}
