namespace DalApi;
using DO;

/// <summary>
/// Interface for Dependency entity
/// </summary>
public interface IDependency : ICrud<Dependency>
{
    Dependency? Read(int depId, int depOnId);//looks for a dependency between the two given task IDs
   
}
