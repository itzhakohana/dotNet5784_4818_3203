namespace BlImplementation;
using BlApi;
using System;
using System.Reflection.Metadata.Ecma335;

internal class Bl : IBl
{
    public Bl()
    {
        Clock = DateControl.GetCurrentDate();
        //new Thread(()=> { while (true) runClock(); }).Start();
    }
    public ITask Task => new BlImplementation.TaskImplementation(this);

    public IEngineer Engineer => new BlImplementation.EngineerImplementation(this);

    public IMilestone Milestone => new BlImplementation.MilestoneImplementation(this);

    public IUser User => new BlImplementation.UserImplementation(this);

    public IDatesControl DateControl => new BlImplementation.DatesControlImplementation();


    private static DateTime s_Clock = DateTime.Now.Date;
    public DateTime Clock { get { return s_Clock; } set { s_Clock = value; } }

    public void ClockAddHour() => Clock += new TimeSpan(1, 0, 0);
    public void ClockAddDay() => Clock += new TimeSpan(1, 0, 0, 0);
    public void ClockAddMonth() => Clock += new TimeSpan(30, 0, 0, 0);
    public void SaveClock()=> DateControl.SetCurrentDate(s_Clock);
    private TimeSpan? _toBeAdded = null;
    private void runClock()
    {
        
        if (_toBeAdded != null)
        {
            Clock += _toBeAdded.Value;
            _toBeAdded = null;
        }
        Clock = Clock.AddMinutes(1);
        Thread.Sleep(1000);
        
    }
    public void InitializeDataBase()
    {
        DalTest.Initialization.Do();
    }
    
    public void Reset()
    {
        DalApi.Factory.Get.Reset();
    }
}
