namespace PL;

using BO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

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
        return (string)value != null ? ((string)value).ElementAt(0) : 'D';
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
                    return (string)"Project has not Yet Started\nCannot start work";
                case BO.Status.Scheduled:                    
                    return (string)$"Task is scheduled for:\n{task.ScheduledDate!.Value.ToShortDateString()}";
                case BO.Status.OnTrack:
                    return (string)$"Task is in-progress\nMust be finished by:\n{task.DeadlineDate!.Value.ToShortDateString()}";
                case BO.Status.InJeopardy:
                    return (string)$"Task is exceeding schedule\nDeadline was:\n{task.DeadlineDate!.Value.ToShortDateString()}";
                case BO.Status.Done:
                    return (string)$"Task completed on:\n{task.CompleteDate!.Value.ToShortDateString()}";
            }
        }                
        return (string)"No Task Found";
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

