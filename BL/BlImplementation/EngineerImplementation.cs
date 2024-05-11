namespace BlImplementation;
using BlApi;
using BO;
using System.Drawing;
using System.Net.Http.Headers;


/// <summary>
/// Interface implementation of the Logic-layer Engineer entity
/// </summary>
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private readonly BlApi.IBl s_bl;

    internal EngineerImplementation(IBl bl) => s_bl = bl;
    /// <summary>
    /// Adds the given Engineer to the data-base
    /// </summary>
    /// <param name="engineer"></param>
    /// <exception cref="BO.BlInvalidValuesException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public void Add(BO.Engineer engineer)
    {
        try
        {
            engineerFieldsValidation(engineer);
        }
        catch (Exception ex)
        {
            throw new BO.BlInvalidValuesException(ex.Message, ex);
        }
        try
        {
            DO.Task task;
            //configures the assigned task for the new engineer. already checked that the task id is valid
            if (engineer.Task is not null)
            {
                task = _dal.Task.Read(engineer.Task.Id)!;
                engineer.Task = new BO.TaskInEngineer() { Id = task.Id, Alias = task.Alias };
            }

            //creats a new engineer
            _dal.Engineer.Create(convertEngineerFromBlToDal(engineer));
            //if (engineer.Task is not null)
            //    updateEngineerInTask(engineer.Task.Id, engineer.Id);
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with Id {engineer.Id} already exists", ex);
        }

    }
    /// <summary>
    /// Verifies that all the fields of the given BO.Engineer are valid
    /// </summary>
    /// <param name="engineer"></param>
    /// <exception cref="BO.BlInvalidValuesException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    private void engineerFieldsValidation(BO.Engineer engineer)
    {
        if (engineer.Id <= 0)
            throw new BO.BlInvalidValuesException("Invalid Id number. Id must be a positive number");
        if (engineer.Name == "")
            throw new BO.BlInvalidValuesException("Invalid Name. Engineer must have a name");
        if (engineer.Cost <= 0)
            throw new BO.BlInvalidValuesException("Invalid Engineer Cost. Engineer Cost must be a positive number");
        if (!engineer.Email.EndsWith("@gmail.com") && !engineer.Email.EndsWith("@gmail.co.il"))
            throw new BO.BlInvalidValuesException("Illegal Email Address. Address must end with '@gmail.com'/'@gmail.co.il'");
        if (engineer.Phone != null && engineer.Phone != "" && engineer.Phone.Length != 10)
            throw new BO.BlInvalidValuesException("Illegal Phone Number");
        if (_dal.Engineer.ReadAll(e => e.Id != engineer.Id && e.Email == engineer.Email).Count() > 0)
            throw new BO.BlAlreadyExistsException($"{engineer.Email} Already in use");
        try
        {
            verifyAssignedTaskField(engineer);
        }
        catch (Exception ex)
        {
            throw new BO.BlInvalidValuesException(ex.Message, ex);
        }
    }
    /// <summary>
    /// Deletes Engineer by given Id number
    /// </summary>
    /// <param name="Id"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public void Delete(int Id)
    {
        BO.Engineer? myEng = Read(Id);
        if (myEng is null)
            throw new BO.BlDoesNotExistException($"Engineer with Id {Id} does not exist");
        if (myEng.Task is not null)
            throw new BO.BlLogicViolationException($"Engineer with Id {Id} is currently working on task {myEng.Task.Alias}");
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
    /// <returns>BO.Engineer if found, null if not found</returns>
    public BO.Engineer? Read(int Id)
    {
        DO.Engineer? myEng = _dal.Engineer.Read(Id);
        if(myEng is null)
            return null;
        return convertEngineerFromDalToBl(myEng);
    }
    /// <summary>
    /// Searches for engineer in the data base by filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>The found BO.Engineer type, null if not found</returns>
    public BO.Engineer? Read(Func<BO.Engineer, bool> filter)
    {
        return (from eng in _dal.Engineer.ReadAll()
                select convertEngineerFromDalToBl(eng)
                into eng
                where filter(eng)
                select eng).FirstOrDefault();
    }
    /// <summary>
    /// Gives all Engineers from data-base that fill the given condition. 
    /// gives all if no condition is given
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Collection of BO.Engineer types that match the given filter.
    /// if no filter is given, returns all engineers</returns>
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
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Engineer engineer)
    {
        try
        {
            engineerFieldsValidation(engineer);
        }
        catch (Exception ex)
        {
            throw new BO.BlInvalidValuesException(ex.Message, ex);
        }

        try
        {
            DO.Task? task;
            //searches for the current engineer
            BO.Engineer originalEng = Read(engineer.Id) 
                ?? throw new BO.BlDoesNotExistException($"Engineer with ID {engineer.Id} not found");
            //configures the assigned task for the updated engineer. already checked that the task id is valid
            if (engineer.Task is not null)
            { 
                task = _dal.Task.Read(engineer.Task.Id)!;
                engineer.Task = new BO.TaskInEngineer() { Id = task.Id, Alias = task.Alias };
                if(!s_bl.Task.ProjectHasStarted())
                {
                    if (originalEng.Task is not null)
                    {
                        var t = s_bl.Task.Read(originalEng.Task.Id);
                        t.Engineer = null;
                        s_bl.Task.Update(t);
                    }
                }
            }

            //else 
            //    //project has started, not allowed to change assigned task
            //    if (s_bl.Task.ProjectHasStarted())
            //        engineer.Task = originalEng.Task;

            //finally perform the updates on the engineer
            _dal.Engineer.Update(convertEngineerFromBlToDal(engineer));

        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with Id {engineer.Id} does not exist", ex);
        }
        catch (Exception ex)
        {
            throw new BO.BlDoesNotExistException($"Error updating engineer, {ex}");
        }
    }
    /// <summary>
    /// Converts the given DO.Engineer to BO.Engineer
    /// </summary>
    /// <param name="dalEngineer"></param>
    /// <returns>BO.Engineer from the given DO.Engineer</returns>
    private BO.Engineer convertEngineerFromDalToBl(DO.Engineer dalEngineer) 
    {
        return new BO.Engineer()
        {
            Id = dalEngineer.Id,
            Name = dalEngineer.Name,
            Email = dalEngineer.Email,
            Phone = dalEngineer.Phone,
            Level = (BO.EngineerExperience)((int)dalEngineer.Level),
            Cost = dalEngineer.Cost,
            Picture = dalEngineer.Picture,
            Task = findAssignedTask(dalEngineer.Id)
        };
    }
    /// <summary>
    /// Converts the given BO.Engineer to DO.Engineer.
    /// also, if the given BO.Engineer is assigned to a task, will update the assigned task in the data-base
    /// </summary>
    /// <param name="blEngineer"></param>
    /// <returns>DO.Engineer from the given BO.Engineer</returns>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    private DO.Engineer convertEngineerFromBlToDal(BO.Engineer blEngineer)
    {
        try
        {
            if (blEngineer.Task is not null)
                updateEngineerInTask(blEngineer.Task.Id, blEngineer.Id);            
            else
            {
                var eng = s_bl.Engineer.Read(blEngineer.Id);
                if (eng != null && eng.Task != null)
                {
                    var t = s_bl.Task.Read(eng.Task.Id);
                    t.Engineer = null;
                    s_bl.Task.Update(t);
                }
            }
        }
        catch 
        {
            throw new BO.BlLogicViolationException($"Failed to to update assigned task of engineer {blEngineer.Id,-10} {blEngineer.Name}");
        }
        return new DO.Engineer() 
        {
            Id = blEngineer.Id,
            Name = blEngineer.Name,
            Email = blEngineer.Email,
            Phone = blEngineer.Phone,
            Cost = blEngineer.Cost,
            Picture = blEngineer.Picture,
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
        DO.Task? task = _dal.Task.Read(t => t.EngineerId == id && t.CompleteDate is null);
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
        DO.Task task = _dal.Task.Read(taskId) 
            ?? throw new BO.BlDoesNotExistException($"Cannot assign task {taskId} since it does not exist"); ;
        _dal.Task.Update(task with {EngineerId = engineerId});

    }
    /// <summary>
    /// Verifies the task that is assigned to the given engineer. 
    /// if the assigned task does not exist, or another engineer is already working on it,
    /// than will throw relevant exeption. if task is valid and availble, will do nothing.
    /// </summary>
    /// <param name="engineer"></param>
    /// <exception cref="BO.BlInvalidValuesException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    private void verifyAssignedTaskField(BO.Engineer engineer)
    {
        if (engineer.Task is not null)
        {
            if (_dal.Task.Read(engineer.Task.Id) is not null)
            {
                //Task is already being worked on by different engineer
                if (_dal.Task.Read(engineer.Task.Id)!.EngineerId is not null)
                    if (_dal.Task.Read(engineer.Task.Id)!.EngineerId != engineer.Id)
                        throw new BO.BlInvalidValuesException($"A different engineer is already working on task- {engineer.Task}");
                if (s_bl.Task.ProjectHasStarted())
                    //the engineer is already assigned to a different task
                    if (_dal.Task.ReadAll(t => t.EngineerId == engineer.Id && t.Id != engineer.Task.Id && t.CompleteDate is null).Any())
                        throw new BO.BlLogicViolationException($"Engineer with ID {engineer.Id} is already assigned to a different task");
                //task is too complex
                if(_dal.Task.Read(engineer.Task.Id)!.Complexity > (DO.EngineerExperience)engineer.Level)
                    throw new BO.BlLogicViolationException($"Task {engineer.Task.Id} is too complex for this engineer" );
            }
            else
                throw new BO.BlDoesNotExistException($"Task with ID {engineer.Task.Id} does not exist");
        }
        else
        {
            //if the engineer already started working on the task
            var originalEngineer = s_bl.Engineer.Read(engineer.Id);
            if (originalEngineer != null && originalEngineer.Task != null)
            {
                var originalTask = s_bl.Task.Read(originalEngineer.Task.Id);
                if (originalTask != null && originalTask.Status == Status.OnTrack)
                    throw new BO.BlLogicViolationException($"Engineer with ID {engineer.Id} is currently working on a Task");
            }
        }
    }
    /// <summary>
    /// Deletes all existing Engineers
    /// </summary>
    public void Reset()
    {
        _dal.Engineer.Reset();
    }
    /// <summary>
    /// Updates the given engineer's assigned task progress.
    /// if engineer has'nt yet started to work on his task, will set task status to "on track"(engineer starts to work).
    /// if engineer already started working on his task, will set task as complete.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="currentTime"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public void UpdateEngineerProgress(int id, DateTime currentTime)
    {

        //cant be null. otherwise an exception was already thrown from 'printEngineerAssignedTask'
        BO.Engineer eng = s_bl.Engineer.Read(id)
            ??throw new BO.BlDoesNotExistException($"Engineer with Id {id} does not exist");
        if (eng.Task is null)
            throw new BO.BlLogicViolationException($"Engineer with Id {id} is not working on any task");
        BO.Task task = s_bl.Task.Read(t => t.Id == eng.Task!.Id && t.Status != BO.Status.Done) 
            ??throw new BO.BlDoesNotExistException($"Engineer with Id {id} is not currently working on a task");
        if (task.StartDate is null)
            task.StartDate = currentTime;
        else
        {
            task.CompleteDate = currentTime;
        }
        s_bl.Task.Update(task);
    }
    /// <summary>
    /// Returns the given engineer's assigned task.
    /// will only return tasks which are not yet comleted (status != Done)
    /// </summary>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Task ReadEngineerAssignedTask(int id )
    {
        BO.Engineer engineer = Read(eng => eng.Id == id)
            ?? throw new BO.BlDoesNotExistException($"Engineer with id {id} does not exist");
        if (engineer.Task is null)
            throw new BO.BlDoesNotExistException($"Engineer with id {id} does not have any assigned tasks");
        return s_bl.Task.Read(engineer.Task.Id)!;
    }
    /// <summary>
    /// Reads all the engineers that can be assigned to the given task (given by id).
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Collection of BO.EngineerInTask of engineers available for assignment</returns>
    public IEnumerable<EngineerInTask>? ReadEngineersInTask(int id)
    {
        var task = s_bl.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Task with Id {id} not exist");
        return (from e in ReadAll()
                where e.Task is null && (e.Level >= task.Complexity)
                select new BO.EngineerInTask() { Id = e.Id, Name = e.Name });

    }
    /// <summary>
    /// Reads all the Engineers that can be assigned to a task with the given comlexity.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Collection of BO.EngineerInTask that match the given task comlexity</returns>
    public IEnumerable<BO.EngineerInTask>? ReadEngineersInTask(BO.EngineerExperience level) 
    {        
        return (from e in ReadAll()
                where e.Task is null && (e.Level >= level)
                select new BO.EngineerInTask() { Id = e.Id, Name = e.Name });
    }
}
