namespace DO;

/// <summary>
/// Represents a task in the system
/// </summary>
/// <param name="Id"> ID number of the task </param>
/// <param name="Alias"> Name of the task </param>
/// <param name="Description"> Description of the the task </param>
/// <param name="CreatedAtDate"> Date at which the task was added to the system </param>
/// <param name="RequiredEffortTime"> Amount of work-days required for completeing the task </param>
/// <param name="IsMilestone"> Wether the task is a milestone </param>
/// <param name="Complexity"> Minimum level of engineer experience required </param>
/// <param name="StartDate"> Actual starting date </param>
/// <param name="ScheduledDate"> Planned starting date </param>
/// <param name="DeadlineDate"> Planned date of completion </param>
/// <param name="CompleteDate"> Actual date of completion </param>
/// <param name="Deliverables"></param>
/// <param name="Remarks"> Remarks from meetings </param>
/// <param name="EngineerId"> ID of the assigned engineer  </param>
public record Task
(
    int Id,
    string Alias,
    string Description,
    DateTime CreatedAtDate,
    TimeSpan? RequiredEffortTime = null,
    bool IsMilestone = false,
    DO.EngineerExperience Complexity = 0,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? EngineerId = null
)
{
    public Task() : this(0, "", "", DateTime.Now) { } 

}
