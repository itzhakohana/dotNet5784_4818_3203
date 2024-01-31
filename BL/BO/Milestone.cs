namespace BO;

/// <summary>
/// Represents a milestone in the system
/// </summary>
public class Milestone
{
    /// <summary>
    /// ID number of the Task that is the Milestone. 
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Description of the Milestone
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Milestone alias(name)
    /// </summary>
    public string Alias { get; set; }
    /// <summary>
    /// Date at which the Milestone was added to the system
    /// </summary>
    public DateTime CreatedAtDate { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    /// <summary>
    /// Milestone current status
    /// </summary>
    BO.Status status { get; set; }
    /// <summary>
    /// Precentage of tasks completed so far.
    /// </summary>
    public double CompletionPercentage { get; set; }
    public string? Remarks { get; set; }
    public List<BO.TaskInList>? Dependencies { get; set; }
}