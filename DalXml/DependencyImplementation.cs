namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Security.Cryptography;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencies_xml = "dependencies";
    private XElement? _dependency_root;

    /// <summary>
    /// Adds the Dependency instance given as parameter to the Xml data-base
    /// </summary>
    /// <param name="item">instance of the Dependency entity to add</param>
    /// <returns>The Id of the newly-added Dependency</returns>
    public int Create(Dependency item)
    {
        if (Read(item.Id) is not null)
            throw new DalAlreadyExistException($"Dependency with Id {item.Id} already exists in the system");

        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        int newId = Config.NextDependencyId;
        dependencies.Add(item with { Id = newId });
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependencies_xml);
        return item.Id;
    }

    /// <summary>
    /// Deletes A Dependency from our Xml data-base by ID
    /// </summary>
    /// <param name="id">Id of the Dependency we wish to delete</param>
    /// <exception cref="DalDoesNotExistException">if the given Id does not exist in the data-base</exception>
    public void Delete(int id)
    {
        if (Read(id) == null)
            throw new DalDoesNotExistException($"Cannot delete Dependency with Id {id} since it does not exist in the system");

        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        dependencies.Remove(dependencies.FirstOrDefault(dep => dep.Id == id)!);
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependencies_xml);
    }

    /// <summary>
    /// Looks for a dependency between the two given task IDs
    /// </summary>
    /// <param name="depId"></param>
    /// <param name="depOnId"></param>
    /// <returns>The found Dependency. null if no such Dependency exists</returns>
    public Dependency? Read(int depId, int depOnId)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        return (from d in  dependencies
                where ((d.DependentTask == depId && d.DependsOnTask == depOnId) || (d.DependentTask == depOnId && d.DependsOnTask == depId))      
                select d).FirstOrDefault();
    }

    /// <summary>
    /// Reads Dependency entity from the Xml data-base by ID
    /// </summary>
    /// <param name="id">given Id number. used to look for Dependency that matches</param>
    /// <returns>The Dependency that matches the given ID number</returns>
    public Dependency? Read(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        return dependencies.FirstOrDefault(dep => dep.Id == id);
    }

    /// <summary>
    /// Recieves a filter function (predicate). will return the first Dependency that gives a match,
    /// null if non matches.
    /// </summary>
    /// <param name="filter">Predicate function. recieves Dependency type as parameter, returns bool</param>
    /// <returns>First Dependency entity in the Xml data-base that matches the given filter function</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        return dependencies.FirstOrDefault(dep => filter(dep));
    }

    /// <summary>
    /// Recieves a filter function (predicate). will return all the Dependency entities 
    /// in the Xml data-base that fit the filter function.
    /// if no filter is given, will return all Dependencies in the data-base. 
    /// </summary>
    /// <param name="filter">Predicate function. recieves Dependency type as parameter, returns bool</param>
    /// <returns>IEnumerable collection of all Dependency entities that match (fit) the filter, if given.
    /// if not given, returnes all Dependencies in the data-base</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        if (filter != null)
        {
            return dependencies.Where(item => filter!(item!));
        }
        return new List<Dependency>(dependencies);
    }

    /// <summary>
    /// Updates the Dependency in the Xml data-base with the values of the Dependency recieved as parameter.
    /// ID number needs to match for the update to accure.
    /// </summary>
    /// <param name="item">Dependency type instance for copying its values into an existing Dependency in the data-base</param>
    /// <exception cref="DalDoesNotExistException">No instance of the the given ID number found in the data base</exception>
    public void Update(Dependency item)
    {
        Dependency? dep = Read(item.Id);
        if (dep == null)
            throw new DalDoesNotExistException($"Cannot update Dependency with Id {item.Id} since it does not exist in the system");
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencies_xml);
        dependencies.Remove(dep);
        dependencies.Add(item);
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependencies_xml);
    }

    /// <summary>
    /// Deletes all Dependencies in the Xml data-base
    /// </summary>
    public void Reset()
    {
        _dependency_root = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        (from e in _dependency_root.Elements()
         select e).Remove();
        XMLTools.SaveListToXMLElement(_dependency_root, s_dependencies_xml);
    }
}
    