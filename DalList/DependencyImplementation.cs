namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        if (item.Id != 0)
            throw new Exception($"Cannot add Dependency with Id {item.Id} since it already exists in the system");
        int newId = DataSource.Config.NextDependencyId;
        Dependency newdependency = item with { Id = newId, StartDate = DataSource.Config.startDate };
        DataSource.Dependencys.Add(newDependency);
        return newId;
    }

    public void Delete(int id)
    {
        foreach (Dependency? dependency in DataSource.Dependencys)
            if (dependency!.Id == id)
            {
                DataSource.Dependency.Remove(dependency);
                return;
            }
        throw new Exception($"Cannot delete Dependency with Id {id} since it does not exist in the system");
    }

    public Dependency? Read(int id)
    {
        foreach (Dependency? dependency in DataSource.Dependencys)
            if (dependency!.Id == id)
                return dependency;
        return null;
    }

    public List<Dependency?> ReadAll()
    {
        List<DO.Dependency?> myList = new List<DO.Dependency?>(DataSource.Dependencys);
        return myList;
    }

    public void Update(Dependency item)
    {
        foreach (Dependency? dependency in DataSource.Dependencys)
            if (dependency!.Id == item.Id)
            {
                DataSource.Dependencys.Remove(dependency);
                DataSource.Dependencys.Add(item);
                return;
            }
        throw new Exception($"Cannot update task with Id {item.Id} since it does not exist in the system");
    }
}
