namespace BlImplementation;
using BlApi;
using BO;


/// <summary>
/// Interface implementation of the Logic-layer Engineer entity
/// </summary>
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Adds the given Engineer to the data-base
    /// </summary>
    /// <param name="engineer"></param>
    public void Add(BO.Engineer engineer)
    {
        if (engineer.Id <= 0)
            throw new BO.BlInvalidValuesException("Invalid Id number. Id must be a positive number");
        if (engineer.Name == "")
            throw new BO.BlInvalidValuesException("Invalid Name. Engineer must have a name");
        if (engineer.Cost <= 0)
            throw new BO.BlInvalidValuesException("Invalid Engineer Cost. Engineer Cost must be a positive number");
        if (!engineer.Email.EndsWith("@gmail.com"))
            throw new BO.BlInvalidValuesException("Invalid Email Address. Address must end with '@gmail.com'");
        if(_dal.Engineer.ReadAll(e => e.Email == engineer.Email).Count() > 0)
            throw new BO.BlAlreadyExistsException($"{engineer.Email} Already in use");
        try
        {
            _dal.Engineer.Create(convertEngineerFromBlToDal(engineer));
            if (engineer.Task is not null)
                updateEngineerInTask(engineer.Task.Id, engineer.Id);
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with Id {engineer.Id} already exists", ex);
        }
        
    }

    /// <summary>
    /// Deletes Engineer from the Data-base that 1. matches the given ID number 
    /// 2.Engineer isnt currently working on a task and hasnt completed a task in the past
    /// </summary>
    /// <param name="engineer"></param>
    public void Delete(int Id)
    {
        BO.Engineer? myEng = Read(Id);
        if (myEng is null)
            throw new BO.BlDoesNotExistException($"Engineer with Id {Id} does not exist");
        if (myEng.Task is not null)
            throw new BO.BlLogicDenialException($"Engineer with Id {Id} is currently working on task {myEng.Task.Alias}");
        try 
        {
            _dal.Engineer.Delete(Id);
            if (myEng.Task is not null)
            {
                DO.Task task = _dal.Task.Read(myEng.Task.Id)!;
                _dal.Task.Update(task! with { EngineerId = null });
            }

        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with Id {Id} does not exist in the data-base", ex);
        }
    }

    /// <summary>
    /// Search for Engineer in the data-base by ID
    /// </summary>
    /// <param name="Id"></param>
    /// <returns>Engineer that matches the given ID</returns>
    public BO.Engineer? Read(int Id)
    {
        DO.Engineer? myEng = _dal.Engineer.Read(Id);
        if(myEng is null)
            return null;
        return convertEngineerFromDalToBl(myEng);
    }

    /// <summary>
    /// Reads all Engineers from data-base that fill the given condition. read all if no condition is given
    /// </summary>
    /// <param name="filter">Optional delegate filter</param>
    /// <returns>Collection of Engineers that meet the given condition. all if a condition is not given</returns>
    public IEnumerable<BO.Engineer>? ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        var engineers = _dal.Engineer.ReadAll().Select(e => convertEngineerFromDalToBl(e!));
        if (filter is null)
            return engineers;
        return (from e in engineers
                where filter!(e) == true
                select e);
    }

    /// <summary>
    /// Updates Engineer in the data-base according to the values recieved as parmeter
    /// </summary>
    /// <param name="engineer"></param>
    /// <exception cref="BO.BlInvalidValuesException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Engineer engineer)
    {
        if (engineer.Id <= 0)
            throw new BO.BlInvalidValuesException("Invalid Id number. Id must be a positive number");
        if (engineer.Name == "")
            throw new BO.BlInvalidValuesException("Invalid Name. Engineer must have a name");
        if (engineer.Cost <= 0)
            throw new BO.BlInvalidValuesException("Invalid Engineer Cost. Engineer Cost must be a positive number");
        if (!engineer.Email.EndsWith("@gmail.com"))
            throw new BO.BlInvalidValuesException("Invalid Email Address. Address must end with '@gmail.com'");
        if (_dal.Engineer.ReadAll(e => e.Email == engineer.Email).Count() > 1)
            throw new BO.BlAlreadyExistsException($"{engineer.Email} Already in use");
        if (engineer.Task is not null)
        {
            if (_dal.Task.Read(engineer.Task.Id) is not null)
            {
                if (_dal.Task.Read(engineer.Task.Id)?.EngineerId is not null)
                    throw new BO.BlInvalidValuesException($"An engineer is already working on task- {engineer.Task}");
            }
            else
                throw new BO.BlDoesNotExistException($"Task with ID {engineer.Task.Id} does not exist");
        }
            
        try 
        {
            BO.Engineer originalEng = Read(engineer.Id) ?? throw new BO.BlDoesNotExistException($"Engineer with ID {engineer.Id} not found");
            DO.Task? task;
            _dal.Engineer.Update(convertEngineerFromBlToDal(engineer));
            if (originalEng.Task is not null)
            {
                task = _dal.Task.Read(originalEng.Task.Id) ?? throw new BO.BlDoesNotExistException($"Cannot find Task {originalEng.Task}");
                _dal.Task.Update(task! with { EngineerId = null });
            }
            
        }
        catch (DO.DalDoesNotExistException ex) 
        {
            throw new BO.BlDoesNotExistException($"Engineer with Id {engineer.Id} does not exist", ex);
        }
    }

    /// <summary>
    /// Converts the given DO.Engineer to BO.Engineer
    /// </summary>
    /// <param name="dalEngineer"></param>
    /// <returns>Engineer type of the logic layer</returns>
    private BO.Engineer convertEngineerFromDalToBl(DO.Engineer dalEngineer) 
    {
        return new BO.Engineer()
        {
            Id = dalEngineer.Id,
            Name = dalEngineer.Name,
            Email = dalEngineer.Email,
            Level = (BO.EngineerExperience)((int)dalEngineer.Level),
            Cost = dalEngineer.Cost,
            Task = findAssignedTask(dalEngineer.Id)
        };
    }

    /// <summary>
    /// Converts the given BO.Engineer to DO.Engineer.
    /// also, if the given BO.Engineer is assigned to a task, will update the assigned task in the data-base
    /// </summary>
    /// <param name="blEngineer"></param>
    /// <returns>Engineer type of the data layer</returns>
    private DO.Engineer convertEngineerFromBlToDal(BO.Engineer blEngineer)
    {
        try
        {
            if (blEngineer.Task is not null)
                updateEngineerInTask(blEngineer.Task.Id, blEngineer.Id);
        }
        catch 
        {
            throw new BO.BlLogicDenialException($"Failed to to update assigned task of engineer {blEngineer.Id,-10} {blEngineer.Name}");
        }
        return new DO.Engineer() 
        {
            Id = blEngineer.Id,
            Name = blEngineer.Name,
            Email = blEngineer.Email,
            Cost = blEngineer.Cost,
            Level = (DO.EngineerExperience)((int)blEngineer.Level)
        };
    }

    /// <summary>
    /// Finds and returns the task that is assigned to the given engineer 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>BO.TaskInEngineer type of the assigned task</returns>
    private BO.TaskInEngineer? findAssignedTask(int id)
    {
        DO.Task? task = _dal.Task.Read(t => t.EngineerId == id);
        if (task == null)
            return null;
        return new BO.TaskInEngineer() { Id = task.Id, Alias = task.Alias };
    }

    /// <summary>
    /// Updates the task in the data-base to include the assigned engineer Id
    /// </summary>
    /// <param name="taskId">Id of the task to update</param>
    /// <param name="engineerId">Id of the engineer that is assigned to the task</param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    private void updateEngineerInTask(int taskId, int? engineerId)
    {
        DO.Task? task = _dal.Task.Read(taskId);
        if (task == null) 
            throw new BO.BlDoesNotExistException($"Cannot assign task {taskId} since it does not exist");
        _dal.Task.Update(task with {EngineerId = engineerId});

    }
}
