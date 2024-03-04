using BO;
using DO;
using System.Reflection.Emit;
using System.Text;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private static readonly DateTime s_projectStartDate = new();
    private static readonly DateTime s_projectEndDate = new();
    private static bool s_projectStarted = s_bl.Task.ProjectHasStarted();
    private static DateTime CurrentTime = new DateTime(2024,01,01);//random date just for testing

    private static void Main(string[] args)
    {
        do
        {
            Console.WriteLine($"*****************{(s_projectStarted ? "Project has started" : "Project has NOT yet started")}****************");
            Console.WriteLine("Current Time: {0}", CurrentTime);
            Console.WriteLine("Main Menu Options:");
            Console.WriteLine("Please choose 0-5");
            Console.WriteLine(" 0 - Exit");
            Console.WriteLine(" 1 - Enter as Admin");
            Console.WriteLine(" 2 - Enter as Engineer");
            Console.WriteLine(" 3 - Reset the data-base (Delete all current data)");
            Console.WriteLine(" 4 - Initiate the Data-base with random Data (Creat Engineers, Tasks, Dependencies with mostly random values)");
            Console.WriteLine(" 5 - Increment current time");
            Console.WriteLine("********************************");

            //BO.User user =  new BO.User();
            //user.Id = 208562159;
            ////user.Id = 2085621;
            //user.UserName = "Test";
            //user.Password = 1234;
            //user.UserType = BO.UserType.Engineer;
            //user.CompletedTasks = 0;
            //user.PastTasks = null;
            //user.CurrentTask = null;
            //user.Level = BO.EngineerExperience.Expert;
            ////s_bl.User.Add(user);
            //try
            //{
            //    var u = s_bl.User.Read(208562159);
            //    s_bl.User.LogIn(u.Id);
            //    if (u.CurrentTask != null)                    
            //        Console.WriteLine(u.CurrentTask);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}            


            try
            {
                if (!int.TryParse(Console.ReadLine(), out int choice))
                    throw new BO.BlInvalidUserInputException("");
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        adminOptionsMenu();
                        break;
                    case 2:
                        engineerOptionsMenu();
                        break;
                    case 3:
                        resetDataBase();
                        break;
                    case 4:
                        resetDataBase();
                        DalTest.Initialization.Do();
                        Console.WriteLine("Data base initialized with random values");
                        break;
                    case 5:
                        Console.WriteLine("Current Time: {0}", CurrentTime);
                        Console.WriteLine("Enter the amount of Days you want to add");
                        if (!int.TryParse(Console.ReadLine(), out int t))
                            Console.WriteLine("Invalid input");
                        else
                            CurrentTime += new TimeSpan(t, 0, 0, 0);
                        break;
                    default:
                        throw new BO.BlInvalidUserInputException("");

                }
            }
            catch (BlInvalidUserInputException)
            {
                Console.WriteLine("Wrong input. please enter 0 - 5");
            }
            catch (BlLogicViolationException ex)
            {
                Console.WriteLine (ex.Message);
            }
        } while (true);
    }
    /// <summary>
    /// Resets the data base
    /// </summary>
    private static void resetDataBase()
    {
        do
        {
            try
            {
                Console.WriteLine("All current data will e deleted, are you sure you want to proceed? \n1 - yes\n2 - no");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                    throw new BO.BlInvalidUserInputException("Invalid input. enter 1 - 2");
                switch (choice)
                {
                    case 1:
                        s_bl.Reset();
                        s_projectStarted = false;
                        Console.WriteLine("Successfuly deleted all data");
                        return;
                    case 2:
                        throw new BO.BlLogicViolationException("Reset operation canceled");                        
                    default:
                        throw new BO.BlInvalidUserInputException("Invalid input. enter 1 - 2");
                }                
            }
            catch (BO.BlInvalidUserInputException ex)
            {
                Console.WriteLine(ex.Message);
            }
        } while (true);
    }

    #region Menus
    /// <summary>
    /// Available opions for Engineer in the project
    /// </summary>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    private static void engineerOptionsMenu()
    {
        if (s_projectStarted)
            do
            {
                Console.WriteLine("****************Project has started****************");
                Console.WriteLine("Current Time: {0}", CurrentTime);
                Console.WriteLine("You are Engineer in the project");
                Console.WriteLine("Please choose 0-2");
                Console.WriteLine(" 0 - Back to previous menu");
                Console.WriteLine(" 1 - Watch your current assigned task");
                Console.WriteLine(" 2 - Update progress on you assigned task");
                Console.WriteLine(" 3 - Watch list of available tasks");
                Console.WriteLine(" 4 - Pick a task to work on");
                Console.WriteLine("********************************");

                try
                {
                    if (!int.TryParse(Console.ReadLine(), out int choice))
                        throw new BO.BlInvalidUserInputException();
                    switch (choice)
                    {
                        case 0:
                            return;
                        case 1:
                            printEngineerAssignedTask();
                            break;
                        case 2:
                            updateProgressOnTask();
                            Console.WriteLine("Successfuly updated progress");
                            break;
                        case 3:
                            printAvailableTasks();
                            break;
                        case 4:
                            s_bl.Task.UpdateAssignedEngineerAndStartWork(readTaskId(), readEngineerId(), CurrentTime);
                            Console.WriteLine("Successfuly assigned task to engineer");
                            break;
                        default:
                            throw new BO.BlInvalidUserInputException();

                    }
                }
                catch (BlInvalidUserInputException)
                {
                    Console.WriteLine("Wrong input. please enter 0 - 2");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (true);
        else
        {
            Console.WriteLine("Project has not yet started. \ngoing back to previous menu...");
            Thread.Sleep(2000);
        }
    }
    /// <summary>
    /// Available opions for the project manager
    /// </summary>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    private static void adminOptionsMenu()
    {
        if (s_projectStarted)
            do
            {
                Console.WriteLine("****************Project has started****************");
                Console.WriteLine("Current Time: {0}", CurrentTime);
                Console.WriteLine("You are the project's manager");
                Console.WriteLine("Please choose 0-4");
                Console.WriteLine(" 0 - Back to previous menu");
                Console.WriteLine(" 1 - Engineer Options");
                Console.WriteLine(" 2 - Task Options");
                Console.WriteLine(" 3 - Milestone Options");
                Console.WriteLine(" 4 - Reset the data-base");
                Console.WriteLine("********************************");


                try
                {
                    if (!int.TryParse(Console.ReadLine(), out int choice))
                        throw new BO.BlInvalidUserInputException();
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
                            s_bl.Task.Reset();
                            s_bl.Engineer.Reset();
                            Console.WriteLine("Successfuly deleted all data");
                            s_projectStarted = false;
                            return;  
                        default:
                            throw new BO.BlInvalidUserInputException();

                    }
                }
                catch (BlInvalidUserInputException)
                {
                    Console.WriteLine("Wrong input. please enter 0 - 4");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }

            } while (true);

        else
            do
            {
                Console.WriteLine("****************Project has NOT yet started****************");
                Console.WriteLine("Current Time: {0}", CurrentTime);
                Console.WriteLine("You are the project's manager");
                Console.WriteLine("Please choose 0-5");
                Console.WriteLine(" 0 - Back to previous menu");
                Console.WriteLine(" 1 - Engineer Options");
                Console.WriteLine(" 2 - Task Options");
                Console.WriteLine(" 3 - Milestone Options (Only available after project starts)");
                Console.WriteLine(" 4 - Start Project (Set the project's starting & ending dates. schedule will authomaticaly be built for the project)");
                
                Console.WriteLine("********************************");
                try
                {
                    if (!int.TryParse(Console.ReadLine(), out int choice))
                        throw new BO.BlInvalidUserInputException();
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
                            Console.WriteLine("Milestone menu only available after project starts!");
                            Thread.Sleep(2000);
                            break;
                        case 4:
                            startProject();
                            //assignEngineerToTask(readTaskId(), readEngineerId());
                            return;
                        default:
                            throw new BO.BlInvalidUserInputException();

                    }
                }
                catch (BlInvalidUserInputException)
                {
                    Console.WriteLine("Wrong input. please enter 0 - 4");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (true);
    }
    /// <summary>
    /// Options fo engineer entity
    /// </summary>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    private static void engineerOptions()
    {
        do
        {
            Console.WriteLine("************************************");
            Console.WriteLine("Engineer Options:");
            Console.WriteLine("Please choose 0-6");
            Console.WriteLine(" 0 - Back to previous Menu");
            Console.WriteLine(" 1 - Add Engineer");
            Console.WriteLine(" 2 - Delete Engineer");
            Console.WriteLine(" 3 - Update Engineer");
            Console.WriteLine(" 4 - Print all the Engineers in the company");
            Console.WriteLine(" 5 - Print Engineer's assigned Task");
            Console.WriteLine("************************************");

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
                        updateEngineer();
                        break;
                    case 4:
                        printEngineers();
                        break;
                    case 5:
                        printEngineerAssignedTask();
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
                Console.WriteLine(ex.Message);
            }

        } while (true);

    }
    /// <summary>
    /// Options fo Task entity
    /// </summary>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    private static void taskOptions()
    {
        if (s_projectStarted)
            do
            {
                Console.WriteLine("******************Project has started******************");
                Console.WriteLine("Task Options:");
                Console.WriteLine("Please choose 0-6");
                Console.WriteLine(" 0 - Back to previous Menu");                
                Console.WriteLine(" 1 - Update existing Task");
                Console.WriteLine(" 2 - Assign Task to Engineer");
                Console.WriteLine(" 3 - Print all Tasks");
                Console.WriteLine("************************************");
                try
                {
                    if (!int.TryParse(Console.ReadLine(), out int choice))
                        throw new BO.BlInvalidUserInputException("Wrong input. please enter 0 - 6");
                    switch (choice)
                    {
                        case 0:
                            return;
                        case 1:
                            updateTask();
                            break;
                        case 2:
                            assignEngineerToTask();
                            break;
                        case 3:
                            printTasks();
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
                    Console.WriteLine(ex.Message);
                }

            } while (true);
        else
            do
            {
                Console.WriteLine("******************Project has NOT yet started******************");
                Console.WriteLine("Task Options:");
                Console.WriteLine("Please choose 0-6");
                Console.WriteLine(" 0 - Back to previous Menu");
                Console.WriteLine(" 1 - Add Task");
                Console.WriteLine(" 2 - Delete Task");
                Console.WriteLine(" 3 - Update existing Task");
                Console.WriteLine(" 4 - Assign Task to Engineer");
                Console.WriteLine(" 5 - Update dependencies of a task");
                Console.WriteLine(" 6 - Print all Tasks");
                Console.WriteLine("************************************");
                try
                {
                    if (!int.TryParse(Console.ReadLine(), out int choice))
                        throw new BO.BlInvalidUserInputException("Wrong input. please enter 0 - 6");
                    switch (choice)
                    {
                        case 0:
                            return;
                        case 1:
                            addTask();
                            break;
                        case 2:
                            deleteTask();
                            break;
                        case 3:
                            updateTask();
                            break;
                        case 4:
                            assignEngineerToTask();
                            break;
                        case 5:
                            updateTaskDependencies(readTaskId());
                            break;
                        case 6:
                            printTasks();
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
                    Console.WriteLine(ex.Message);
                }

            } while (true);

    }
    /// <summary>
    /// Options fo Milestone entity
    /// </summary>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    private static void milestoneOptions()
    {
        do
        {
            Console.WriteLine("******************Project has started******************");
            Console.WriteLine("Milestone Options:");
            Console.WriteLine("Please choose 0-6");
            Console.WriteLine(" 0 - Back to previous Menu");
            Console.WriteLine(" 1 - Update existing Milestone");
            Console.WriteLine(" 2 - Print all Milestones");
            Console.WriteLine("************************************");
            try
            {
                if (!int.TryParse(Console.ReadLine(), out int choice))
                    throw new BO.BlInvalidUserInputException("Wrong input. please enter 0 - 6");
                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                        updateMilestone();
                        break;
                    case 2:
                        foreach (var ms in s_bl.Milestone.ReadAll() ?? throw new BO.BlDoesNotExistException("There are no milestones in the project"))
                            Console.WriteLine(ms);
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
                Console.WriteLine(ex.Message);
            }

        } while (true);
    }
    /// <summary>
    /// Start the project. only availale for the admin
    /// </summary>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    private static void startProject()
    {
        while (true)
        {
            DateTime myStartDate, myEndDate;
                
            Console.WriteLine("Please enter the project starting date (format: dd-mm-yyyy)");
            do
            {
                if (DateTime.TryParse(Console.ReadLine(), out myStartDate) is false)
                    Console.WriteLine("wrong format");
                else break;
            } while (true);
            Console.WriteLine("Please enter the project ending date (format: dd-mm-yyyy)");
            do
            {
                if (DateTime.TryParse(Console.ReadLine(), out myEndDate) is false)
                    Console.WriteLine("wrong format");
                else break;
            } while (true);

            Console.WriteLine("Attemting to initialize project...");
            s_bl.Milestone.StartProject(myStartDate, myEndDate);
            Console.WriteLine("Schedule successfuly created! all tasks have been scheduled");
            s_projectStarted = true;
            CurrentTime = myStartDate;
            return;
        }
    }
    #endregion

    #region Milestone functions
    /// <summary>
    /// Update Milestone with uniqe alias, description and remarks values
    /// </summary>
    private static void updateMilestone()
    {
        int id = readTaskId();
        try
        {

            string alias, description, remarks;
            Console.WriteLine("Please enter Updated Milestone Alias");
            alias = Console.ReadLine() ?? throw new BO.BlInvalidUserInputException();
            Console.WriteLine("Please enter Updated Milestone Description");
            description = Console.ReadLine() ?? throw new BO.BlInvalidUserInputException();
            Console.WriteLine("Please enter free remarks");
            remarks = Console.ReadLine() ?? throw new BO.BlInvalidUserInputException();
            s_bl.Milestone.Update(id, alias, description, remarks);
            Console.WriteLine("Successfuly updated milestone with Id {0}", id);
        }
        catch (BO.BlInvalidUserInputException)
        {
            Console.WriteLine("Invalid Input");
        }
    }
    #endregion

    #region Engineer functions
    /// <summary>
    /// Update progress on assigned task as engineer
    /// </summary>
    private static void updateProgressOnTask()
    {
        int id = readEngineerId();
        printEngineerAssignedTask(id);
        s_bl.Engineer.UpdateEngineerProgress(id, CurrentTime);
    }
    /// <summary>
    /// Allows an engineer to take on a task from the availale ones
    /// </summary>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    private static void updateEngineerAssignedTask()
    {
        int id = printAvailableTasks(), taskId;
        Console.WriteLine("Please enter Id of a task from the obove list you wish to take");
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
        BO.Engineer eng = s_bl.Engineer.Read(id)!;
        eng.Task = new BO.TaskInEngineer() { Id = taskId, Alias = "" };
        s_bl.Engineer.Update(eng);
    }
    /// <summary>
    /// Updates engineer in the system
    /// </summary>
    private static void updateEngineer()
    {
        s_bl.Engineer.Update(readEngineer());
        Console.WriteLine("Engineer updated successfuly");
    }
    /// <summary>
    /// Prints all available task for a specific engineer
    /// </summary>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    private static int printAvailableTasks()
    {
        int id = readEngineerId();
        var tasks = s_bl.Task.ReadAllAvailableTasks(id) ?? throw new BO.BlDoesNotExistException("No available tasks");
        Console.WriteLine("Available Tasks:");
        foreach (var task in tasks)
            Console.WriteLine(task);
        return id;
    }
    /// <summary>
    /// Reads engineer Id and prints his\her assigned task
    /// </summary>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    private static void printEngineerAssignedTask(int? id = null)
    {
        if (id == null)
            id = readEngineerId();
        BO.Task task = s_bl.Engineer.ReadEngineerAssignedTask(id.Value);
        Console.WriteLine("Your assigned task: " + task);
    }
    /// <summary>
    /// Deletes an engineer by ID
    /// </summary>
    private static void deleteEngineer()
    {
        int id = readEngineerId();
        s_bl.Engineer.Delete(id);
        Console.WriteLine("Engineer deleted successfuly");
    }
    /// <summary>
    /// Returns BO.Engineer initialized from user input
    /// </summary>
    /// <returns>BO.Engineer type with user given values</returns>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    private static BO.Engineer readEngineer()
    {
        double cost;
        int level, taskId = 0, id = readEngineerId();
        //read name
        Console.WriteLine("Enter Engineer Name");
        string name = Console.ReadLine()!;
        //read email
        Console.WriteLine("Enter Engineer Email address");
        string email;
        while (true)
        {
            email = Console.ReadLine()!;
            try
            {
                if (!email.EndsWith("@gmail.com"))
                    throw new BO.BlInvalidUserInputException("Invalid Email Address. Address must end with '@gmail.com'");
                break;
            }
            catch (BlInvalidUserInputException ex)
            {
                Console.WriteLine(ex.Message); ;
            }

        }
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
        //Console.WriteLine("Optional: Assign a task to this Engineer(input task ID). to not assign any task, press 0");
        //while (true)
        //{
        //    try
        //    {
        //        if (!int.TryParse(Console.ReadLine(), out taskId))
        //            throw new BO.BlInvalidUserInputException("Invalid Input. Please enter an Integer");
        //        break;
        //    }
        //    catch (BlInvalidUserInputException ex)
        //    {
        //        Console.WriteLine(ex.Message); ;
        //    }

        //}
        return new BO.Engineer()
        {
            Id = id,
            Name = name,
            Email = email,
            Cost = cost,
            Level = (BO.EngineerExperience)level,
            Task = taskId != 0 ? new BO.TaskInEngineer() { Id = taskId } : null
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
        Console.WriteLine("Engineer added successfuly");
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
    
    #region Task functions
    /// <summary>
    /// Updates dependency list of a given task from user input
    /// </summary>
    /// <param name="taskId"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    private static void updateTaskDependencies(int taskId) 
    {
        //reads dependencies
        var dependencies = readDependencies(taskId);
        s_bl.Task.UpdateTaskDependencies(taskId, dependencies);
        Console.WriteLine("Successfuly updated dependencies");
    }
    /// <summary>
    /// Updates an existing task
    /// </summary>
    private static void updateTask()
    {
        BO.Task myTask = readTask();
        s_bl.Task.Update(myTask);
        Console.WriteLine("Task updated successfuly");
    }
    /// <summary>
    /// Assign an existing engineer to an existing task.
    /// will only work if the engineer is availale AND no other engineer is already assigned to the task
    /// </summary>
    private static void assignEngineerToTask()
    {
        int engId = readEngineerId();
        int taskId = readTaskId();
        if (s_projectStarted)
            s_bl.Task.UpdateAssignedEngineerAndStartWork(taskId, engId, CurrentTime);
        else
            s_bl.Task.UpdateAssignedEngineer(taskId, engId);
        Console.WriteLine($"Successfuly assigned engineer");
    }
    /// <summary>
    /// Deletes Task by ID
    /// </summary>
    private static void deleteTask()
    {
        int id = readTaskId();
        try
        {
            s_bl.Task.Delete(id);
        }
        catch (BlDoesNotExistException ex)
        {
            throw new BO.BlLogicViolationException(ex.Message + ", " + (ex.InnerException is not null ? ex.InnerException.Message : ""));
        }
        catch (BlLogicViolationException ex)
        {
            throw new BO.BlLogicViolationException(ex.Message + ", " + (ex.InnerException is not null ? ex.InnerException.Message : ""));
        }
        Console.WriteLine("Task deleted successfully");
    }
    /// <summary>
    /// Prints all the tasks in the data base with the option for detailed/concise print
    /// </summary>
    private static void printTasks()
    {
        int choice;
        Console.WriteLine("Choose printing format \n1 - Short \n2 - Long\n3 - print by milestone dependency");
        while (true)
        {
            try
            {
                if (!int.TryParse(Console.ReadLine(), out choice))
                    throw new BO.BlInvalidUserInputException("Invalid Input. Please enter an Integer");
                if (choice < 1 || choice > 3)
                    throw new BO.BlInvalidUserInputException("Number must be 1 - 3");
                break;
            }
            catch (BlInvalidUserInputException ex)
            {
                Console.WriteLine(ex.Message); ;
            }

        }
        switch (choice)
        {
            case 1:
                foreach (var task in s_bl.Task.ReadAllTasksInList())
                    Console.WriteLine(task);
                break;
            case 2:
                foreach (var task in s_bl.Task.ReadAll(t => !t.IsMilestone))
                    Console.WriteLine(task);
                break; 
            case 3:
                Console.WriteLine("Enter milestone name (alias):");
                string ms = Console.ReadLine() ?? throw new BO.BlInvalidUserInputException("Invalid Input");
                foreach (var task in s_bl.Task.ReadTasksByMilestone(ms))
                    Console.WriteLine(task);
                break;
        }
    }
    /// <summary>
    /// Tries to add a task from user input. throw exception if an input problem accured
    /// </summary>
    /// <exception cref="BO.BlInvalidValuesException"></exception>
    private static void addTask()
    {
        try
        {
            s_bl.Task.Add(readTask());
            Console.WriteLine("Task added successfuly");
        }
        catch (Exception ex)
        {

            throw new BO.BlInvalidValuesException(ex.Message + ", " + (ex.InnerException is not null ? ex.InnerException.Message : ""));
        }
    }
    /// <summary>
    /// Returns task entity from user input
    /// </summary>
    /// <param name="projectStarted">Project Started flag. determines whether to lock some of task input fiels.
    /// after project is initialized, some data fields cannot be changed</param>
    /// <returns>BO.task type form user input</returns>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    private static BO.Task readTask()
    {
        IEnumerable<BO.TaskInList>? dependencies = null;
        int level = 1, requiredTime = 0, id = readTaskId();
        //read alias
        Console.WriteLine("Enter Task alias (name)");
        string alias = Console.ReadLine()!;
        //read description
        Console.WriteLine("Enter Task description");
        string delscription = Console.ReadLine()!;
        //read deliverables
        Console.WriteLine("Enter the Task deliverables");
        string deliverables = Console.ReadLine()!;
        //read remarks
        Console.WriteLine("Optional: Enter free remarks. leave blank to skip");
        string? remarks = Console.ReadLine();
        if (remarks == "") remarks = null;

        //if the project hasnt started yet
        if (!s_projectStarted)
        {
            requiredTime = readTaskRequiredTime(false);
            level = readTaskComlexity(false);
        }

        //reads assigned engineer
        int engId = 0;
        //Console.WriteLine("Assign engineer to the task: Please enter engineer ID, enter 0 to assign no one");
        //while (true)
        //{
        //    try
        //    {
        //        if (!int.TryParse(Console.ReadLine(), out engId))
        //            throw new BO.BlInvalidUserInputException("Invalid Input. Please enter an integer");
        //        if (engId < 0)
        //            throw new BO.BlInvalidUserInputException("Invalid Input. Must be a positive number");
        //        break;
        //    }
        //    catch (BlInvalidUserInputException ex)
        //    {
        //        Console.WriteLine(ex.Message); ;
        //    }

        //}


        return new BO.Task()
        {
            Id = id,
            Description = delscription,
            Alias = alias,
            CreatedAtDate = DateTime.Now,
            Status = BO.Status.Scheduled,
            Dependencies = dependencies is null ? null : dependencies.ToList(),
            Milestone = null,
            RequiredEffortTime = new TimeSpan(requiredTime, 0, 0, 0),
            Deliverables = deliverables,
            Remarks = remarks,
            Engineer = engId == 0 ? null : new EngineerInTask() { Id = engId, Name = "" },
            Complexity = (BO.EngineerExperience)level
        };
    }
    /// <summary>
    /// Reads the Complexity in days of a specific task.
    /// if the bool param 'alsoUpdate' is true, will also read Task Id
    /// from the user and attempt to update said task with the new Comlexity
    /// </summary>
    /// <param name="alsoUpdate">Determines wether to also update the task</param>
    /// <returns>Task complexity as integer (1-5)</returns>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    private static int readTaskComlexity(bool alsoUpdate)
    {
        int level, id = new();
        if (alsoUpdate)
            id = readTaskId();
        //read comlexity
        Console.WriteLine(@"Please choose the task's comlexity. (choose 1-5)");
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
        if (alsoUpdate)
        {
            BO.Task task = s_bl.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Task with Id {id} does not exist");
            task.Complexity = (BO.EngineerExperience)level;
            s_bl.Task.Update(task);
        }

        return level;
    }
    /// <summary>
    /// Reads the required time in days of a specific task.
    /// if the bool param 'alsoUpdate' is true, will also read Task Id
    /// from the user and attempt to update said task with the new date
    /// </summary>
    /// <param name="alsoUpdate">Determines wether to also update the task as well</param>
    /// <returns>Task 'required effort time' in days</returns>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    private static int readTaskRequiredTime(bool alsoUpdate)
    {
        int id = new();
        if (alsoUpdate)
            id = readTaskId();
        int requiredTime;
        //reads required time
        Console.WriteLine($"Please enter the required time to complete the task (in days)");
        while (true)
        {
            try
            {
                if (!int.TryParse(Console.ReadLine(), out requiredTime))
                    throw new BO.BlInvalidUserInputException("Invalid Input. Please enter an integer");
                if (requiredTime <= 0)
                    throw new BO.BlInvalidUserInputException("Invalid Input. Must be a positive number");
                break;
            }
            catch (BlInvalidUserInputException ex)
            {
                Console.WriteLine(ex.Message); ;
            }

        }
        if (alsoUpdate)
        {
            BO.Task task = s_bl.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Task with Id {id} does not exist");
            task.RequiredEffortTime = new TimeSpan(requiredTime, 0, 0, 0);
            s_bl.Task.Update(task);
        }
        return requiredTime;
    }
    /// <summary>
    /// Return collection of dependencies from the user. 
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns>Collection of dependencies in the form of BO.TaskInList.
    /// the actual validity of the dependencies is yet unchecked at this point.
    /// returns null if no dependencies given by user</returns>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    private static IEnumerable<BO.TaskInList>? readDependencies(int taskId)
    {
        int choice, depOnTask = 0;
        List<BO.TaskInList> myDepOnTasks = new();
        Console.WriteLine("Caution! Added dependencies will overide previous ones");
        Console.WriteLine("Form dependencies: do you want to add dependency to this task? \n 1 - Yes \n 2 - No");
        while (true)
        {
            try
            {
                if (!int.TryParse(Console.ReadLine(), out choice))
                    throw new BO.BlInvalidUserInputException("Invalid Input. Please enter an integer");
                switch (choice)
                {
                    case 1:
                        Console.WriteLine($"Task {taskId} will be dependent on task ????  (Enter task ID)");
                        while (true)
                        { 
                            try
                            {
                                if (!int.TryParse(Console.ReadLine(), out depOnTask))
                                    throw new BO.BlInvalidUserInputException("Invalid Input. Please enter an integer");
                                break;
                            }
                            catch (BlInvalidUserInputException ex)
                            {
                                Console.WriteLine(ex.Message); ;
                            }
                        }
                        myDepOnTasks.Add(new BO.TaskInList() { Id = depOnTask, Alias = "", Description = "", Status = BO.Status.Unscheduled });
                        break;
                    case 2:
                        return myDepOnTasks.Any() == false ? null : myDepOnTasks;
                    default:
                        throw new BO.BlInvalidUserInputException("Invalid Input. Enter One or two");
                }
                
            }
            catch (BlInvalidUserInputException ex)
            {
                Console.WriteLine(ex.Message); ;
            }
            Console.WriteLine("Do you want to add additional dependency? \n 1 - Yes \n 2 - No");
        }
    }
    /// <summary>
    /// Reads task Id from user
    /// </summary>
    /// <returns></returns>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    private static int readTaskId()
    {
        int id;
        Console.WriteLine("Please enter Task ID number:");
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
    /// assign given task to given engineer
    /// </summary>
    /// <param name="engId"></param>
    /// <param name="taskId"></param>
    private static void assignEngineerToTask(int engId, int taskId)
    {
        s_bl.Task.UpdateAssignedEngineerAndStartWork(taskId, engId);
        Console.WriteLine($"Task {taskId} successfuly assigned to engineer {engId}");
    }
    #endregion
}


