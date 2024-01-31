using BO;
using DO;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private static void Main(string[] args)
    {
        do
        {

            Console.WriteLine("Please choose 0-5");
            Console.WriteLine("0 - exit");
            Console.WriteLine("1 - Engineer Options");
            Console.WriteLine("2 - Task Options");
            Console.WriteLine("3 - Milestone Options(NOT DONE!)");
            Console.WriteLine("4 - Initiate the Data-base with random values");
        
        
            try
            {
                if (!int.TryParse(Console.ReadLine(), out int choice))
                    throw new BO.BlInvalidUserInputException("Wrong input. please enter 0 - 4");
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        engineerOptions();
                        break;
                    case 2:
                        taskOptions();
                        break;
                    case 3:
                        milestoneOptions();
                        break;
                    case 4:
                        DalTest.Initialization.Do();
                        Console.WriteLine("Data base initialized with random values");
                        break;
                    default:
                        throw new BO.BlInvalidUserInputException("Wrong input. please enter 0 - 4");

                }
            }
            catch (BlInvalidUserInputException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Undefined Errorr: " + ex.Message);
            }

        } while (true);
    }

    #region options menus
    private static void engineerOptions()
    {
        do
        {

            Console.WriteLine("Engineer Options:");
            Console.WriteLine("Please choose 0-6");
            Console.WriteLine("0 - Back to Main Menu");
            Console.WriteLine("1 - Add Engineer");
            Console.WriteLine("2 - Delete Engineer");
            Console.WriteLine("3 - Update Engineer");
            Console.WriteLine("4 - Print all the Engineers in the company");
            Console.WriteLine("5 - Print Engineer's assigned Task");
            Console.WriteLine("6 - Print Engineer's assigned Milestone(NOT DONE!)");

       
            try
            {
                if (!int.TryParse(Console.ReadLine(), out int choice))
                    throw new BO.BlInvalidUserInputException("Wrong input. please enter 0 - 6");
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        addEngineer();
                        break;
                    case 2:
                        deleteEngineer();
                        break;
                    case 3:
                        s_bl.Engineer.Update(readEngineer());
                        break;
                    case 4:
                        printEngineers();
                        break;
                    case 5:
                        printAssignedTask();
                        break;
                    case 6:
                        break;
                    default: 
                        throw new BO.BlInvalidUserInputException("Wrong input. please enter 0 - 6");

                }
            }
            catch (BlInvalidUserInputException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Undefined Errorr: " + ex.Message);
            }

        } while (true);

    }

    private static void taskOptions()
    {

        do
        {
            Console.WriteLine("Task Options:");
            Console.WriteLine("Please choose 0-5");
            Console.WriteLine("0 - Back to Main Menu");
            Console.WriteLine("1 - Add Task");
            Console.WriteLine("2 - Delete Task");
            Console.WriteLine("3 - Assign Task to Engineer");
            Console.WriteLine("4 - Update existing Task");
            Console.WriteLine("5 - Print all Tasks");

            try
            {
                if (!int.TryParse(Console.ReadLine(), out int choice))
                    throw new BO.BlInvalidUserInputException("Wrong input. please enter 0 - 5");
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    default:
                        throw new BO.BlInvalidUserInputException("Wrong input. please enter 0 - 5");

                }
            }
            catch (BlInvalidUserInputException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Undefined Errorr: " + ex.Message);
            }

        } while (true);
    }

    private static void milestoneOptions()
    { }
    #endregion

    #region engineer functions

    /// <summary>
    /// Deletes an engineer by ID
    /// </summary>
    private static void deleteEngineer()
    {
        int id = readEngineerId();
        s_bl.Engineer.Delete(id);
    }

    /// <summary>
    /// Returns BO.Engineer initialized from user input
    /// </summary>
    /// <returns>BO.Engineer type with user given values</returns>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    private static BO.Engineer readEngineer()
    {
        double cost;
        int level, taskId, id = readEngineerId();
        //read name
        Console.WriteLine("Enter Engineer Name");
        string name = Console.ReadLine()!;
        //read email
        Console.WriteLine("Enter Engineer Email address");
        string email = Console.ReadLine()!;
        //read level
        Console.WriteLine(@"Please choose the Engineer's expertise level. (choose 1-5)");
        Console.WriteLine("1 - Beginner\n2 - Advanced Beginner\n3 - Intermediate\n4 - Advanced\n5 - Expert");
        while (true)
        {
            try
            {
                if (!int.TryParse(Console.ReadLine(), out level))
                    throw new BO.BlInvalidUserInputException("Invalid Input. Please enter 1-5");
                break;
            }
            catch (BlInvalidUserInputException ex)
            {
                Console.WriteLine(ex.Message); ;
            }

        }
        //read cost
        Console.WriteLine("Enter Engineer total cost per-hour. can be decimal");
        while (true)
        {
            try
            {
                if (!double.TryParse(Console.ReadLine(), out cost))
                    throw new BO.BlInvalidUserInputException("Invalid Input. Please enter a number");
                break;
            }
            catch (BlInvalidUserInputException ex)
            {
                Console.WriteLine(ex.Message); ;
            }

        }
        //reads assingned task
        Console.WriteLine("Optional: Assign a task to this Engineer(input task ID). press 0 to skip this step");
        while (true)
        {
            try
            {
                if (!int.TryParse(Console.ReadLine(), out taskId))
                    throw new BO.BlInvalidUserInputException("Invalid Input. Please enter an Integer");
                break;
            }
            catch (BlInvalidUserInputException ex)
            {
                Console.WriteLine(ex.Message); ;
            }

        }
        return new BO.Engineer()
        {
            Id = id,
            Name = name,
            Email = email,
            Cost = cost,
            Level = (BO.EngineerExperience)level,
            Task = s_bl.Task.ReadTaskInEngineer(taskId)
        };
    }

    /// <summary>
    /// Reads ID number from the user. includes input validation
    /// </summary>
    /// <returns>User given engineer ID number</returns>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    private static int readEngineerId()
    {
        //read Id
        int id;
        Console.WriteLine("Enter Engineer ID number (must be a postive number)");
        while (true)
        {
            try
            {
                if (!int.TryParse(Console.ReadLine(), out id))
                    throw new BO.BlInvalidUserInputException("Invalid Input. Please enter an Integer");
                break;
            }
            catch (BlInvalidUserInputException ex)
            {
                Console.WriteLine(ex.Message); ;
            }

        }
        return id;
    }

    /// <summary>
    /// Adds a new engineer to the data base
    /// </summary>
    private static void addEngineer()
    {
        s_bl.Engineer.Add(readEngineer());
    }

    /// <summary>
    /// Print all the engineers in the Data-base
    /// </summary>
    private static void printEngineers() 
    { 
        foreach(var eng in s_bl.Engineer.ReadAll())
            Console.WriteLine(eng);
    }

    /// <summary>
    /// Prints the assigned task of the specified engineer
    /// </summary>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    private static void printAssignedTask()
    {
        int id = readEngineerId();
        BO.Engineer? eng = s_bl.Engineer.Read(id);
        if (eng is not null)
        {
            if (eng.Task is not null)
                Console.WriteLine(eng.Task);
            else
                throw new BO.BlDoesNotExistException($"Engineer with ID {id} is not assigned to any task");
            return;
        }
        throw new BO.BlDoesNotExistException($"Engineer with ID {id} does not exist");

    }

    #endregion
}


