namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System.Collections.Generic;
using System.Linq;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;


    /// <summary>
    /// Adds the given Task to the data-base
    /// </summary>
    /// <param name="task"></param>
    /// <exception cref="BO.BlInvalidValuesException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public void Add(BO.Task task)
    {
        if (task.Id <= 0)
            throw new BO.BlInvalidValuesException("Invalid ID number. ID must be a positive number");
        if (task.Alias == "")
            throw new BO.BlInvalidValuesException("Invalid Alias. Alias field cannot be empty");
        try
        {
            _dal.Task.Create(ConvertTaskFromBlToDal(task));
            

        }
        catch (DO.DalAlreadyExistException ex) 
        {
            throw new BO.BlAlreadyExistsException($"Task with ID {task.Id} already exists", ex);
        }
        try 
        {
            _dal.Dependency.Create((DO.Dependency)(from d in task.Dependencies
                                                   select new DO.Dependency { DependentTask = d.Id, DependsOnTask = task.Id }));
        }

        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistsException($"Error creating dependencies for task {task.Alias}", ex);
        }

    }

    /// <summary>
    /// Deletes the Task by the given ID number only if no tasks exist that are dependent on this task
    /// </summary>
    /// <param name="engineer"></param>
    public void Delete(int Id)
    {
        try
        {
            if (_dal.Dependency.ReadAll(d => d.DependsOnTask == Id).Count() > 0)
                throw new BO.BlLogicDenialException($"There are Tasks that depend on Task {Id}");

            try
            {
                var depsToDelete = (from d in _dal.Dependency.ReadAll()
                                    where d.DependentTask == Id
                                    select d.Id);
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
    /// <returns>A Task that matches the given ID</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Task? Read(int Id)
    {
        DO.Task? task = _dal.Task.Read(Id);
        if (task is null)
            return null;
        return ConvertTaskFromDalToBl(task);
    }

    /// <summary>
    /// Reads all Tasks from data-base that fill the given condition. read all if no condition is given
    /// </summary>
    /// <param name="filter">Optional delegate filter</param>
    /// <returns>Collection of Tasks that meet the given condition. returns all if a condition is not given</returns>
    public IEnumerable<BO.Task>? ReadAll(Func<BO.Task, bool>? filter = null)
    {
        var tasks = _dal.Task.ReadAll().Select(t => ConvertTaskFromDalToBl(t!));
        if (filter is null)
            return tasks;
        return (from t in tasks
                where filter!(t) == true
                select t);
    }


    /// <summary>
    /// Reads all the tasks that this task (given by ID) is dependent on
    /// </summary>
    /// <param name="Id"></param>
    /// <returns>Collection of tasks that the given task is dependent on</returns>
    public IEnumerable<TaskInList>? ReadDependsOnTasks(int Id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates Task in the data-base according to the values recieved as parmeter
    /// </summary>
    /// <param name="task"></param>
    public void Update(BO.Task task)
    {
        if (task.Id <= 0)
            throw new BO.BlInvalidValuesException("Invalid ID number. ID must be a positive number");
        if (task.Alias == "")
            throw new BO.BlInvalidValuesException("Invalid Alias. Alias field cannot be empty");
        if (_dal.Task.Read(task.Id) is null)
            throw new BO.BlDoesNotExistException($"Task with ID {task.Id} not found");

        _dal.Task.Update(ConvertTaskFromBlToDal(task));
    }

    /// <summary>
    /// Updates the start date of the the given task (recieved by ID).
    /// performs logical checks to ensure the date is valid (in terms of scheduled dependencies).
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="Id"></param>
    public void UpdateTaskStartDate(DateTime startDate, int Id)
    {
       

    }


    /// <summary>
    /// Assignes the given engineer to the given task.
    /// if no engineer is given, will delete the current engineer working on the task
    /// </summary>
    /// <param name="task"></param>
    /// <param name="engineer"></param>
    public void UpdateAssignedEngineer(BO.Task task, BO.Engineer? engineer = null) 
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Converts BO.Task type to DO.Task type
    /// </summary>
    /// <param name="blTask"></param>
    /// <returns>DO.Task type made from the given BO.Task type</returns>
    private DO.Task ConvertTaskFromBlToDal(BO.Task blTask)
    {
        return new DO.Task
        {
            Id = blTask.Id,
            Alias = blTask.Alias,
            Description = blTask.Description,
            CreatedAtDate = DateTime.Now,
            RequiredEffortTime = blTask.RequiredEffortTime,
            Complexity = (DO.EngineerExperience)blTask.Copmlexity,
            IsMilestone = (blTask.Milestone is not null),
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
    /// <param name="blTask"></param>
    /// <returns>BO.Task type made from the given DO.Task type</returns>
    private BO.Task ConvertTaskFromDalToBl(DO.Task dalTask)
    {
        return new BO.Task
        {
            Id = dalTask.Id,
            Alias = dalTask.Alias,
            Description = dalTask.Description,
            CreatedAtDate = dalTask.CreatedAtDate,
            Status = CalculateStatus(dalTask),
            Dependencies = CalculateDependencies(dalTask.Id)?.ToList(),
            Milestone = null,
            RequiredEffortTime = dalTask.RequiredEffortTime,
            StartDate = dalTask.StartDate,
            ScheduledDate = dalTask.ScheduledDate,
            ForecastDate = CalculateForcastDate(dalTask.StartDate, dalTask.ScheduledDate, dalTask.RequiredEffortTime),
            DeadlineDate = dalTask?.DeadlineDate,
            CompleteDate = dalTask?.CompleteDate,
            Deliverables = dalTask.Deliverables ?? "Empty",
            Remarks = dalTask.Remarks,
            Engineer = CalculateEngineerInTask(dalTask.EngineerId),
            Copmlexity = (BO.EngineerExperience)dalTask.Complexity
        };
    }

    /// <summary>
    /// Calculates the planned completion date of a task
    /// </summary>
    /// <param name="start"></param>
    /// <param name="plannedStart"></param>
    /// <param name="time"></param>
    /// <returns>Max {start, plannedStart} + requiredTime(days)</returns>
    /// <exception cref="NotImplementedException"></exception>
    private DateTime? CalculateForcastDate(DateTime? start, DateTime? plannedStart, TimeSpan time)
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
    /// Calculates task status based on other fields. UNFINISHED!!!
    /// </summary>
    /// <returns></returns>
    private BO.Status CalculateStatus(DO.Task task)
    {
        return BO.Status.Scheduled;
    }

    private IEnumerable<BO.TaskInList>? CalculateDependencies(int id)
    {
        var deps = _dal.Dependency.ReadAll(d => d.DependsOnTask == id);
        if (!deps.Any())
            return null;
        return (from t in deps
                let task = _dal.Task.Read(t.DependentTask)
                        select new BO.TaskInList {
                            Id = task.Id,
                            Description = task.Description,
                            Alias = task.Alias,
                            Status = CalculateStatus(task)
                        });
    }


    /// <summary>
    /// Gives BO.EngineerInTask type of the assigned engineer
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BO.EngineerInTask type of the engineer that is assigned to the given task</returns>
    private BO.EngineerInTask? CalculateEngineerInTask(int? id)
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

    public BO.TaskInEngineer? ReadTaskInEngineer(int id)
    {
        BO.Task? task = Read(id);
        if (task is null) 
            return null;
        return new BO.TaskInEngineer() { Id = task.Id, Alias = task.Alias };
    }
}
