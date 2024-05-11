namespace BlImplementation;
using BlApi;
using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

internal class Bl : IBl
{
    private static TimeSpan _toBeAdded = TimeSpan.Zero;
    private static BackgroundWorker? clockWorker = null;
    private void WorkerReloadClock_DoWork(object sender, DoWorkEventArgs e)
    {
        while (clockWorker!.CancellationPending == false)
        {
            Thread.Sleep(1000);
            clockWorker.ReportProgress(0);
        }
    }
    private void WorkerReloadClock_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        if (!DateControl.GetIsRealTimeClock())
        {
            if (_toBeAdded != TimeSpan.Zero)
            {
                s_Clock += _toBeAdded;
                _toBeAdded = TimeSpan.Zero;
            }
            s_Clock += new TimeSpan(0, 0, 60);     
        }    
        else
            s_Clock = DateTime.Now;
    }

    public Bl()
    {
        //initiating clock background worker
        if (clockWorker is null)
        {
            clockWorker = new BackgroundWorker();
            clockWorker.DoWork += WorkerReloadClock_DoWork;
            clockWorker.ProgressChanged += WorkerReloadClock_ProgressChanged;
            clockWorker.WorkerReportsProgress = true;
            clockWorker.WorkerSupportsCancellation = true;
            clockWorker.RunWorkerAsync();
        }

        _toBeAdded = TimeSpan.Zero;
        if (s_Clock == DateTime.MinValue)
            s_Clock = DateControl.GetCurrentDate();

    }
    public ITask Task => new BlImplementation.TaskImplementation(this);

    public IEngineer Engineer => new BlImplementation.EngineerImplementation(this);

    public IMilestone Milestone => new BlImplementation.MilestoneImplementation(this);

    public IUser User => new BlImplementation.UserImplementation(this);

    public IDatesControl DateControl => new BlImplementation.DatesControlImplementation();


    private static DateTime s_Clock = DateTime.MinValue;
    public DateTime Clock { get { return s_Clock; } }

    public void ClockAddHour() => _toBeAdded += new TimeSpan(1, 0, 0);
    public void ClockAddDay() => _toBeAdded += new TimeSpan(1, 0, 0, 0);
    public void ClockAddMonth() => _toBeAdded += new TimeSpan(30, 0, 0, 0);

    public void SaveClock()=> DateControl.SetCurrentDate(s_Clock);
    
    public void stopClock(object sender, EventArgs e)
    {
        clockWorker.CancelAsync();    
        SaveClock();
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
