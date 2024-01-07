namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        if (item.Id != 0)
            throw new Exception($"Cannot add Engineer with Id {item.Id} since it already exists in the system");
        int newId = DataSource.Config.NextEngineerId;
        Engineer newEngineer = item with { Id = newId, StartDate = DataSource.Config.startDate };
        DataSource.Engineers.Add(newEngineer);
        return newId;
    }

    public void Delete(int id)
    {
        foreach (Engineer? engineer in DataSource.Engineers)
            if (engineer!.Id == id)
            {
                DataSource.Engineer.Remove(engineer);
                return;
            }
        throw new Exception($"Cannot delete Engineer with Id {id} since it does not exist in the system");
    }

    public Engineer? Read(int id)
    {
        foreach (Engineer? engineer in DataSource.Engineers)
            if (engineer!.Id == id)
                return engineer;
        return null;
    }

    public List<Engineer> ReadAll()
    {
        List<DO.Engineer?> myList = new List<DO.Engineer?>(DataSource.Engineers);
        return myList;
    }

    public void Update(Engineer item)
    {
        foreach (Engineer? engineer in DataSource.Engineers)
            if (engineer!.Id == item.Id)
            {
                DataSource.Engineers.Remove(engineer);
                DataSource.Engineers.Add(item);
                return;
            }
        throw new Exception($"Cannot updateEngineer with Id {item.Id} since it does not exist in the system");
    }
}
