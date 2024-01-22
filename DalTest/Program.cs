using Dal;
using DalApi;
using DalTest;
using DO;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;


/// <summary>
/// Temporary for testing the Data-layer
/// </summary>
internal class Program
{

    //static readonly IDal s_dal = new DalList(); //stage 2
    static readonly IDal s_dal = new DalXml(); //stage 3

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
        Console.WriteLine("Please select 0-5 to proceed.");
        Console.WriteLine("0 - Exit.");
        Console.WriteLine("1 - Engineer Options.");
        Console.WriteLine("2 - Dependency Options.");
        Console.WriteLine("3 - Task Options.");
        Console.WriteLine("4 - Initiate the Data-base with random values.");
        Console.WriteLine("5 - Reset Data-Base.");

        
        //using tryParse
        if (!int.TryParse(Console.ReadLine(), out choice))
            throw new DalInvalidInputException("Invalid Input.");
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
            case 4:
                resetAndInitialize();
                break;
            case 5:
                reset();
                break;
        }

    }

    /// <summary>
    /// Resets the data-base
    /// </summary>
    private static void reset()
    {
        int answer;
        Console.Write("All current data will be deleted. Are you sure you want to proceed? (press 0 to go back, 1 to proceed)");
        //using tryParse
        if (!int.TryParse(Console.ReadLine(), out answer))
            throw new DalInvalidInputException("Invalid Input.");
        if (answer == 1)
        {
            s_dal.Reset();
            Console.WriteLine("Reset Successful.");
        }
    }

    /// <summary>
    /// Resets the data-base and initialize with random values
    /// </summary>
    private static void resetAndInitialize()
    {
        int answer;
        Console.Write("All current data will be deleted. Are you sure you want to proceed? (press 0 to go back, 1 to proceed)");
        if (!int.TryParse(Console.ReadLine(), out answer))
            throw new DalInvalidInputException("Invalid Input.");
        if (answer == 1)
        {
            s_dal.Reset();
            Initialization.Do(s_dal);
            Console.WriteLine("Data-Base initiated successfuly!");
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
            throw new DalInvalidInputException("Invalid Input. Please select 0-5.");

        switch (choice)
        {
            case _CRUDoptions.Back: //back to main menu
                return;

            case _CRUDoptions.Creat://adding a new Dependency
                myDependency = creatNewDependency(0);
                s_dal.Dependency.Create(myDependency);
                Console.WriteLine("Dependency added successfuly.");
                break;

            case _CRUDoptions.Delete: //delete Dependency 
                s_dal!.Dependency.Delete(readDependencyId());
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
                s_dal.Dependency.Update(myDependency);
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
            throw new DalInvalidInputException("Invalid Input. Please select 0-5.");
        switch (choice)
        {
            case _CRUDoptions.Back: //back to main menu
                return;

            case _CRUDoptions.Creat://adding a new Engineer
                Id = readEngineerId();
                myEngineer = creatNewEngineer(Id);
                s_dal.Engineer.Create(myEngineer);
                Console.WriteLine("Engineer added successfuly.");
                break;

            case _CRUDoptions.Delete: //delete engineer 
                s_dal!.Engineer.Delete(readEngineerId());
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
                s_dal!.Engineer.Update(myEngineer);
                Console.WriteLine("Engineer updated successfuly.");
                break;

        }

    }

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
        Console.WriteLine("4 - Print all Tasks in the system.");
        Console.WriteLine("5 - Update an existing Task.");
        _CRUDoptions choice;
        int Id;
        DO.Task? myTask;
        if (!_CRUDoptions.TryParse(Console.ReadLine(), out choice))
            throw new DalInvalidInputException("Invalid Input. Please select 0-5.");
        switch (choice)
        {
            case _CRUDoptions.Back: //back to main menu
                return;

            case _CRUDoptions.Creat://adding a new Task
                myTask = creatNewTask(0);
                s_dal.Task.Create(myTask);
                Console.WriteLine("Task added successfuly.");
                break;

            case _CRUDoptions.Delete: //delete Task 
                Id = readTaskId();
                s_dal.Task.Delete(Id);
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
                s_dal.Task.Update(myTask);
                Console.WriteLine("Task updated successfuly.");
                break;

        }
    }

    //-------------------Task Interface methods-------------------

    /// <summary>
    /// Prints all the Tasks
    /// </summary>
    private static void printAllTasks()
    {
        List<DO.Task> myTasks = new List<DO.Task> (s_dal!.Task.ReadAll()!);
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
        DO.Task? myTask = s_dal.Task.Read(myId);
        if (myTask != null)
        {
            Console.WriteLine(myTask);
            return myId;
        }
        else
            throw new DalDoesNotExistException($"Task with Id {myId} does not exist in the system.");
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
            throw new DalInvalidInputException("Invalid Input.");
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
            throw new DalInvalidInputException("Invalid Input.");

        //reads Task description 
        Console.WriteLine("Optional: Enter Task description in free words. leave blank to skip.");
        string? myDescription = Console.ReadLine();

        //reads the required work time of the Task
        Console.WriteLine("Enter the time required to complete the Task. (format: d.hh:mm:ss)");
        TimeSpan myTimeSpan;
        if (!TimeSpan.TryParse(Console.ReadLine(), out myTimeSpan))
            throw new DalInvalidInputException("Invalid Input.");


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
            throw new DalInvalidInputException("Invalid Input.");
        return myId;
    }

    /// <summary>
    /// Prints all the Engineers
    /// </summary>
    private static void printAllEngineers() 
    { 
        List<DO.Engineer> myEngineers = new List<DO.Engineer>(s_dal!.Engineer.ReadAll()!);
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
        Engineer? myEngineer = s_dal.Engineer.Read(myId);
        if (myEngineer != null)
        {
            Console.WriteLine(myEngineer);
            return myId;
        }
        else
            throw new DalDoesNotExistException($"Engineer with id {myId} does not exist in the system.");
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
            throw new DalInvalidInputException("Invalid Input.");

        //reads daily cost
        Console.WriteLine("Optional: enter the Engineer's daily cost. Enter zero to skip this step.");
        int myCost;
        if (!int.TryParse(Console.ReadLine(), out myCost))
            throw new DalInvalidInputException("Invalid Input.");

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
            throw new DalInvalidInputException("Invalid Input.");
        return myId;
    }

    /// <summary>
    /// Prints all the Dependencys
    /// </summary>
    private static void printAllDependencies()
    {
        List<DO.Dependency> myDependencies = new List <DO.Dependency> (s_dal.Dependency.ReadAll()!);
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
        Dependency? myDependency = s_dal.Dependency.Read(myId);
        if (myDependency != null)
        {
            Console.WriteLine(myDependency);
            return myId;
        }
        else
            throw new DalDoesNotExistException($"Dependency with id {myId} does not exist in the system.");
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
            throw new DalInvalidInputException("Invalid Input.");

        //reads Dependent task ID
        Console.WriteLine($"Task with ID {dependentId} depends on Task: (Enter different task ID).");
        if (!int.TryParse(Console.ReadLine(), out dependentOnId))
            throw new DalInvalidInputException("Invalid Input.");

        //returns the newly created Dependency with user values
        return new DO.Dependency(myId, dependentId, dependentOnId);
    }

}


