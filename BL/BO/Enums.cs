namespace BO;


public enum EngineerExperience 
{
    Beginner = 1,
    AdvancedBeginner,
    Intermediate,
    Advanced,
    Expert,
    None //for PL
}

public enum Status
{
    Unscheduled, 
    Scheduled, 
    OnTrack, 
    InJeopardy, 
    Done
}

public enum UserType
{
    Admin = 1,
    Engineer
}