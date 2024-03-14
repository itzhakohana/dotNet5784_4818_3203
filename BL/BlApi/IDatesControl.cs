namespace BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IDatesControl
{
    public DateTime? GetStartDate();
    public DateTime? GetEndDate();
    public DateTime GetCurrentDate();
    public void SetStartDate(DateTime? start);
    public void SetEndDate(DateTime? end);
    public void SetCurrentDate(DateTime? current);
    public void SetProjectSchedule(DateTime? start, DateTime? end);
    public void ResetAllDates();
}
