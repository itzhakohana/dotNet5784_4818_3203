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
    /// Phone number of the engineer
    /// </summary>
    public string? Phone { get; set; }
    /// <summary>
    /// Engineers display-Picture 
    /// </summary>
    public string Picture { get; set; }
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
        return ($"\nId: {Id, -9} " +
                $"Name: {Name, -20} " +
                $"\nEmail: {Email, -20} " +
                $"\nPhone: {Phone,-12} " +
                $"\nLevel: {Level, -18} " +
                $"\nCost: {Cost, -4} " +
                $"\nAssigned Task: {(Task is not null ? Task : "Unassigned")} ");
    }
}
