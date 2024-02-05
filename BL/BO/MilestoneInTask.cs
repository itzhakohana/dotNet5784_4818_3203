using System.Xml.Linq;

namespace BO;

public class MilestoneInTask
{
    public int Id { get; set; }
    public string Alias { get; set; }
    public override string ToString()
    {
        return ($"ID: {Id,-6} Alias: {Alias}");
    }
}
