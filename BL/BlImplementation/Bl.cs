namespace BlImplementation;
using BlApi;
using System;
using System.Reflection.Metadata.Ecma335;

internal class Bl : IBl
{
    public Bl()
    {
        if(s_Clock == DateTime.MinValue)
            s_Clock = DateControl.GetCurrentDate();
        //new Thread(()=> { while (true) runClock(); }).Start();
    }
    public ITask Task => new BlImplementation.TaskImplementation(this);

    public IEngineer Engineer => new BlImplementation.EngineerImplementation(this);

    public IMilestone Milestone => new BlImplementation.MilestoneImplementation(this);

    public IUser User => new BlImplementation.UserImplementation(this);

    public IDatesControl DateControl => new BlImplementation.DatesControlImplementation();


    private static DateTime s_Clock = DateTime.MinValue;
    public DateTime Clock { get { return s_Clock; } }

    public void ClockAddHour() => s_Clock += new TimeSpan(1, 0, 0);
    public void ClockAddDay() => s_Clock += new TimeSpan(1, 0, 0, 0);
    public void ClockAddMonth() => s_Clock += new TimeSpan(30, 0, 0, 0);
    public void SaveClock()=> DateControl.SetCurrentDate(s_Clock);
    private TimeSpan? _toBeAdded = null;
    private void runClock()
    {
        
        if (_toBeAdded != null)
        {
            s_Clock += _toBeAdded.Value;
            _toBeAdded = null;
        }
        s_Clock = Clock.AddMinutes(1);
        Thread.Sleep(1000);
        
    }
    public void stopClock()
    {

    }
    public void InitializeDataBase()
    {
        DalTest.Initialization.Do();
    }
    
    public void Reset()
    {
        DalApi.Factory.Get.Reset();
        s_Clock = DateTime.Now;
    }

}
