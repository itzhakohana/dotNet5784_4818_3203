namespace DO;

/// <summary>
/// Represents the dependency of this task on another task.
/// as well as the dependency of another task of this task.
/// </summary>
/// <param name="Id"> dependency ID number </param>
/// <param name="DependentTask"> ID of a given dependent task </param>
/// <param name="DependsOnTask"> ID of the task that is dependent opun </param>
public record Dependency
(
    int Id,
    int DependentTask,
    int DependsOnTask
)
{
    public override string ToString()
    {
        return ($"Dependency ID: {Id + ".",-5} Dependent Task ID: {DependentTask + ".",-6} Depends On Task: {DependsOnTask + ".",-6}");
    }
    public Dependency() : this(0, 0, 0) { }

}


