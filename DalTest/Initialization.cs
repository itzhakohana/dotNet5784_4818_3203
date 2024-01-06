namespace DalTest;
using DalApi;
using DO;


/// <summary>
/// Initializing the Data lists with initial mostly random values
/// </summary>
public static class Initialization
{
    private static ITask? s_dalTask; //stage 1
    private static IEngineer? s_dalEngineer; //stage 1
    private static IDependency? s_dalDependency; //stage 1

    private static readonly Random s_rand = new();

    
    /// <summary>
    /// initializes task-list with 20 uniqe tasks
    /// </summary>
    private static void creatTasks()
    {
        string[] taskTypes = 
            {
                "Debugging Code",
                "Database Optimization",
                "UI Refactoring",
                "Code Review",
                "Network Configuration",
                "Algorithm Enhancement",
                "Data Encryption",
                "User Authentication",
                "Software Testing",
                "API Integration",
                "System Diagnostics",
                "Frontend Styling",
                "Responsive Design",
                "User Interface Testing",
                "Database Schema Design",
                "Performance Tuning",
                "Version Control Cleanup",
                "Security Patching",
                "Mobile App Integration",
                "Workflow Automation",
                "Cloud Migration",
                "User Experience Audit",
                "API Documentation",
                "Load Testing",
                "Code Profiling",
                "Responsive Web Design",
                "Cross-Browser Testing",
                "Automated Testing Setup",
                "Code Refactoring",
                "System Logging Implementation",
                "Dependency Management",
                "Server Configuration",
                "Data Backup Implementation",
                "Accessibility Testing",
                "Automated Deployment",
                "Continuous Integration Setup",
                "Scalability Assessment",
                "Security Audit",
                "Code Coverage Analysis",
                "Error Handling Optimization",
            }; //random task to choose from
        int[] usedTasks = new int[taskTypes.Length]; // keeps track of which tasks are used
        
        for (int i = 0; i < 20; i++) //initialize 20 tasks
        {
            //randomizing task
            int randIndex;
            do
            {
                randIndex = s_rand.Next(taskTypes.Length);
            } while (usedTasks[randIndex] == 1);
            usedTasks[randIndex] = 1;
            string randAlias = taskTypes[randIndex];

            //randomizing complexity
            int myComlexity = s_rand.Next(0, 5);
            DO.EngineerExperience randLevel = (DO.EngineerExperience)myComlexity;

            //randomizing date of creation
            DateTime start = new DateTime(2010, 1, 1);
            DateTime randDate = start.AddDays(s_rand.Next(3000));

            //creating a new task and adding to the list
            Task newTask = new Task(0, randAlias, "", randDate, null, false, randLevel);
            s_dalTask.Create(newTask);
        }
    }



    /// <summary>
    /// Calls the "creat" methods which initialize the data-lists
    /// </summary>
    /// <param name="dalDependency"></param>
    /// <param name="dalEngineer"></param>
    /// <param name="dalTask"></param>
    /// <exception cref="NullReferenceException"></exception>
    public static void DO(IDependency dalDependency, IEngineer dalEngineer, ITask dalTask) 
    {
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        creatTasks();
        //creatEngineers();
        //creatDependencies();
    }


}
