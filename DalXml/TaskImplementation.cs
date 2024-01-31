namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";
    private XElement? _task_root;

    /// <summary>
    /// Used to convert XElement type to Task Type. for internal use only
    /// </summary>
    /// <param name="elem"></param>
    /// <returns>Newly created Task entity from the given XElement parameter</returns>
    /// <exception cref="FormatException"></exception>
    private Task makeTask(XElement elem)
    {
        return new Task()
        {
            Id = elem.ToIntNullable("id")! ?? throw new FormatException("Error converting ID from file. ID was read as null."),
            Alias = (string)elem.Element("alias")!,
            Description = (string)elem.Element("description")!,
            CreatedAtDate = elem.Element("dates")!.ToDateTimeNullable("createdAtDate") ?? throw new FormatException("Error converting Creation-Date from file. Creation-Date was read as null."),
            RequiredEffortTime = (TimeSpan)elem.Element("requiredEffortTime")!,
            Complexity = elem.ToEnumNullable<EngineerExperience>("complexity") ?? throw new FormatException("Error converting Enum entity from file. Task Comlexity was read as null."),
            IsMilestone = (bool)elem.Element("isMilestone")!,
            StartDate = elem.Element("dates")!.ToDateTimeNullable("startDate"),
            ScheduledDate = elem.Element("dates")!.ToDateTimeNullable("scheduledDate"),
            DeadlineDate = elem.Element("dates")!.ToDateTimeNullable("deadlineDate"),
            CompleteDate = elem.Element("dates")!.ToDateTimeNullable("completeDate"),
            Deliverables = (string?)elem.Element("deliverables"),
            Remarks = (string?)elem.Element("remarks"),
            EngineerId = elem.ToIntNullable("engineerId")
        };
    }

    /// <summary>
    /// Adds the given Task Entity to the Xml data-base. assuming all fields are set correctly.
    /// </summary>
    /// <param name="item"></param>
    private void add(Task item) 
    {
        _task_root = XMLTools.LoadListFromXMLElement(s_tasks_xml);
        _task_root.Add(new XElement("task",
                                    new XElement("id", item.Id),
                                    new XElement("alias", item.Alias),
                                    new XElement("description", item.Description),
                                    new XElement("requiredEffortTime", item.RequiredEffortTime),
                                    new XElement("complexity", item.Complexity),
                                    new XElement("isMilestone", item.IsMilestone),
                                    new XElement("dates",
                                        new XElement("createdAtDate", item.CreatedAtDate),
                                        new XElement("startDate", item.StartDate),
                                        new XElement("scheduledDate", item.ScheduledDate),
                                        new XElement("deadlineDate", item.DeadlineDate),
                                        new XElement("completeDate", item.CompleteDate)),
                                    new XElement("deliverables", item.Deliverables),
                                    new XElement("remarks", item.Remarks),
                                    new XElement("engineerId", item.EngineerId)
                                    )
                );
        XMLTools.SaveListToXMLElement(_task_root, s_tasks_xml);
    }

    /// <summary>
    /// Adds the Task instance given as parameter to the Xml data-base
    /// </summary>
    /// <param name="item">instance of the task entity to add</param>
    /// <returns>The Id of the newly-added task</returns>
    public int Create(Task item)
    {

        if (item.Id != 0)
            throw new DalAlreadyExistException($"A Task with Id {item.Id} already exists in the system");

        int newId = Config.NextTaskId;
        add(item with { Id = newId });        
        return newId;
    }

    /// <summary>
    /// Deletes a Task from our Xml data-base by ID
    /// </summary>
    /// <param name="id">Id of the Task we wish to delete</param>
    /// <exception cref="DalDoesNotExistException">if the given Id does not exist in the data-base</exception>
    public void Delete(int id)
    {
        if (Read(id) == null)
            throw new DalDoesNotExistException($"Cannot delete Task with Id {id} since it does not exist in the system");

        _task_root = XMLTools.LoadListFromXMLElement(s_tasks_xml);
        (from e in _task_root.Elements()
            where (int)e.Element("id")! == id
                select e).FirstOrDefault()?.Remove();

        XMLTools.SaveListToXMLElement(_task_root, s_tasks_xml);
    }

    /// <summary>
    /// Reads a Task entity from the Xml data-base by ID
    /// </summary>
    /// <param name="id">given Id number. used to look for a match</param>
    /// <returns>The Task that matches the given ID number</returns>
    public Task? Read(int id)
    {
        _task_root = XMLTools.LoadListFromXMLElement(s_tasks_xml);
        return (from e in _task_root.Elements()
                where (int)e.Element("id")! == id
                    select makeTask(e)).FirstOrDefault();
    }

    /// <summary>
    /// Recieves a filter function (predicate) and returns the first Task that gives a match,
    /// null if non matches.
    /// </summary>
    /// <param name="filter">Predicate function. recieves Task type as parameter, returns bool</param>
    /// <returns>First Task entity in the Xml data-base that matches the given filter function</returns>
    public Task? Read(Func<Task, bool> filter)
    {
        _task_root = XMLTools.LoadListFromXMLElement(s_tasks_xml);
        return (from e in _task_root.Elements()
                where (filter(makeTask(e)))
                    select makeTask(e)).FirstOrDefault();
    }

    /// <summary>
    /// Recieves a filter function (predicate). will return all the Task entities 
    /// in the Xml data-base that fit the filter function.
    /// if no filter is given, will return all the Tasks in the data-base. 
    /// </summary>
    /// <param name="filter">Predicate function. recieves Task type as parameter, returns bool</param>
    /// <returns>IEnumerable collection of all Task entities that match (fit) the filter, if given.
    /// if not given, returnes all Tasks</returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        _task_root = XMLTools.LoadListFromXMLElement(s_tasks_xml);
        if (filter != null)
        {
            return (from e in _task_root.Elements()
                    where (filter(makeTask(e)))
                        select makeTask(e));
        }

        return (from e in _task_root.Elements()
                select (makeTask(e)));
    }

    /// <summary>
    /// Updates the Task in the Xml data-base with the values of the Task recieved as parameter.
    /// ID number needs the match for the update to accure.
    /// </summary>
    /// <param name="item">Task type instance for copying its values into an existing Task in the data-base</param>
    /// <exception cref="DalDoesNotExistException">no instance of the the given ID number found in the data base</exception>
    public void Update(Task item)
    {
        if (Read(item.Id) == null)
            throw new DalDoesNotExistException($"Cannot update Task with Id {item.Id} since it does not exist in the system");
        Delete(item.Id);
        add(item);
    }

    /// <summary>
    /// Deletes all Tasks from the data-base
    /// </summary>
    public void Reset()
    {
        _task_root = XMLTools.LoadListFromXMLElement(s_tasks_xml);
        (from e in _task_root.Elements()
         select e).Remove();
        XMLTools.SaveListToXMLElement(_task_root, s_tasks_xml);
    }
}
