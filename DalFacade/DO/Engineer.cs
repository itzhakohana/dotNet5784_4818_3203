﻿
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
    double Cost,
    DO.EngineerExperience Level,
    string Email,
    string Name
)
{
    public DateTime? StartDate;

    public Engineer() : this(0, 0, 0, "", "") { } 
}

