using System.ComponentModel.Design;
using System.Runtime.Intrinsics.Arm;

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
    /// Milestone that this task is dependent on.
    /// Calculated when building schedule, populated if there is milestone in dependency, 
    /// relevant only after schedule is built.
    /// </summary>
    public BO.MilestoneInTask? Milestone { get; set; }
    /// <summary>
    /// Determines wether this is a milestone
    /// </summary>
    public bool IsMilestone { get; set; }
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
    public BO.EngineerExperience Complexity { get; set; }


    public override string ToString()
    {
        string myStr = ($"--------------------------------" +
                        $"\nId:                {Id,-10} " +
                        $"\nName:              {Alias,-10} " +
                        $"\nTask Description:  {Description, -10} " +
                        $"\nFree Remarks:      {Remarks,-10} " +
                        $"\nTask Deliverables: {Deliverables} " +
                        $"\nIs Milestone:      {IsMilestone} " +
                        $"\nCreation Date:     {CreatedAtDate,-10} " +
                        $"\nRequired Time:     {RequiredEffortTime.Days,-4} days " +
                        $"\nStatus:            {Status,-16} Complexity: {Complexity,-15}" +
                        $"\nStart Date:        {(StartDate != null ? StartDate.Value.ToShortDateString() : ""),-10}" +
                        $"\nScheduled Date:    {(ScheduledDate != null ? ScheduledDate.Value.ToShortDateString() : "")}" +
                        $"\nForecast Date:     {(ForecastDate != null ? ForecastDate.Value.ToShortDateString() : ""),-10}" +
                        $"\nDeadline Date:     {(DeadlineDate != null ? DeadlineDate.Value.ToShortDateString() : ""),-10}" +
                        $"\nCompletion Date:   {(CompleteDate != null ? CompleteDate.Value.ToShortDateString() : ""),-10}" +
                        $"\nAssigned Engineer: {(Engineer is null ? "None" : "(" + Engineer + ")"),-10}" +
                        $"\nMilestone:         {(Milestone is null ? "None" : "(" + Milestone + ")"),-10}" +
                        $"\nDependencies: ");

        if (Dependencies is not null)
        {
            
            foreach (BO.TaskInList dep in Dependencies)
                myStr = myStr + '\n' + dep;
        }
        else
            myStr += "None";
        myStr += $"\n--------------------------------"; ;

        return myStr;
    }
}