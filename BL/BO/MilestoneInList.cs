namespace BO;

public class MilestoneInList
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public BO.Status Status { get; set; }
    public double CompletionPercentage { get; set; }

    public override string ToString()
    {
        return ($"ID: {Id,-6} Alias: {Alias, -15} Status: {Status, - 13} Completion Progress: {CompletionPercentage, -6} Description: {Description}");
    }
}
