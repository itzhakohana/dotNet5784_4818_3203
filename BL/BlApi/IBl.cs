using BlImplementation;

namespace BlApi;

public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }
    public void InitializeDataBase();
    public void Reset();
    //public DateTime? CurrentTime { set; get; }
}
