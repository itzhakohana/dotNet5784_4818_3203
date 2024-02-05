using BlApi;
using System;

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
    public BO.Status Status { get; set; }
    /// <summary>
    /// Precentage of tasks completed so far.
    /// </summary>
    public double CompletionPercentage { get; set; }
    public string? Remarks { get; set; }
    /// <summary>
    /// Tasks Dependent on this milestone
    /// </summary>
    public List<BO.TaskInList>? Dependencies { get; set; }

    public override string ToString()
    {
        string myStr = ($"--------------------------------" +
                        $"\nId:                {Id,-10} " +
                        $"\nName:              {Alias,-10} " +
                        $"\nTask Description:  {Description,-10} " +
                        $"\nFree Remarks:      {Remarks,-10} " +
                        $"\nStatus:            {Status,-16} "+
                        $"\nCreation Date:     {CreatedAtDate,-10} " +
                        $"\nForecast Date:     {(ForecastDate != null ? ForecastDate.Value.ToShortDateString() : ""),-10}" +
                        $"\nDeadline Date:     {(DeadlineDate != null ? DeadlineDate.Value.ToShortDateString() : ""),-10}" +
                        $"\nCompletion Date:   {(CompleteDate != null ? CompleteDate.Value.ToShortDateString() : ""),-10}" +
                        $"\nCompletion Percentage: {$"{CompletionPercentage * 100:F2}%",-10}" +
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