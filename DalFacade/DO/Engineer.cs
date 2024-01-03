namespace DO;

///Engineer Entity represents a engineer with all its props
public record Engineer
(
    int ID,
    double Cost,
    DO.EngineerExperience? level,
    string? Email,
    string? Name
)
{
    public Engineer() : this(0, 0, null, "", "") { } //empty ctor for stage 3
}

