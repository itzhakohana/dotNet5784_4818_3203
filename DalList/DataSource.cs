namespace Dal;


/// <summary>
/// 
/// </summary>
internal static class DataSource
{
    internal static List<DO.Task?> Tasks { get; } = new();
    internal static List<DO.Dependency?> Dependencies { get; } = new();
    internal static List<DO.Engineer?> Engineers { get; } = new();


    /// <summary>
    /// This class configures running ID numbers for Tasks and Dependencies
    /// </summary>
    internal static class Config
    {
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
