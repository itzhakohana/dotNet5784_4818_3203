namespace Dal;
using DalApi;
using DO;
using System.Security.Cryptography;
using System.Xml.Linq;


/// <summary>
/// Implements Interface for Engineer entity with XML files as the data-base.
/// </summary>
internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";
    private XElement? _engineer_root;

    /// <summary>
    /// Used to convert XElement type to Engineer Type. for internal use only
    /// </summary>
    /// <param name="element"></param>
    /// <returns>Newly created Engineer entity from the given XElement parameter</returns>
    /// <exception cref="FormatException"></exception>
    private Engineer makeEngineer(XElement element) 
    {
        return new Engineer()
        {
            Id = element.ToIntNullable("id")! ?? throw new FormatException("Error converting ID from file. ID was read as null."),
            Level = element.ToEnumNullable<EngineerExperience>("level") ?? 0,
            Name = (string)element.Element("name")!,
            Email = (string)element.Element("email")!,
            Phone = (string?)element.Element("phone"),
            Picture = (string)element.Element("picture")!,
            Cost = element.ToDoubleNullable("cost") ?? 0,
        };
    }

    /// <summary>
    /// Adds the Engineer instance given as parameter to the Xml data-base
    /// </summary>
    /// <param name="item">instance of the Engineer entity to add</param>
    /// <returns>The Id of the newly-added engineer</returns>
    public int Create(Engineer item)
    {

        if (Read(item.Id) is not null)
            throw new DalAlreadyExistException($"An Engineer with Id {item.Id} already exists in the system");
        
        _engineer_root = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        _engineer_root.Add(new XElement("engineer",
                                    new XElement("id", item.Id),
                                    new XElement("level", item.Level),
                                    new XElement("name", item.Name),
                                    new XElement("email", item.Email),
                                    new XElement("phone", item.Phone),
                                    new XElement("picture", item.Picture),
                                    new XElement("cost", item.Cost)
                                    )
                );
        XMLTools.SaveListToXMLElement(_engineer_root!, s_engineers_xml);
        return item.Id;
    }

    /// <summary>
    /// Deletes an Engineer from our Xml data-base by ID
    /// </summary>
    /// <param name="id">Id of the engineer we wish to delete</param>
    /// <exception cref="DalDoesNotExistException">if the given Id does not exist in the data-base</exception>
    public void Delete(int id)
    {
        if (Read(id) == null)
            throw new DalDoesNotExistException($"Cannot delete Engineer with Id {id} since it does not exist in the system");

        _engineer_root = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        (from e in _engineer_root.Elements()
            where (int)e.Element("id")! == id
                select e
                    ).FirstOrDefault()?.Remove();
        XMLTools.SaveListToXMLElement(_engineer_root, s_engineers_xml);        
    }

    /// <summary>
    /// Reads an Engineer entity from the Xml data-base by ID
    /// </summary>
    /// <param name="id">given Id number. used to look for engineer that matches</param>
    /// <returns>The engineer that matches the given ID number</returns>
    public Engineer? Read(int id)
    {
        _engineer_root = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        return (from e in _engineer_root.Elements()
                    where (int)e.Element("id")! == id
                        select makeEngineer(e)).FirstOrDefault(); 
    }

    /// <summary>
    /// Recieves a filter function (predicate). will return the first engineer that gives a match,
    /// null if non matches.
    /// </summary>
    /// <param name="filter">Predicate function. recieves Engineer type as parameter, returns bool</param>
    /// <returns>First Engineer entity in the Xml data-base that matches the given filter function</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        _engineer_root = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        return (from e in _engineer_root.Elements()
                where (filter(makeEngineer(e)))
                    select makeEngineer(e)).FirstOrDefault();

    }

    /// <summary>
    /// Recieves a filter function (predicate). will return all the Engineer entities 
    /// in the Xml data-base that fit the filter function.
    /// if no filter is given, will return all the engineers in the data-base. 
    /// </summary>
    /// <param name="filter">Predicate function. recieves Engineer type as parameter, returns bool</param>
    /// <returns>IEnumerable collection of all Engineer entities that match (fit) the filter, if given.
    /// if not given, returnes all Engineers</returns>
    public IEnumerable<Engineer?>? ReadAll(Func<Engineer, bool>? filter = null)
    {
        _engineer_root = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        if (filter != null) 
        {
            return (from e in _engineer_root.Elements()
                    where (filter(makeEngineer(e)))
                        select makeEngineer(e));
        }

        return (from e in _engineer_root.Elements()
                    select(makeEngineer(e)));
    }

    /// <summary>
    /// Updates the Engineer in the Xml data-base with the values of the engineer recieved as parameter.
    /// ID number needs the match for the update to accure.
    /// </summary>
    /// <param name="item">Engineer type instance for copying its values into an existing Engineer in the data-base</param>
    /// <exception cref="DalDoesNotExistException">no instance of the the given ID number found in the data base</exception>
    public void Update(Engineer item)
    {
        if (Read(item.Id) == null)
            throw new DalDoesNotExistException($"Cannot update Engineer with Id {item.Id} since it does not exist in the system");
        Delete(item.Id);
        Create(item);
    }

    /// <summary>
    /// Deletes all Engineers in the Xml date-base
    /// </summary>
    public void Reset()
    {
        _engineer_root = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        (from e in _engineer_root.Elements()
         select e).Remove();
        XMLTools.SaveListToXMLElement(_engineer_root, s_engineers_xml);
    }
}
