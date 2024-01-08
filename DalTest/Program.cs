using Dal;
using DalApi;
using DalTest;


/// <summary>
/// Temporary for testing the data-layer
/// </summary>
internal class Program
{
    private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
    private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
    private static ITask? s_dalTask = new TaskImplementation(); //stage 1

    private static void Main(string[] args)
    {
        try
        {
            Initialization.DO(s_dalDependency!, s_dalEngineer!, s_dalTask!);

            List<DO.Engineer> myEngineers = s_dalEngineer.ReadAll();
            foreach (var item in myEngineers)
            {
                Console.WriteLine(item);
            }

            List<DO.Task> myTasks = s_dalTask.ReadAll();
            foreach (var item in myTasks)
            {
                Console.WriteLine(item);
            }

            List<DO.Dependency> myDependencies = s_dalDependency.ReadAll();
            foreach (var item in myDependencies)
            {
                Console.WriteLine(item);
            }


        }


        catch (Exception ex)
        {
            // Catching the exception and printing custom message
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }

}