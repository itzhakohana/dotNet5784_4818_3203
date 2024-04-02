namespace BlImplementation;
using BlApi;
using BO;
using System.ComponentModel.Design;
using System.Data;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private readonly BlApi.IBl s_bl;
    internal TaskImplementation(IBl bl) => s_bl = bl;

    /// <summary>
    /// Adds new engineer to the data base. calculates dependencies and status
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BO.BlInvalidValuesException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Add(BO.Task task)
    {
        //validating data
        if(task.Id != 0)
            if (task.Id <= 0)
                throw new BO.BlInvalidValuesException("Invalid ID number. ID must be a positive number");
        if (_dal.Task.Read(t => t.Id == task.Id) is not null)
            throw new BO.BlAlreadyExistsException($"Task with ID {task.Id} already exists");
        if (task.Alias == "")
            throw new BO.BlInvalidValuesException("Invalid Alias. Alias field cannot be empty");

        //calculating status
        task.Status = CalculateStatus(task.CompleteDate, task.StartDate, task.ScheduledDate, task.DeadlineDate, task.ForecastDate);

        //calculating and validating the assigned engineer
        if (task.Engineer is not null)
        {
            validateAssignedEngineer(task.Engineer.Id);
            task.Engineer = calculateEngineerInTask(task.Engineer.Id) ?? throw new BO.BlDoesNotExistException($"Engineer with ID {task.Engineer.Id} does not exist");
        }

        try
        {
            task.CreatedAtDate = s_bl.Clock;
            _dal.Task.Create(convertTaskFromBlToDal(task));
        }
        catch (DO.DalAlreadyExistException ex) 
        {
            throw new BO.BlAlreadyExistsException($"Task with ID {task.Id} already exists", ex);
        }
        catch(Exception ex) 
        {
            throw new BO.BlInvalidValuesException($"Unknown error when trying to creat task {task.Id}", ex);
        }
        
    }
    /// <summary>
    /// Deletes the Task by the given ID number only if no tasks exist that are dependent on this task
    /// </summary>
    /// <param name="Id"></param>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int Id)
    {
        try
        {
            if (_dal.Dependency.ReadAll(d => d.DependsOnTask == Id).Count() > 0)
                throw new BO.BlLogicViolationException($"There are Tasks that depend on Task {Id}");

            try
            {
                List <int> depsToDelete = (from d in _dal.Dependency.ReadAll(dep => dep.DependentTask == Id)
                                    select d.Id).ToList();
                foreach (var depId in depsToDelete)
                {
                    _dal.Dependency.Delete(depId);
                }
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"Errorr deleting dependencies of task {Id}", ex);
            }
            _dal.Task.Delete(Id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Errorr deleting task {Id}", ex);
        }
    }
    /// <summary>
    /// Search for Task in the data-base by ID
    /// </summary>
    /// <param name="Id"></param>
    /// <returns>BO.Task if found. null if not</returns>
    public BO.Task? Read(int Id)
    {
        DO.Task? task = _dal.Task.Read(t =>t.Id == Id);
        if (task is null)
            return null;
        return convertTaskFromDalToBl(task);
    }
    /// <summary>
    /// Searchs a task by filter function (condition)
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>BO.Task if found. null if not </returns>
    public Task? Read(Func<Task, bool> filter)
    {
        var tasks = (from task in _dal.Task.ReadAll()
                     select convertTaskFromDalToBl(task));
        return (from task in tasks
                where filter(task)
                select task).FirstOrDefault();
    }
    /// <summary>
    /// Reads all Tasks from data-base that fill the given condition. read all if no condition is given
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Collection of tasks that match the filter. all tasks if filter is not given</returns>
    public IEnumerable<BO.Task>? ReadAll(Func<BO.Task, bool>? filter = null)
    {
        var tasks = _dal.Task.ReadAll().Select(t => convertTaskFromDalToBl(t!));
        if (filter is null)
            return tasks;
        return (from t in tasks
                where filter!(t) == true
                select t);
    }
    /// <summary>
    /// Updates Task in the data-base according to the values recieved as parmeter
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BO.BlInvalidValuesException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Task task)
    {
        //data validation
        if (task.Id <= 0)
            throw new BO.BlInvalidValuesException("Invalid ID number. ID must be a positive number");
        if (task.Alias == "")
            throw new BO.BlInvalidValuesException("Invalid Alias. Alias field cannot be empty");
        
        //finding the original engineer in the data base
        BO.Task originalTask = convertTaskFromDalToBl(_dal.Task.Read(task.Id) 
            ?? throw new BO.BlDoesNotExistException($"Task with ID {task.Id} does not exist"));

        //deleting dependencies of the original task
        try
        {
            if (task.Dependencies is not null && !ProjectHasStarted())
            {

                //collection of all the dependencies we need to creat
                IEnumerable<DO.Dependency> newDeps = (from d in task.Dependencies
                                                      let id = d.Id
                                                      from t in _dal.Task.ReadAll()
                                                      where id == t.Id
                                                      select new DO.Dependency { DependentTask = task.Id, DependsOnTask = id });
                //checking that all the tasks in blTask.Dependencies are found (exist) in previous query
                if (newDeps.Count() != task.Dependencies.Count())
                    throw new BO.BlDoesNotExistException($"At least one dependency-task not found");
                //deleting old dependencies (from the original task)
                var depsToDelete = (from dep in _dal.Dependency.ReadAll(d => d.DependentTask == task.Id)
                                    select dep).ToList();
                foreach (var dep in depsToDelete)
                    _dal.Dependency.Delete(dep.Id);
                //adds the new dependencies to the data-base
                foreach (var dep in newDeps)
                {
                    //a similar dependency does not already exist 
                    if (_dal.Dependency.Read(d => dep.DependsOnTask == d.DependsOnTask && dep.DependentTask == d.DependentTask) is null)
                        _dal.Dependency.Create(dep);
                }
                //adds the task-dependencies to the new task as BO.TaskInList types
                task.Dependencies = (from d in _dal.Dependency.ReadAll(dep => dep.DependentTask == task.Id)
                                       let t = _dal.Task.Read(d.DependsOnTask)
                                       select new BO.TaskInList()
                                       {
                                           Id = t.Id,
                                           Alias = t.Alias,
                                           Description = t.Description,
                                           Status = CalculateStatus(t.CompleteDate, t.StartDate, t.ScheduledDate, t.DeadlineDate)
                                       }).ToList();

            }
            else task.Dependencies = originalTask.Dependencies;
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlLogicViolationException($"Error setting dependencies of task: ({task.Id} {task.Alias}) || " + ex.Message, ex);
        }

        //calculating status
        task.Status = CalculateStatus(task.CompleteDate, task.StartDate, task.ScheduledDate, task.DeadlineDate, task.ForecastDate);
        //fields we dont wish to change
        if (ProjectHasStarted())
        {
            task.Complexity = originalTask.Complexity;
            task.RequiredEffortTime = originalTask.RequiredEffortTime;
            task.ScheduledDate = originalTask.ScheduledDate;
            task.DeadlineDate = originalTask.DeadlineDate;
        }

        //task.StartDate = originalTask.StartDate;
        //task.ForecastDate = originalTask.ForecastDate;
        //task.CompleteDate = originalTask.CompleteDate;
        //task.CreatedAtDate = originalTask.CreatedAtDate;
        //task.Engineer = originalTask.Engineer;
        task.Milestone = originalTask.Milestone;
        _dal.Task.Update(convertTaskFromBlToDal(task));
    }
    /// <summary>
    /// Assignes the given engineer to the given task.
    /// if no engineer is given, will set assigned engineer to null.
    /// also sets the starting date (to Date.Now if no date is given)
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="engId"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public void UpdateAssignedEngineerAndStartWork(int taskId, int? engId = null, DateTime? startDate = null)
    {
        if (startDate is null)
            //startDate = s_bl.CurrentTime;
            startDate = s_bl.Clock;
        DO.Task task = _dal.Task.Read(taskId) ?? throw new BO.BlDoesNotExistException($"Task with ID {taskId} does not exist");
        if (engId is null)
        {   
            _dal.Task.Update(task with { EngineerId = null });
            return;
        }
        validateAssignedEngineer(engId.Value, taskId);
        _dal.Task.Update(task with {EngineerId = engId, StartDate = startDate});
    }
    /// <summary>
    /// Assigns the given task to the given engineer without starting the work (will NOT set starting date)
    /// </summary>
    /// <param name="engId"></param>
    /// <param name="taskId"></param>
    public void UpdateAssignedEngineer(int engId, int taskId)
    { 
        validateAssignedEngineer(engId, taskId);
        DO.Task task = _dal.Task.Read(taskId) ?? throw new BO.BlDoesNotExistException($"Task with ID {taskId} does not exist");
        _dal.Task.Update(task with {EngineerId = engId});
    }
    /// <summary>
    /// Converts BO.Task type to DO.Task type
    /// </summary>
    /// <param name="blTask"></param>
    /// <returns>A new DO.Task type based on the given BO.Task type </returns>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    private DO.Task convertTaskFromBlToDal(BO.Task blTask)
    {
        return new DO.Task
        {
            Id = blTask.Id,
            Alias = blTask.Alias,
            Description = blTask.Description,
            CreatedAtDate = blTask.CreatedAtDate,
            RequiredEffortTime = blTask.RequiredEffortTime,
            Complexity = (DO.EngineerExperience)blTask.Complexity,
            IsMilestone = blTask.IsMilestone,
            StartDate = blTask.StartDate,
            ScheduledDate = blTask.ScheduledDate,
            DeadlineDate = blTask.DeadlineDate,
            CompleteDate = blTask.CompleteDate,
            Deliverables = blTask.Deliverables,
            Remarks = blTask.Remarks,
            EngineerId = blTask.Engineer?.Id
        };

    }
    /// <summary>
    /// Converts DO.Task type to BO.Task type
    /// </summary>
    /// <param name="dalTask"></param>
    /// <returns>A new BO.Task type based on the given DO.Task type </returns>
    private BO.Task convertTaskFromDalToBl(DO.Task dalTask)
    {
        return new BO.Task
        {
            Id = dalTask.Id,
            Alias = dalTask.Alias,
            Description = dalTask.Description,
            CreatedAtDate = dalTask.CreatedAtDate,
            Dependencies = calculateDependencies(dalTask.Id)?.ToList(),
            ForecastDate = calculateForcastDate(dalTask.StartDate, dalTask.ScheduledDate, dalTask.RequiredEffortTime),
            Status = CalculateStatus(dalTask),
            Milestone = CalculateMilestoneInTask(dalTask.Id),
            IsMilestone = dalTask.IsMilestone,
            RequiredEffortTime = dalTask.RequiredEffortTime,
            StartDate = dalTask.StartDate,
            ScheduledDate = dalTask.ScheduledDate,
            DeadlineDate = dalTask.DeadlineDate,
            CompleteDate = dalTask.CompleteDate,
            Deliverables = dalTask.Deliverables ?? "Empty",
            Remarks = dalTask.Remarks,
            Engineer = calculateEngineerInTask(dalTask.EngineerId),
            Complexity = (BO.EngineerExperience)dalTask.Complexity
        };
    }
    /// <summary>
    /// Calculates the planned completion date of a task
    /// </summary>
    /// <param name="start"></param>
    /// <param name="plannedStart"></param>
    /// <param name="time"></param>
    /// <returns>Max {start, plannedStart} + requiredTime(days)</returns>
    private DateTime? calculateForcastDate(DateTime? start, DateTime? plannedStart, TimeSpan time)
    {
        if (start is null && plannedStart is null) 
            return null;
        else if (start is null) 
            return plannedStart + time;
        else if (plannedStart is null)
            return start + time;
        return DateTime.Compare(start.Value, plannedStart.Value) > 0 ? start + time: plannedStart + time;
    }
    /// <summary>
    /// Recieves task Id and returns a collection of BO.TaskInList which are the tasks that
    /// the given Task (given by ID) is dependent on (prior tasks)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private IEnumerable<BO.TaskInList>? calculateDependencies(int id)
    {
        var deps = _dal.Dependency.ReadAll(d => d.DependentTask == id);
        if (!deps.Any())
            return null;
        return (from d in deps
                let task = _dal.Task.Read(d.DependsOnTask)
                where task is not null
                    select new BO.TaskInList 
                        {
                            Id = task.Id,
                            Description = task.Description,
                            Alias = task.Alias,
                            Status = task.IsMilestone ? calculateDependencies(task.Id) is null || calculateDependencies(task.Id)!.ToList().All(d => d.Status == BO.Status.Done)
                                ? BO.Status.Done : BO.Status.Scheduled : CalculateStatus(task.CompleteDate, task.StartDate, task.ScheduledDate, task.DeadlineDate),
                    });
    }
    /// <summary>
    /// Calculates task status from dateTime fields
    /// </summary>
    /// <param name="CompleteDate"></param>
    /// <param name="StartDate"></param>
    /// <param name="ScheduledDate"></param>
    /// <param name="DeadlineDate"></param>
    /// <param name="ForecastDate"></param>
    /// <returns>BO.Status type according to the given dates</returns>
    public BO.Status CalculateStatus(DateTime? CompleteDate, DateTime? StartDate, DateTime? ScheduledDate,
        DateTime? DeadlineDate, DateTime? ForecastDate = null)
    {
        if (CompleteDate is not null)
            return BO.Status.Done;
        else if ((DeadlineDate is not null && ((DeadlineDate < ForecastDate) || s_bl.Clock > DeadlineDate)))
            return BO.Status.InJeopardy;
        else if (StartDate is not null)
        {             
            return BO.Status.OnTrack; 
        }
        else if (ScheduledDate is null)
            return BO.Status.Unscheduled;        
        return BO.Status.Scheduled;
    }
    /// <summary>
    /// Calculates status from given dalTask (also works on milestones)
    /// </summary>
    /// <param name="dalTask"></param>
    /// <returns>BO.status of the given task or milestone</returns>
    public BO.Status CalculateStatus(DO.Task dalTask)
    {
        if (dalTask.IsMilestone)
            if (calculateDependencies(dalTask.Id) is null || calculateDependencies(dalTask.Id)!.ToList().All(d => d.Status == BO.Status.Done))
                return BO.Status.Done;
            else
                return BO.Status.Scheduled;
        return CalculateStatus(dalTask.CompleteDate, dalTask.StartDate, dalTask.ScheduledDate, 
            dalTask.DeadlineDate, calculateForcastDate(dalTask.StartDate, dalTask.ScheduledDate, dalTask.RequiredEffortTime));
    }
    /// <summary>
    /// Gives BO.EngineerInTask type of the engineer given as id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BO.EngineerInTask type of the engineer given by id, null if the engineer does not
    /// exist in the data-base</returns>
    private BO.EngineerInTask? calculateEngineerInTask(int? id)
    {
        if (id == null) 
            return null;
        DO.Engineer? engineer = _dal.Engineer.Read(eng => eng.Id == id);
        if (engineer is null)
            return null;
        return new BO.EngineerInTask
        {
            Id = engineer!.Id,
            Name = engineer!.Name
        };
    }
    /// <summary>
    /// Verfies that the given Engineer (given by ID) can be assigned to the given task (given by ID).
    /// if engineer already working on a different task 
    /// OR task already is assigned a different engineer
    /// OR task does not exist, will throw exeption. else, will do nothing.
    /// if no task Id is given, will only check that engineer isnt already assigned to a 
    /// another task
    /// </summary>
    /// <param name="engineer"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    private void validateAssignedEngineer(int engId, int? taskId = null)
    {
        DO.Task task;
        if (taskId is not null)
        {
            task = _dal.Task.Read(taskId.Value) 
                ?? throw new BO.BlDoesNotExistException($"Task with ID {taskId} does not exist");
            if (task.IsMilestone)
                throw new BO.BlLogicViolationException("Task cannot be a milestone");
            if (ProjectHasStarted())
                if (s_bl.Milestone.Read(s_bl.Task.Read(taskId.Value).Milestone.Id).Status != BO.Status.Done)
                    throw new BO.BlLogicViolationException($"Task {$"({taskId} {task.Alias})"} cannot be worked on since its prior Milestone is not yet completed");
            if (_dal.Engineer.Read(engId) is null)
                throw new BO.BlDoesNotExistException($"Engineer with ID {engId} does not exist");
            if (task.EngineerId is not null)
                throw new BO.BlLogicViolationException($"A different engineer is already assigned to task {task.Id}");
            if (task.Complexity > _dal.Engineer.Read(engId)!.Level)
                throw new BO.BlLogicViolationException($"Task's comlexity is too advanced for the engineer. maximum comlexity allowed: {_dal.Engineer.Read(engId)!.Level}");
        }
        if (_dal.Task.ReadAll(t => t.EngineerId == engId && t.CompleteDate is null).Any())
            throw new BO.BlLogicViolationException($"Engineer with ID {engId} is already assigned to a task");       
    }
    /// <summary>
    /// Returns the parent Milestone of the given task
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BO.MilestoneInTask type of the parent milestone</returns>
    public BO.MilestoneInTask? CalculateMilestoneInTask(int id)
    {
        if (_dal.Task.Read(t => t.Id == id && t.IsMilestone) is not null)
        {
            return null;
            //the task is the starting milestone
            //if (_dal.Dependency.ReadAll(d => d.DependentTask == id) is null)
            //    return new BO.MilestoneInTask() { Id = id, Alias = _dal.Task.Read(id)!.Alias };
            //else
            //    return new BO.MilestoneInTask() { Id = id, Alias = _dal.Task.Read(id)!.Alias }; 
        }
        
        return (from d in _dal.Dependency.ReadAll(d => d.DependentTask == id)
                    let ms = _dal.Task.Read(t => t.Id == d.DependsOnTask && t.IsMilestone)
                    where ms is not null
                    select new BO.MilestoneInTask() { Id = ms.Id, Alias = ms.Alias}).FirstOrDefault();
    }
    /// <summary>
    /// Reads a task by filter function (condition). if found returns 
    /// TaskInList type (a concise vertion of task entity)
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>BO.TaskInList type of the found task. null if not found</returns>
    public BO.TaskInList? ReadTaskInList(Func<TaskInList, bool> filter)
    {
        return (from task in _dal.Task.ReadAll()
                     select convertTaskFromDalToBl(task)
                     into t
                     select new BO.TaskInList()
                     {
                         Id = t.Id,
                         Alias = t.Alias,
                         Description = t.Description,
                         Status = t.IsMilestone ? calculateDependencies(t.Id) is null || calculateDependencies(t.Id)!.ToList().All(d => d.Status == BO.Status.Done)
                                ? BO.Status.Done : BO.Status.Scheduled : CalculateStatus(t.CompleteDate, t.StartDate, t.ScheduledDate, t.DeadlineDate),
                     }
                     into t
                     where filter(t)
                     select t).FirstOrDefault();
    }
    /// <summary>
    /// Gives a Collection of TaskInList types that match a given filter condition.
    /// all tasks if filter function is not given
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Collection of TaskInList types that match the given filter. if no
    /// filter is given, returns all tasks in the data base</returns>
    public IEnumerable<TaskInList>? ReadAllTasksInList(Func<TaskInList, bool>? filter = null)
    {
        var tasks = (from task in _dal.Task.ReadAll(t => !t.IsMilestone)
                     select convertTaskFromDalToBl(task)
                     into t
                     select new BO.TaskInList()
                     {
                         Id = t.Id,
                         Alias = t.Alias,   
                         Description = t.Description,
                         Status = t.IsMilestone ? calculateDependencies(t.Id) is null || calculateDependencies(t.Id)!.ToList().All(d => d.Status == BO.Status.Done)
                                ? BO.Status.Done : BO.Status.Scheduled : CalculateStatus(t.CompleteDate, t.StartDate, t.ScheduledDate, t.DeadlineDate),
                     });
        if (filter is null) 
            return tasks;
        return (from task in tasks
                where filter(task)
                select task);
    }
    /// <summary>
    /// Updates the given task's scheduled start date (and consequently, ForecastDate) on conditions:
    /// 1 all prior task dependencies are already have scheduled date.
    /// 2 the task's scheduled date must be later then its preceeding tasks(task must start after them)
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="scheduledDate"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public void CalculateScheduledDate(int taskId, DateTime scheduledDate)
    {
        BO.Task myTask = Read(taskId) ?? throw new BO.BlDoesNotExistException($"Task with Id {taskId} does not exist");
        var depTasks = (from d in _dal.Dependency.ReadAll(d => d.DependentTask == taskId)
                       select convertTaskFromDalToBl(_dal.Task.Read(d.DependsOnTask) 
                            ?? throw new BO.BlDoesNotExistException($"Error loading task dependencies. task with Id {d.DependsOnTask} not found")) 
                       ).ToList();
        
        foreach(var task in depTasks)
        {
            if (task.ScheduledDate is null)
                throw new BO.BlLogicViolationException("One or more of Prior tasks are not initialized with a Scheduled Date");
        }
        foreach (var task in depTasks)
        {
            if (task.ScheduledDate is null)
                throw new BO.BlLogicViolationException("Scheduled Date collision. the task's Scheduled date must be after prior tasks");
        }
        myTask.ScheduledDate = scheduledDate;
        myTask.ForecastDate = scheduledDate + myTask.RequiredEffortTime;
        Update(myTask);
    }
    /// <summary>
    /// Gives all task that are available for the given engineer
    /// </summary>
    /// <param name="engineerId"></param>
    /// <returns>Collection of tasks that the given engineer is allowed to work on</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public IEnumerable<BO.TaskInList>? ReadAllAvailableTasks(int engineerId)
    {
        BO.Engineer engineer = s_bl.Engineer.Read(engineerId)
            ?? throw new BO.BlDoesNotExistException($"Engineer with Id {engineerId} does not exist");
        return (from task in ReadAll(t => !t.IsMilestone && (t.Dependencies is null || t.Dependencies.All(dep => dep.Status == BO.Status.Done)))
                     where task.Complexity <= (BO.EngineerExperience)engineer.Level && task.Engineer is null
                     select new BO.TaskInList()
                     {
                         Id = task.Id,
                         Alias = task.Alias,
                         Description = task.Description,
                         Status = task.IsMilestone ? calculateDependencies(task.Id) is null || calculateDependencies(task.Id)!.ToList().All(d => d.Status == BO.Status.Done)
                                ? BO.Status.Done : BO.Status.Scheduled : CalculateStatus(task.CompleteDate, task.StartDate, task.ScheduledDate, task.DeadlineDate),
                     });
    }
    /// <summary>
    /// Determines wether the project has started already by checking that all tasks and milestones
    /// are initialized with scheduled and deadline dates.
    /// </summary>
    /// <returns>True if project has started, false if not</returns>
    public bool ProjectHasStarted ()
    {
        return (s_bl.DateControl.GetStartDate() != null && s_bl.DateControl.GetEndDate() != null);
        //var tasks = _dal.Task.ReadAll().ToList();
        //return (tasks.Any() && tasks.All(t => t!.ScheduledDate is not null && t.DeadlineDate is not null));
    }
    /// <summary>
    /// Deletes all existing tasks and dependencies
    /// </summary>
    public void Reset()
    {
        _dal.Task.Reset();
        _dal.Dependency.Reset();
    }
    /// <summary>
    /// Updates the given dependencies list to the given task
    /// </summary>
    /// <param name="taskId"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlInvalidUserInputException"></exception>
    public void UpdateTaskDependencies(int taskId, IEnumerable<BO.TaskInList>? dependencies)
    {
        BO.Task myTask = s_bl.Task.Read(taskId) ??
            throw new BO.BlDoesNotExistException($"Task with Id {taskId} does not exist");
        if (dependencies is null)
            throw new BO.BlInvalidUserInputException("No dependencies given. no changes made");
        myTask.Dependencies = dependencies.ToList();
        s_bl.Task.Update(myTask);
    }
    /// <summary>
    /// Accepts milestone alias string and gives a collection
    /// of tasks which are dependent on (come after) said milestone
    /// </summary>
    /// <param name="alias"></param>
    /// <returns>Collection of tasks dependent on the given milestone</returns>
    public IEnumerable<BO.Task>? ReadTasksByMilestone(string alias)
    {
        if (s_bl.Milestone.Read(ms => ms.Alias == alias) is null)
            throw new BO.BlDoesNotExistException($"Milestone with Alias {alias} not found");
        return (from task in ReadAll(t => !t.IsMilestone)
                where task.Milestone!.Alias == alias
                select task);
    }
    /// <summary>
    /// Reads all Tasks(only tasks!) from data-base that fill the given condition. read all if no condition is given
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Collection of tasks only! that match the filter. all tasks if filter is not given</returns>
    public IEnumerable<Task>? ReadAllTasks(Func<Task, bool>? filter = null)
    {
        var tasks = _dal.Task.ReadAll(t => !t.IsMilestone).Select(t => convertTaskFromDalToBl(t!));
        if (filter is null)
            return tasks;
        return (from t in tasks
                where filter!(t) == true
                select t);
    }
    /// <summary>
    /// Checks if there is a dependency between the two given tasks.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True if there is one task is dependant on the other, false if not</returns>
    public bool CheckDependency(int id1, int id2)
    {
        if (_dal.Dependency.Read(d => (d.DependentTask == id1 && d.DependsOnTask == id2) || (d.DependentTask == id2 && d.DependsOnTask == id1)) is not null)
            return true;
        return false;
    }
    /// <summary>
    /// Returns the time left for the completion of the task. assumes the project has started, and an engineer is assigned to the task
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the time left for the completion of the task</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public TimeSpan TimeLeftForTask(int id)
    {
        BO.Task task = Read(id) ?? throw new BO.BlDoesNotExistException($"Task {id} not found");
        if (task.Engineer is null) throw new BO.BlLogicViolationException($"Task {id} is not assigned to an engineer");
        if (task.CompleteDate is not null) throw new BO.BlLogicViolationException($"Task {id} is already completed");
        if (!ProjectHasStarted()) throw new BO.BlLogicViolationException("Project not yet started");
        var startingDate = task.StartDate != null && task.ScheduledDate < task.StartDate ? task.StartDate : task.ScheduledDate;
        return (task.DeadlineDate - s_bl.Clock)!.Value;
    }
    /// <summary>
    /// Checkes whether its possible to start work on a task. in terms of dependencies
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True if all previous tasks are completed and work can be started, false otherwise</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public bool CanStartWork(int id)
    {
        BO.Task? task = Read(id);
        if (task is null)
            return false;
        if (!ProjectHasStarted() || task.Milestone is null) return false;
        if (s_bl.Milestone.Read(task.Milestone.Id) is null) return false;
        return s_bl.Milestone.Read(task.Milestone.Id)!.Status == BO.Status.Done;
    }
}
