namespace Dal;


/// <summary>
/// Contains our Data base in the form of lists
/// and an internal Config class
/// </summary>
internal static class DataSource
{
    internal static List<DO.Task?> Tasks { get; } = new();
    internal static List<DO.Dependency?> Dependencies { get; } = new();
    internal static List<DO.Engineer?> Engineers { get; } = new();


    /// <summary>
    /// Configures running ID numbers for Tasks and Dependencies
    /// and start&end dates for a given project(task)
    /// </summary>
    internal static class Config
    {
        //start and deadline date for a project's schedule
        internal static DateTime? startDate = null;
        internal static DateTime? deadlineDate = null;

        //running number for Task ID
        internal const int startTaskId = 1000;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        //running number for Dependency ID
        internal const int startDependancyId = 100;
        private static int nextDependancyId = startDependancyId;
        internal static int NextDependancyId { get => nextDependancyId++; }
    }

}
