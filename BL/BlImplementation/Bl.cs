namespace BlImplementation;
using BlApi;
using System;
using System.Reflection.Metadata.Ecma335;

internal class Bl : IBl
{
    public ITask Task => new BlImplementation.TaskImplementation();

    public IEngineer Engineer => new BlImplementation.EngineerImplementation();

    public IMilestone Milestone => new BlImplementation.MilestoneImplementation();

    public IDatesControl DateControl => new BlImplementation.DatesControlImplementation();

    public IUser User => new BlImplementation.UserImplementation();

    public void InitializeDataBase()
    {
        DalTest.Initialization.Do();
    }
    
    public void Reset()
    {
        DalApi.Factory.Get.Reset();
    }
}
