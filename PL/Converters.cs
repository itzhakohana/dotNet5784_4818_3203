namespace PL;

using Microsoft.Win32;
using PL.TaskPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using BO;
using System.Globalization;
using BlApi;
using DO;
using PL.CustomControls;

internal static class GetBlAccess
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static internal BlApi.IBl getBl { get { return s_bl; } }
}
internal class ConvertEngIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //return (((BO.Engineer)value).Id == 0) ? "Add" : "Update";
        return ((int)value == 0) ? "Add" : "Save";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


internal class ConvertTaskInEngToIsEnabled : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((BO.TaskInEngineer)value == null) ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((bool)value == true ? new BO.TaskInEngineer() {Id = 0, Alias = "" } : null);
    }
}

internal class ConvertIdToIsEnabled : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((int)value == 0) ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertBoolToVisibility: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((bool)value == false) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertEngineerTaskToVisiblity : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((BO.TaskInEngineer)value == null) ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertUserTypeToVisiblity : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((BO.UserType)value == BO.UserType.Admin) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertUserTypeToIsEnabled : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((BO.UserType)value == BO.UserType.Admin) ? true : false;
        //return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertNullToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value == null) ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertNullEngineerInTaskToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value == null) ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((bool)value == true) ? new BO.EngineerInTask() : null;
    }
}

internal class ConvertNullTaskInEngineerToBool : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value == null) ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((bool)value == true) ? new BO.TaskInEngineer() : null;
    }
}

internal class ConvertStatusToNumber : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((BO.Status)value == BO.Status.Unscheduled) return 1;
        if ((BO.Status)value == BO.Status.Scheduled) return 2;
        if ((BO.Status)value == BO.Status.OnTrack) return 3;
        if ((BO.Status)value == BO.Status.InJeopardy) return 4;
        else return 5;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertLevelToNumber : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((BO.EngineerExperience)value == BO.EngineerExperience.Beginner) return 1;
        if ((BO.EngineerExperience)value == BO.EngineerExperience.AdvancedBeginner) return 2;
        if ((BO.EngineerExperience)value == BO.EngineerExperience.Intermediate) return 3;
        if ((BO.EngineerExperience)value == BO.EngineerExperience.Advanced) return 4;
        else return 5;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertUserTypeToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.UserType) value == BO.UserType.Admin? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertUserNameToChar : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {        
        return (string)value != null ? char.ToUpper(((string)value).ElementAt(0)) : 'D';
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertUTaskProgressToStringMessage : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var task = (BO.Task)value;
        if (task != null)
            return (task.Status == BO.Status.OnTrack || (task.Status == BO.Status.InJeopardy && task.StartDate != null) ? ((string)"Complete Task") : ((string)"Start Work"));
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertTaskToStatusMessage : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Task task = (BO.Task)value;
        if (task != null)
        {
            switch (task.Status) 
            {
                case BO.Status.Unscheduled:
                    return (string)"Project has not Yet Started. Cannot start work";
                case BO.Status.Scheduled:                    
                    return (string)$"Task is scheduled for: {task.ScheduledDate!.Value.ToShortDateString()}";
                case BO.Status.OnTrack:
                    return (string)$"Task is in-progress Must be finished by: {task.DeadlineDate!.Value.ToShortDateString()} ({(task.DeadlineDate - GetBlAccess.getBl.Clock).GetValueOrDefault().Days} Days left)";
                case BO.Status.InJeopardy:
                    return (string)$"Task is exceeding schedule by {(GetBlAccess.getBl.Clock - task.DeadlineDate).GetValueOrDefault().Days} Days. Deadline was: {task.DeadlineDate!.Value.ToShortDateString()}. you must finish work asp or contact the admin!";
                case BO.Status.Done:
                    return (string)$"Task completed on: {task.CompleteDate!.Value.ToShortDateString()}";
            }
        }                
        return (string)"You have no assigned task";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertTaskStatusToBackgroundColor : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Task? task = (BO.Task)value;
        if (task != null)
        {
            switch (task.Status)
            {
                case BO.Status.Unscheduled:
                    return Brushes.DarkBlue;
                case BO.Status.Scheduled:
                    return Brushes.LightSeaGreen;
                case BO.Status.OnTrack:
                    return Brushes.Green;
                case BO.Status.InJeopardy:
                    return Brushes.OrangeRed;
                case BO.Status.Done:
                    return (string)$"Task completed on:\n{task.CompleteDate!.Value.ToShortDateString()}";
            }
        }
        return Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


internal class ConvertDependenciesListToListCount : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((IEnumerable<TaskInList>?)value != null)
            return ((IEnumerable<TaskInList>?)value)!.ToList().Count();
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertNullDateTimeToString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((DateTime?)value == null)
            return (string)"N/A";
        return ((DateTime?)value).ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertTimeSpanToDaysOnlyString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((TimeSpan?)value == null)
            return 0;
        return (int)(((TimeSpan?)value).Value.TotalDays);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((string)value == null)
            return new TimeSpan(0,0,0,0);
        int tmp;
        if (!(int.TryParse(((string)value), out tmp)))
            tmp = 0;
        return new TimeSpan(tmp, 0, 0, 0);
    }
}


internal class ConvertUserAndProjectStateToBool : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if(values.Length == 2 && values[0] is BO.UserType type && values[1] is bool projectStatus)
        {
            if (type == BO.UserType.Admin && projectStatus == false)
                return (bool)true;           
        }
        return (bool)false;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertNullToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is null)
            return Visibility.Collapsed;
        else 
            return Visibility.Visible; 
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertBytesToImage : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string myImage)
        {
            if (myImage is null || myImage == "")
                myImage = Util.DEFAULT_PROFILE_PICTURE;
            return Util.BytesToImage(System.Convert.FromBase64String(myImage));
        }
        return null;        
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConverImageListItemToBitMapImage : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ImageListItem item)
        {
            BitmapImage bitMap = new BitmapImage();
            if (item is null)
                bitMap = Util.BytesToImage(System.Convert.FromBase64String(Util.DEFAULT_PROFILE_PICTURE));
            else
                bitMap = Util.ConvertToBitmapImage(item.Image);
            return bitMap;
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertTaskToGanttTaskRecWidth : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int taskId)
        {
            BO.Task? task = GetBlAccess.getBl.Task.Read(taskId);
            if ((task != null && task.ForecastDate != null && task.ScheduledDate != null))
                return (int)((task.ForecastDate - task.ScheduledDate).Value.TotalDays) * 25;
        }
        return 20;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertTaskToGanttTaskRecMargin : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int taskId)
        {
            BO.Task? task = GetBlAccess.getBl.Task.Read(taskId);
            if (task != null && task.ScheduledDate != null && GetBlAccess.getBl.DateControl.GetStartDate() != null)
                return new Thickness((((int)(task.ScheduledDate - GetBlAccess.getBl.DateControl.GetStartDate()).Value.TotalDays) * 25), 0, 0, 0);
        }
        return new Thickness(0, 0, 0, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertTaskToGanttTaskRecBackground : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int taskId)
        {
            BO.Task? task = GetBlAccess.getBl.Task.Read(taskId);
            if (task != null )
                switch (task.Status)
                {
                    case BO.Status.Unscheduled:
                        return Brushes.DarkBlue;
                    case BO.Status.Scheduled:
                        return Brushes.LightSeaGreen;
                    case BO.Status.OnTrack:
                        return Brushes.Green;
                    case BO.Status.InJeopardy:
                        return Brushes.OrangeRed;
                    case BO.Status.Done:
                        return Brushes.DarkGray;
                }
        }
        return Brushes.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


internal class ConvertUserTypeToNumber : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((BO.UserType)value == BO.UserType.Engineer) return 1;
        else if ((BO.UserType)value == BO.UserType.Admin) return 2;
        else return 1;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertLastLoginDateToString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime myDate && GetBlAccess.getBl.Clock >= myDate)
        {            
            TimeSpan timePassed = GetBlAccess.getBl.Clock - myDate;
            return new string($"{timePassed.Days} Days & {timePassed.Hours} Hours ago");
        }
        else 
            return new string(@"N/A");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertEngineerInUserToString : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is BO.Engineer myEng)
        {            
            return new string($"{myEng.Name}");
        }
        else 
            return new string("--------");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


internal class ConvertUserAndEngineerToVisibility : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 && values[0] is BO.User user && values[1] is BO.Engineer eng)
        {
            if (user.Engineer != null && user.Engineer.Id == eng.Id)
                return Visibility.Visible;
            if (user.UserType == BO.UserType.Admin)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

internal class ConvertUserAndEngineerToIsEnabled : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 && values[0] is BO.User user && values[1] is BO.Engineer eng)
        {
            if (user.Engineer != null && user.Engineer.Id == eng.Id)
                return (bool)true;
            if (user.UserType == BO.UserType.Admin)
                return (bool)true;
            return (bool)false;
        }
        return (bool)false;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}