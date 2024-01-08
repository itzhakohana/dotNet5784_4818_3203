
namespace DO;

/// <summary>
/// Represents engineer working in the company
/// </summary>
/// <param name="Id"> ID number of the engineer (uniqe) </param>
/// <param name="Cost"> Daily cost of the engineer including salary, workplace, tools </param>
/// <param name="Level"> Expertise level </param>
/// <param name="Name"> Engineer's name </param>
/// <param name="Email"> Engineer's Email address </param>
public record Engineer
(
    int Id,
    DO.EngineerExperience Level,
    string Name,
    string Email,
    double Cost = 0
)
{
    public override string ToString()
    {
        return ($"Engineer ID: {Id + ".",-10} Engineer Name: {Name + ".",-18} Expertise Level: {Level,-18} Email Address: {Email,-20} " +  $"{(Cost != 0 ? $"Daily Cost = {Cost, -4}" : "")}");
    }
    public Engineer() : this(0, 0, "", "") { } 
}

