namespace BlImplementation;
using BlApi;
internal class Bl : IBl
{
    public ITask Task => new BlImplementation.TaskImplementation();

    public IEngineer Engineer => new BlImplementation.EngineerImplementation();

    //public IMilestone Milestone => throw new NotImplementedException();
}
