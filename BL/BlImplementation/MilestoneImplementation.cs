namespace BlImplementation;
using BlApi;
using BO;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    /// <summary>
    /// Reads milesonte by ID number
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Milesonte in the data base that match the given id, null if not found</returns>
    public BO.Milestone? Read(int id)
    {
        DO.Task? dalTask = _dal.Task.Read(t => t.Id == id && t.IsMilestone);
        if (dalTask is null)
            return null;
        BO.Task? blTask = s_bl.Task.Read(t => t.Id == id);
        return new BO.Milestone()
        {
            Id = id,
            Description = blTask.Description,
            Alias = blTask.Alias,
            CreatedAtDate = dalTask.CreatedAtDate,
            ForecastDate = blTask.ForecastDate,
            DeadlineDate = blTask.DeadlineDate,
            CompleteDate = blTask.CompleteDate,
            Status = CalculateMilestoneStatus(blTask.Dependencies),
            CompletionPercentage = calculateCompletionPercentage(blTask),
            Remarks = blTask.Remarks,
            Dependencies = blTask.Dependencies,
        };
        
    }
    /// <summary>
    /// Reads Milestone by a given condition
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Milestone that meets the given condition, null if non does</returns>
    public BO.Milestone? Read(Func<BO.Milestone, bool> filter)
    {
        //var tasks = _dal.Task.ReadAll(t => t.IsMilestone);
        return (from t in s_bl.Task.ReadAll(task => _dal.Task.Read(it => task.Id == it.Id && it.IsMilestone) is not null)
                     let temp = new BO.Milestone()
                     {
                         Id = t.Id,
                         Description = t.Description,
                         Alias = t.Alias,
                         CreatedAtDate = t.CreatedAtDate,
                         ForecastDate = t.ForecastDate,
                         DeadlineDate = t.DeadlineDate,
                         CompleteDate = t.CompleteDate,
                         Status = CalculateMilestoneStatus(t.Dependencies),
                         CompletionPercentage = calculateCompletionPercentage(t),
                         Remarks = t.Remarks,
                         Dependencies = t.Dependencies,
                     }
                     where filter(temp)
                     select temp).FirstOrDefault();       
    }
    /// <summary>
    /// Reads all Milestones from data-base that fill the given condition. read all if no condition is given
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>Collection of Milestones that match the filter. all Milestones if filter is not given</returns>
    public IEnumerable<BO.Milestone>? ReadAll(Func<BO.Milestone, bool>? filter = null)
    {
        if (filter is null)
            return (from t in s_bl.Task.ReadAll(t => t.IsMilestone)
                    select new BO.Milestone()
                    {
                        Id = t.Id,
                        Description = t.Description,
                        Alias = t.Alias,
                        CreatedAtDate = t.CreatedAtDate,
                        ForecastDate = t.ForecastDate,
                        DeadlineDate = t.DeadlineDate,
                        CompleteDate = t.CompleteDate,
                        Status = CalculateMilestoneStatus(t.Dependencies),
                        CompletionPercentage = calculateCompletionPercentage(t),
                        Remarks = t.Remarks,
                        Dependencies = t.Dependencies,
                    });
        return (from t in s_bl.Task.ReadAll(t => t.IsMilestone)
                let temp = new BO.Milestone()
                {
                    Id = t.Id,
                    Description = t.Description,
                    Alias = t.Alias,
                    CreatedAtDate = t.CreatedAtDate,
                    ForecastDate = t.ForecastDate,
                    DeadlineDate = t.DeadlineDate,
                    CompleteDate = t.CompleteDate,
                    Status = CalculateMilestoneStatus(t.Dependencies),
                    CompletionPercentage = calculateCompletionPercentage(t),
                    Remarks = t.Remarks,
                    Dependencies = t.Dependencies,
                }
                where filter(temp)
                select temp);
    }
    /// <summary>
    /// Updates the given milestone's alias, description and remarks fields.
    /// will throw exception if milestone is not found
    /// </summary>
    /// <param name="id"></param>
    /// <param name="alias"></param>
    /// <param name="description"></param>
    /// <param name="remarks"></param>
    /// <returns>The newly updated Milestone</returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Milestone Update(int id, string alias, string description, string remarks)
    {
        BO.Task task = s_bl.Task.Read(t => t.Id == id && t.IsMilestone)
            ?? throw new BO.BlDoesNotExistException($"A milestone with ID {id} does not exist");
        task.Description = description;
        task.Alias = alias;
        task.Remarks = remarks;
        s_bl.Task.Update(task);
        return Read(id)!;
    }
    /// <summary>
    /// Called once for setting the project on its way.
    /// will calculate and creat milestones from the dependency list
    /// </summary>
    public void CreatMilestones()
    {
        int i = 0;
        var depGroups = (from dep in _dal.Dependency.ReadAll()
                         group dep by dep.DependentTask into g
                         select g);

        foreach (var group in depGroups)
        {
            foreach (var dep in group)
                group.OrderBy(dep => dep.Id);
        }
        depGroups = (from g in depGroups
                     select g).Distinct();
        //selecting starting tasks
        var firstTasks = (from task in _dal.Task.ReadAll()
                          where _dal.Dependency.Read(d => d.DependentTask == task.Id) is null
                          select task).ToList();
        //selecting final tasks
        var finalTasks = (from task in _dal.Task.ReadAll()
                          where _dal.Dependency.Read(d => d.DependsOnTask == task.Id) is null
                          select task).ToList();
        //reseting dependencies
        _dal.Dependency.Reset();
        //creating milesontes and setting their dependencies
        foreach (var group in depGroups)
        {
            string alias = "M" + i++;
            creat(alias);
            foreach (var task in group)
                _dal.Dependency.Create(new DO.Dependency
                {
                    DependentTask = _dal.Task.Read(t => t.Alias == alias)!.Id,
                    DependsOnTask = task.DependsOnTask
                });
            _dal.Dependency.Create(new DO.Dependency
            {
                DependentTask = group.Key,
                DependsOnTask = _dal.Task.Read(t => t.Alias == alias)!.Id
            });
        }
        creat("start");
        creat("end");
        DO.Task beg = _dal.Task.Read(t => t.Alias == "start")!;
        DO.Task end = _dal.Task.Read(t => t.Alias == "end")!;
        //setting dependencies for the begining tasks
        foreach (var task in firstTasks)
            _dal.Dependency.Create(new DO.Dependency
            {
                DependentTask = task.Id,
                DependsOnTask = beg.Id
            });
        //setting dependencies for the final tasks
        foreach (var task in finalTasks)
            _dal.Dependency.Create(new DO.Dependency
            {
                DependentTask = end.Id,
                DependsOnTask = task.Id
            });

    }
    /// <summary>
    /// Recieves start and end date for the project and creats schedule for 
    /// all of the project's tasks.
    /// if given time-scope is too short, will throw exception
    /// </summary>
    /// <param name="projectStart"></param>
    /// <param name="projectEnd"></param>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public void CreatProjectSchedule(DateTime projectStart, DateTime projectEnd)
    {
        var temp = (from t in _dal.Task.ReadAll(t => t.IsMilestone)
                     where t.Alias[0] == 'M'
                     orderby int.Parse(t.Alias.Substring(1)) descending
                     select t);
        string highestMs = temp.ElementAt(0).Alias.Substring(1);
        int j;
        int i = j = int.Parse(highestMs);
        BO.Task? ms = s_bl.Task.Read(t => _dal.Task.Read(it => t.Id == it.Id && it.IsMilestone && it.Alias == "end") != null)!;
        ms.DeadlineDate = projectEnd;
        s_bl.Task.Update(ms);
        //setting end-date from end to start
        while (i != -2)
        {
            if (i == -1)
                ms = s_bl.Task.Read(t => _dal.Task.Read(it => t.Id == it.Id && it.IsMilestone && it.Alias == "start") != null)!;
            else
                ms = s_bl.Task.Read(t => _dal.Task.Read(it => t.Id == it.Id && it.IsMilestone && it.Alias == "M" + i) != null)
                    ?? throw new BO.BlDoesNotExistException($"Failed to load milestone M{i}") ;
            i--;
            ms.DeadlineDate = projectEnd;
            //all tasks that depend on the milestone (ms)
            foreach (var dep in _dal.Dependency.ReadAll(d => d.DependsOnTask == ms.Id))
            {
                BO.Task t = s_bl.Task.Read(dep!.DependentTask)!;
                t.DeadlineDate = projectEnd;
                if (ms.DeadlineDate > (t.DeadlineDate - t.RequiredEffortTime))
                {
                    ms.DeadlineDate = t.DeadlineDate - t.RequiredEffortTime;
                }
                s_bl.Task.Update(t);
            }
            projectEnd = ms.DeadlineDate.Value;
            s_bl.Task.Update(ms);
        }
        //checking if the project's ending date is valid
        ms = s_bl.Task.Read(t => _dal.Task.Read(it => t.Id == it.Id && it.IsMilestone && it.Alias == "start") != null)!;
        if (projectEnd < projectStart)
        {
            throw new BO.BlLogicViolationException("Please extend the project's given time-scope");
        }
        //updating the first milestone with dates
        ms.DeadlineDate = projectEnd;
        ms.ScheduledDate = projectStart;
        s_bl.Task.Update(ms);

        //now setting schedule start date for the project's tasks
        //from start to end
        DateTime? projectStartTemp = projectStart;
        i = -1;
        while (i != j + 2)
        {
            i++;
            ms.ScheduledDate = projectStart;
            s_bl.Task.Update(ms);
            //all tasks that depend on the milestone (ms)
            foreach (var dep in _dal.Dependency.ReadAll(d => d.DependsOnTask == ms.Id))
            {
                BO.Task t = s_bl.Task.Read(dep!.DependentTask)!;
                t.ScheduledDate = projectStart;
                if (projectStartTemp < (t.ScheduledDate + t.RequiredEffortTime))
                {
                    projectStartTemp = t.ScheduledDate + t.RequiredEffortTime;
                }
                s_bl.Task.Update(t);
            }
            projectStart = projectStartTemp.Value;
            if ( i == j + 1)
                ms = s_bl.Task.Read(t => _dal.Task.Read(it => t.Id == it.Id && it.IsMilestone && it.Alias == "end") != null)!;
            else
                ms = s_bl.Task.Read(t => _dal.Task.Read(it => t.Id == it.Id && it.IsMilestone && it.Alias == "M" + i) != null);
            
        }

        //checking if the project's start date is valid
        ms = s_bl.Task.Read(t => _dal.Task.Read(it => t.Id == it.Id && it.IsMilestone && it.Alias == "end") != null)!;
        if (ms.DeadlineDate < projectStart)
        {
            throw new BO.BlLogicViolationException("Please extend the project's given time-scope");
        }
        //updating the last milestone with the start date
        ms.ScheduledDate = projectStart;
        s_bl.Task.Update(ms);

    }
    /// <summary>
    /// Creats new milestone with the given name string as Alias
    /// </summary>
    /// <param name="name"></param>
    private void creat(string name)
    {

        _dal.Task.Create(new DO.Task
        {
            Alias = name,
            CreatedAtDate = DateTime.Now,
            IsMilestone = true,
            RequiredEffortTime = TimeSpan.Zero,
        });
    }
    /// <summary>
    /// Calculate precentage of completed tasks in the given milestone-task
    /// </summary>
    /// <param name="blTask"></param>
    /// <returns>Precentage of the milestone's completed tasks</returns>
    /// <exception cref="NotImplementedException"></exception>
    private double calculateCompletionPercentage(BO.Task blTask)
    {
        var tasks = (from dep in _dal.Dependency.ReadAll(d => d.DependentTask == blTask.Id)
                          select dep).ToList();
        if (tasks.Count() == 0)
            return 1;
        int comletedTasks = (from dep in tasks
                             let task = s_bl.Task.Read(dep.DependsOnTask)
                             where task.Status == BO.Status.Done
                             select task).Count();
        if (tasks.Count() == 0)
            return 0;
        return (Double)decimal.Divide(comletedTasks, tasks.Count());
    }
    /// <summary>
    /// Deletes milestone by Id
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete (int id) 
    {
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException)
        {

            throw new BO.BlDoesNotExistException($"Milestone with Id {id} not found");
        }
    }
    /// <summary>
    /// Calculates the status of a milestone based on the given list of dependent tasks (prior tasks).
    /// Milestone's status will be done only of all of the prior tasks are done, 
    /// Injeopardy if at least one prior task is Injeopardy.
    /// </summary>
    /// <param name="dependencies"></param>
    /// <returns>Calculated BO.Status based on the given prior tasks</returns>
    public BO.Status CalculateMilestoneStatus(List <BO.TaskInList>? dependencies)
    {
        if (dependencies is null || !dependencies.Any())
            return BO.Status.Done;
        return (dependencies.All(d => d.Status == BO.Status.Done) ? BO.Status.Done :
           (dependencies.Any(d => d.Status == BO.Status.InJeopardy) ? BO.Status.InJeopardy : BO.Status.Scheduled));
    }
    /// <summary>
    /// Starting the preject. will call the relevant functions for creating
    /// milestone and scheduleing all the tasks.
    /// will throw exeption if date given need is too short, or other error occured. 
    /// one SUCCESSFUL call of this function is needed to start the project.
    /// </summary>
    /// <param name="myStartDate"></param>
    /// <param name="myEndDate"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlLogicViolationException"></exception>
    public void StartProject(DateTime myStartDate, DateTime myEndDate)
    {
        if (s_bl.Task.ReadAll() is null || !_dal.Dependency.ReadAll().Any())
            throw new BO.BlDoesNotExistException("No tasks or dependencies detected in the data-base");
        try
        {
            if (s_bl.Milestone.Read(ms => ms.Alias == "start") is null && !s_bl.Task.ProjectHasStarted())
            {
                s_bl.Milestone.CreatMilestones();
            }
            s_bl.Milestone.CreatProjectSchedule(myStartDate, myEndDate);
            s_bl.DateControl.SetProjectSchedule(myStartDate, myEndDate);
        }   
        catch (Exception ex)
        {
            var tasks = s_bl.Task.ReadAll() ?? throw new BO.BlDoesNotExistException("Couldn't load any tasks from the data-base");
            foreach (var task in tasks)
            {
                task.ScheduledDate = null;
                task.DeadlineDate = null;
                s_bl.Task.Update(task);
            }
            throw new BO.BlLogicViolationException("Failed to set schedule for the Project || " + ex.Message);
        }
    }
}
