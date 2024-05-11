namespace BlImplementation;

using BlApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



internal class DatesControlImplementation : IDatesControl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

   
    public DateTime? GetStartDate()
    {
        return _dal.ReadStartDate();
    }

    public DateTime? GetEndDate()
    {
        return _dal.ReadEndDate();
    }

    public DateTime GetCurrentDate()
    {
        return _dal.ReadCurrentTime() ?? DateTime.Now;
    }

    public void SetStartDate(DateTime? start)
    {
        _dal.SetProjectSchedule(start, _dal.ReadEndDate());
    }

    public void SetEndDate(DateTime? end)
    {
        _dal.SetProjectSchedule(_dal.ReadStartDate(), end);
    }

    public void SetCurrentDate(DateTime? current)
    {
        _dal.SetCurrentTime(current);
    }

    public void SetProjectSchedule(DateTime? start, DateTime? end)
    {
        _dal.SetProjectSchedule(start, end);        
    }

    public void ResetAllDates()
    {
        _dal.SetProjectSchedule(null, null);
        _dal.SetCurrentTime(null);
    }

    public bool GetIsRealTimeClock()
    {
        return _dal.IsRealTimeClock;
    }

    public void setIsRealTimeClock(bool value)
    {
        _dal.IsRealTimeClock = value;
    }
}
