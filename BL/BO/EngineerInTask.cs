namespace BO;


public class EngineerInTask
{
    public int Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return ($"ID: {Id, -11} Name: {Name}");
    }
}
