using Dal;
using DalApi;
using DalTest;
using DO;
using System.Threading.Tasks;


/// <summary>
/// Temporary for testing the Data-layer
/// </summary>
internal class Program
{
    private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
    private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
    private static ITask? s_dalTask = new TaskImplementation(); //stage 1

    /// <summary>
    /// Used for interface options
    /// </summary>
    private enum _CRUDoptions 
    { 
        Back, Creat, Delete, Read, ReadAll, Update
    }

    private static void Main(string[] args)
    {
        try
        {
            Initialization.DO(s_dalDependency!, s_dalEngineer!, s_dalTask!);
            int choice = 1;

            while (choice != 0)
            {
                try
                {
                    //Calling for the Main Interface
                    mainInterfaceOptions(ref choice);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
        }

        catch (Exception ex)
        {
            // Catching the exception and printing custom message
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }

    /// <summary>
    /// Main Interface for performing operation on the Data-Entities
    /// </summary>
    /// <param name="choice"></param>
    /// <exception cref="Exception"></exception>
    private static void mainInterfaceOptions(ref int choice)
    {
        Console.WriteLine("\nMain Menu:");
        Console.WriteLine("Please select 0-3 to proceed.");
        Console.WriteLine("0 - Exit.");
        Console.WriteLine("1 - Engineer Options.");
        Console.WriteLine("2 - Dependency Options.");
        Console.WriteLine("3 - Task Options.");
        //using tryParse
        if (!int.TryParse(Console.ReadLine(), out choice))
            throw new Exception("Invalid Input."); ;
        switch (choice)
        {
            case 0:
                break;
            case 1:
                engineerInterface();
                break;
            case 2:
                dependencyInterface();
                break;
            case 3:
                taskInterface();
                break;
        }

    }

    /// <summary>
    /// Dependency options
    /// </summary>
    private static void dependencyInterface()
    {
        Console.WriteLine("\nDependency Options:");
        Console.WriteLine("Please select 0-5 to proceed.");
        Console.WriteLine("0 - Back to main menu.");
        Console.WriteLine("1 - Form a new Dependency.");
        Console.WriteLine("2 - Delete an existing Dependency.");
        Console.WriteLine("3 - Find and print Dependency.");
        Console.WriteLine("4 - Print all Dependencies in the system.");
        Console.WriteLine("5 - Update an existing Dependency.");
        _CRUDoptions choice;
        int Id;
        Dependency? myDependency;

        //reading choice
        if (!_CRUDoptions.TryParse(Console.ReadLine(), out choice))
            throw new Exception("Invalid Input.");

        switch (choice)
        {
            case _CRUDoptions.Back: //back to main menu
                return;

            case _CRUDoptions.Creat://adding a new Dependency
                myDependency = creatNewDependency(0);
                s_dalDependency.Create(myDependency);
                Console.WriteLine("Dependency added successfuly.");
                break;

            case _CRUDoptions.Delete: //delete Dependency 
                s_dalDependency!.Delete(readDependencyId());
                Console.WriteLine("Dependency deleted successfuly.");
                break;

            case _CRUDoptions.Read: //find and print Dependency 
                findAndPrintDependency();
                break;

            case _CRUDoptions.ReadAll: //print all the Dependency in the system
                printAllDependencies();
                break;

            case _CRUDoptions.Update: //update Dependency
                Id = findAndPrintDependency();
                myDependency = creatNewDependency(Id);
                s_dalDependency.Update(myDependency);
                Console.WriteLine("Dependency updated successfuly.");
                break;
        }


    }

    /// <summary>
    /// Engineer options
    /// </summary>
    private static void engineerInterface()
    {
        Console.WriteLine("\nEngineer Options:");
        Console.WriteLine("Please select 0-5 to proceed.");
        Console.WriteLine("0 - Back to main menu.");
        Console.WriteLine("1 - Creat a new Engineer.");
        Console.WriteLine("2 - Delete an existing Engineer.");
        Console.WriteLine("3 - Find and print Engineer.");
        Console.WriteLine("4 - Print all Engineers in the system.");
        Console.WriteLine("5 - Update an existing Engineer.");
        _CRUDoptions choice;
        int Id;
        Engineer? myEngineer;
        if (!_CRUDoptions.TryParse(Console.ReadLine(), out choice))
            throw new Exception("Invalid Input.");
        switch (choice)
        {
            case _CRUDoptions.Back: //back to main menu
                return;

            case _CRUDoptions.Creat://adding a new Engineer
                Id = readEngineerId();
                myEngineer = creatNewEngineer(Id);
                s_dalEngineer.Create(myEngineer);
                Console.WriteLine("Engineer added successfuly.");
                break;

            case _CRUDoptions.Delete: //delete engineer 
                s_dalEngineer!.Delete(readEngineerId());
                Console.WriteLine("Engineer deleted successfuly.");
                break;

            case _CRUDoptions.Read: //find and print engineer 
                findAndPrintEngineer();
                break;

            case _CRUDoptions.ReadAll: //print all the Engineers in the system
                printAllEngineers();
                break;

            case _CRUDoptions.Update: //update engineer
                Id = findAndPrintEngineer();
                myEngineer = creatNewEngineer(Id);
                s_dalEngineer!.Update(myEngineer);
                Console.WriteLine("Engineer updated successfuly.");
                break;

        }

    }


    //-------------------Task Interface methods-------------------

    /// <summary>
    /// Task options
    /// </summary>
    private static void taskInterface()
    {
        Console.WriteLine("\nTask Options:");
        Console.WriteLine("Please select 0-5 to proceed.");
        Console.WriteLine("0 - Back to main menu.");
        Console.WriteLine("1 - Creat a new Task.");
        Console.WriteLine("2 - Delete an existing Task.");
        Console.WriteLine("3 - Find and print Task.");
        Console.WriteLine("4 - Print all Task in the system.");
        Console.WriteLine("5 - Update an existing Task.");
        _CRUDoptions choice;
        int Id;
        DO.Task? myTask;
        if (!_CRUDoptions.TryParse(Console.ReadLine(), out choice))
            throw new Exception("Invalid Input.");
        switch (choice)
        {
            case _CRUDoptions.Back: //back to main menu
                return;

            case _CRUDoptions.Creat://adding a new Task
                myTask = creatNewTask(0);
                s_dalTask.Create(myTask);
                Console.WriteLine("Task added successfuly.");
                break;

            case _CRUDoptions.Delete: //delete Task 
                Id = readTaskId();
                s_dalTask.Delete(Id);
                Console.WriteLine("Task deleted successfuly.");
                break;

            case _CRUDoptions.Read: //find and print Task 
                findAndPrintTask();
                break;

            case _CRUDoptions.ReadAll: //print all the Tasks in the system
                printAllTasks();
                break;

            case _CRUDoptions.Update: //update Task
                Id = findAndPrintTask();
                myTask = creatNewTask(Id);
                s_dalTask.Update(myTask);
                Console.WriteLine("Task updated successfuly.");
                break;

        }
    }

    /// <summary>
    /// Prints all the Tasks
    /// </summary>
    private static void printAllTasks()
    {
        List<DO.Task> myTasks = s_dalTask!.ReadAll();
        foreach (var myTask in myTasks)
        {
            Console.WriteLine(myTask);
        }
    }

    /// <summary>
    /// Search and print Task by ID
    /// </summary>
    private static int findAndPrintTask()
    {
        int myId = readTaskId();
        DO.Task? myTask = s_dalTask.Read(myId);
        if (myTask != null)
        {
            Console.WriteLine(myTask);
            return myId;
        }
        else
            throw new Exception($"Task with id {myId} does not exist in the system.");
    }

    /// <summary>
    /// Reads Task ID from the user
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static int readTaskId()
    {
        Console.WriteLine("Please enter Task ID numer.");
        int myId;
        //using tryParse
        if (!int.TryParse(Console.ReadLine(), out myId))
            throw new Exception("Invalid Input.");
        return myId;
    }

    /// <summary>
    /// Returns a new Task from user input
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static DO.Task creatNewTask(int myId)
    {
        //reads Task name
        Console.WriteLine("Please enter Task name. (Alias)");
        string? myName = Console.ReadLine();

        //reads Task comlexity
        Console.WriteLine("Please choose the Task's comlexity. (choose 1-5)");
        Console.WriteLine("1 - Beginner\n2 - Advanced Beginner\n3 - Intermediate\n4 - Advanced\n5 - Expert");
        int myLevel;
        if (!int.TryParse(Console.ReadLine(), out myLevel))
            throw new Exception("Invalid Input.");

        //reads Task description 
        Console.WriteLine("Optional: Enter Task description in free words. leave blank to skip.");
        string? myDescription = Console.ReadLine();

        //reads the required work time of the Task
        Console.WriteLine("Enter the time required to complete the Task in days. (format: d.hh:mm:ss)");
        TimeSpan myTimeSpan;
        if (!TimeSpan.TryParse(Console.ReadLine(), out myTimeSpan))
            throw new Exception("Invalid Input.");


        //returns the newly created Task with user values
        return new DO.Task(myId, myName, myDescription, DateTime.Now, myTimeSpan, (EngineerExperience)myLevel);
    }


    //-------------------Engineer Interface methods-------------------

    /// <summary>
    /// Reads engineer ID from the user
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static int readEngineerId()
    {
        Console.WriteLine("Please enter Engineer ID numer.");
        int myId;
        //using tryParse
        if (!int.TryParse(Console.ReadLine(), out myId))
            throw new Exception("Invalid Input.");
        return myId;
    }

    /// <summary>
    /// Prints all the Engineers
    /// </summary>
    private static void printAllEngineers() 
    { 
        List<DO.Engineer> myEngineers = s_dalEngineer!.ReadAll();
        foreach(var myEngineer in myEngineers) 
        {
            Console.WriteLine(myEngineer);
        }
    }

    /// <summary>
    /// Search and print Engineer by ID
    /// </summary>
    private static int findAndPrintEngineer()
    {
        int myId = readEngineerId();
        Engineer? myEngineer = s_dalEngineer.Read(myId);
        if (myEngineer != null)
        {
            Console.WriteLine(myEngineer);
            return myId;
        }
        else
            throw new Exception($"Engineer with id {myId} does not exist in the system.");
    }

    /// <summary>
    /// Creats a new engineer from user's input
    /// </summary>
    private static Engineer creatNewEngineer(int myId)
    {
        //reads engineer name
        Console.WriteLine("Please enter Engineer name.");
        string ?myName = Console.ReadLine();

        //reads engineer email address
        Console.WriteLine("Please enter the Engineer's Email address.(in the form: xxxxx@gmail.com)");
        string? myEmail = Console.ReadLine();

        //reads engineer level
        Console.WriteLine(@"Please choose the Engineer's expertise level. (choose 1-5)");
        Console.WriteLine("1 - Beginner\n2 - Advanced Beginner\n3 - Intermediate\n4 - Advanced\n5 - Expert");
        int myLevel;
        if (!int.TryParse(Console.ReadLine(), out myLevel))
            throw new Exception("Invalid Input.");

        //reads daily cost
        Console.WriteLine("Optional: enter the Engineer's daily cost. Enter zero to skip this step.");
        int myCost;
        if (!int.TryParse(Console.ReadLine(), out myCost))
            throw new Exception("Invalid Input.");

        //returns the newly created Engineer with user values
        return new DO.Engineer(myId, (EngineerExperience)myLevel, myName!, myEmail!, myCost);        
    }


    //-------------------Dependency Interface methods-------------------

    /// <summary>
    /// Reads Dependency ID from the user
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static int readDependencyId()
    {
        Console.WriteLine("Please enter Dependency ID numer.");
        int myId;
        if (!int.TryParse(Console.ReadLine(), out myId))
            throw new Exception("Invalid Input.");
        return myId;
    }

    /// <summary>
    /// Prints all the Dependencys
    /// </summary>
    private static void printAllDependencies()
    {
        List<DO.Dependency> myDependencies = s_dalDependency!.ReadAll();
        foreach (var myDependency in myDependencies)
        {
            Console.WriteLine(myDependency);
        }
    }

    /// <summary>
    /// Search and print Dependency by ID
    /// </summary>
    private static int findAndPrintDependency()
    {
        int myId = readDependencyId();
        Dependency? myDependency = s_dalDependency.Read(myId);
        if (myDependency != null)
        {
            Console.WriteLine(myDependency);
            return myId;
        }
        else
            throw new Exception($"Dependency with id {myId} does not exist in the system.");
    }

    /// <summary>
    /// Creats a new Dependency between two existing tasks
    /// </summary>
    private static Dependency creatNewDependency(int myId)
    {
        int dependentId, dependentOnId;
        //reads Dependent task ID
        Console.WriteLine("Please enter the Dependent-Task ID.");
        if (!int.TryParse(Console.ReadLine(), out dependentId))
            throw new Exception("Invalid Input.");

        //reads Dependent task ID
        Console.WriteLine($"Task with ID {dependentId} depends on Task: (Enter different task ID).");
        if (!int.TryParse(Console.ReadLine(), out dependentOnId))
            throw new Exception("Invalid Input.");

        //returns the newly created Dependency with user values
        return new DO.Dependency(myId, dependentId, dependentOnId);
    }

}


