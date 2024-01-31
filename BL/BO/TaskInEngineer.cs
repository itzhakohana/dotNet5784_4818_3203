namespace BO;

public class TaskInEngineer
{
    /// <summary>
    /// Task ID number
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Task name
    /// </summary>
    public string Alias { get; set; }

    public override string ToString()
    {
        return ($"Id: {Id,-5} Name: {Alias,-8}");
    }
}
