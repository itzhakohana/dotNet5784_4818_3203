namespace DalTest;
using DalApi;
using DO;
using System.ComponentModel;
using System.Runtime.Intrinsics.X86;


/// <summary>
/// Initializing the Data lists with random values
/// </summary>
public static class Initialization
{
    private static IDal? s_dal; //stage 2

    private static readonly Random s_rand = new();

    private static readonly int s_tasksAmount = 20; //amount of Tasks created randomly 
    private static readonly int s_dependenciesAmount = 40; //amount of Dependencies created randomly 
    private static readonly int s_engineersAmount = 10;  //amount of Engineers created randomly 
    private static readonly int MIN_ID = 200000000;
    private static readonly int MAX_ID = 400000000;

    /// <summary>
    /// Initializes predecided amount of uniqe Engineers
    /// </summary>
    private static void creatEngineers()
    {
        string[] firstNames =
        {
            "Ethan",
            "Ava",
            "Liam",
            "Mia",
            "Noah",
            "Olivia",
            "Lucas",
            "Sophia",
            "Jackson",
            "Emma",
            "Aiden",
            "Isabella",
            "Mason",
            "Harper",
            "Benjamin",
            "Amelia",
            "Elijah",
            "Abigail",
            "Logan",
            "Evelyn",
        }; // array of first names
        string[] surNames = 
        {
            "Smith",
            "Johnson",
            "Williams",
            "Jones",
            "Brown",
            "Davis",
            "Miller",
            "Wilson",
            "Moore",
            "Taylor",
            "Anderson",
            "Thomas",
            "Jackson",
            "White",
            "Harris",
            "Martinez",
            "Nelson",
            "Carter",
            "Cooper",
            "Stewart",
        };   //array of surnames
        int emailFiller = 1000;

        for (int i = 0; i < s_engineersAmount; i++) //randomizing 10 Engineers
        {
            //Randomly assembles a name from the two arrays
            int firstNameIndex = s_rand.Next(firstNames.Length);
            int surNameIndex = s_rand.Next(surNames.Length);
            string randName = (firstNames[firstNameIndex] + " " + surNames[surNameIndex]);

            //setting email address based on the randomized name
            string randEmail = (surNames[surNameIndex] + (s_rand.Next(emailFiller, emailFiller += 10)) + "@gmail.com");

            //randomizing ID number (range: 200000000 to 400000000)
            int randId = s_rand.Next(MIN_ID, MAX_ID);

            //randomizing expertise level
            int myComlexity = s_rand.Next(1, 6);
            DO.EngineerExperience randLevel = (DO.EngineerExperience)myComlexity;

            //randomizing phone number
            string randPhone = ("050" + s_rand.Next(1000000, 9999999).ToString());

            //creating and adding a new Engineer to the database
            Engineer myEngineer = new Engineer(randId, randLevel, randName, randEmail, randPhone, s_rand.Next(10,200));
            s_dal!.Engineer.Create(myEngineer);

        }
    }

    /// <summary>
    /// Initializes predecided amount of unique Dependencies
    /// </summary>
    private static void creatDependencies() 
    {
        List<Task?> listCopy = (List<Task?>)s_dal!.Task.ReadAll().ToList();
        int quarter = listCopy.Count / 4;
        int half = listCopy.Count / 2;
        for (int i = 0; i < (s_dependenciesAmount / 4); i++) //randomizing 40 Dependencies
        { 
            //randomly picking a dependent-ON-task from the task-list
            int randListIndex = s_rand.Next(0, quarter);
            Task depOnTask = listCopy.ElementAt(randListIndex)!;
            int depOnTaskId = depOnTask.Id;

            //randomly picking a dependent-task from the task-list
            randListIndex = s_rand.Next(quarter, half);
            Task depTask = listCopy.ElementAt(randListIndex)!;
            int depTaskId = depTask.Id;

            //creating and adding a new Dependency to the database
            Dependency myDependency = new Dependency(0, depTaskId, depOnTaskId);
            s_dal!.Dependency.Create(myDependency);
        }
        for (int i = s_dependenciesAmount / 4; i < (2 * (s_dependenciesAmount / 4)); i++) 
        {

            //randomly picking a dependent-ON-task from the task-list
            int randListIndex = s_rand.Next(quarter, half);
            Task depOnTask = listCopy.ElementAt(randListIndex)!;
            int depOnTaskId = depOnTask.Id;

            //randomly picking a dependent-task from the task-list
            randListIndex = s_rand.Next(half, 3 * quarter);
            Task depTask = listCopy.ElementAt(randListIndex)!;
            int depTaskId = depTask.Id;

            //creating and adding a new Dependency to the database
            Dependency myDependency = new Dependency(0, depTaskId, depOnTaskId);
            s_dal!.Dependency.Create(myDependency);

        }
        for (int i = s_dependenciesAmount / 2; i < s_dependenciesAmount; i++)
        {
            //randomly picking a dependent-ON-task from the task-list
            int randListIndex = s_rand.Next(half, half + quarter);
            Task depOnTask = listCopy.ElementAt(randListIndex)!;
            int depOnTaskId = depOnTask.Id;

            //randomly picking a dependent-task from the task-list
            randListIndex = s_rand.Next(half + quarter, listCopy.Count);
            Task depTask = listCopy.ElementAt(randListIndex)!;
            int depTaskId = depTask.Id;

            //creating and adding a new Dependency to the database
            Dependency myDependency = new Dependency(0, depTaskId, depOnTaskId);
            s_dal!.Dependency.Create(myDependency);

        }
    }

    /// <summary>
    /// Initializes predecided amount of unique tasks
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
        
        for (int i = 0; i < s_tasksAmount; i++) //initialize 20 tasks
        {
            //randomizing task-Alias
            int randIndex;
            do
            {
                randIndex = s_rand.Next(taskTypes.Length);
            } while (usedTasks[randIndex] == 1);
            usedTasks[randIndex] = 1;
            string randAlias = taskTypes[randIndex];

            //randomizing complexity
            int myComlexity = s_rand.Next(1, 6);
            DO.EngineerExperience randLevel = (DO.EngineerExperience)myComlexity;

            //randomizing date of creation
            DateTime start = new DateTime(2023, 1, 1);
            DateTime randDate = start.AddDays(s_rand.Next(300));

            //creating and adding a new task to the database
            Task newTask = new Task(0, randAlias, "", randDate, new TimeSpan(s_rand.Next(1,15), s_rand.Next(24),0, 0), randLevel);
            s_dal!.Task.Create(newTask);
        }
    }

    /// <summary>
    /// Calls the "creat" methods which initialize the database
    /// </summary>
    public static void Do() 
    {
        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2;
        s_dal = DalApi.Factory.Get; //stage 4
        creatTasks();
        creatEngineers();
        creatDependencies();
    }

}
