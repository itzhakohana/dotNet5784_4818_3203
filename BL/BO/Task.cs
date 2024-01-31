namespace BO;

public class Task 
{
    /// <summary>
    /// Task ID number. uniqe for each task
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Desctiption of the task
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Task name
    /// </summary>
    public string Alias { get; set; }
    /// <summary>
    /// Date when the task was added to the system
    /// </summary>
    public DateTime CreatedAtDate { get; set; }
    /// <summary>
    /// Task current status.
    /// </summary>
    public BO.Status Status { get; set; }
    /// <summary>
    /// List of Dependent-tasks. relevant only before schedule is built
    /// </summary>
    public List<BO.TaskInList>?  Dependencies { get; set; }
    /// <summary>
    /// Calculated when building schedule, populated if there is milestone in dependency, 
    /// relevant only after schedule is built
    /// </summary>
    public BO.MilestoneInTask? Milestone { get; set; }
    /// <summary>
    /// Time in days required to complete the task
    /// </summary>
    public TimeSpan RequiredEffortTime { get; set; }
    /// <summary>
    /// Actual start date
    /// </summary>
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// Planned start date
    /// </summary>
    public DateTime? ScheduledDate { get; set; }
    /// <summary>
    /// Calcualed planned completion date
    /// </summary>
    public DateTime? ForecastDate { get; set; }
    /// <summary>
    /// Latest Completion date allowd
    /// </summary>
    public DateTime? DeadlineDate { get; set; }
    /// <summary>
    /// Actual completion date of the task
    /// </summary>
    public DateTime? CompleteDate { get; set; }
    /// <summary>
    /// Description of deliverables upon comletion
    /// </summary>
    public string Deliverables { get; set; }
    /// <summary>
    /// Free comments and remarks
    /// </summary>
    public string? Remarks { get; set; }
    /// <summary>
    /// The Engineer that is currently working on the Task
    /// </summary>
    public BO.EngineerInTask? Engineer { get; set; }
    /// <summary>
    /// Complexity of the task
    /// </summary>
    public BO.EngineerExperience Copmlexity { get; set; }
      
}