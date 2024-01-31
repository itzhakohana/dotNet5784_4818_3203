namespace BO;

public class MilestoneInList
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Alias { get; set; }
    BO.Status Status { get; set; }
    public double CompletionPercentage { get; set; }
}
