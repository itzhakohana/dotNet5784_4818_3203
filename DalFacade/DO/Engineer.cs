namespace DO;

/// Engineer Entity represents a engineer with all its props
public record Engineer


(
        int ID,
        double Cost,
        int Level,

        string? Email = null,
        string? Name = null

)




{
    public Engineer() : this(0, 0, 0) { } //empty ctor for stage 3
}




///// Student Entity represents a 
