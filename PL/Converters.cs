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
                myImage = "iVBORw0KGgoAAAANSUhEUgAAAgAAAAIACAYAAAD0eNT6AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAjqAAAI6gAW78HIYAAEt + SURBVHhe7d0JYBXVvfjxc + berJBAEgKEXRERkN0dF1xaF9S6EHADAiq2trWtfXa1MmgXW7s9bW3lqeBaJbjv1iUqrqCACuLCvi8hQMieO + edyT2uBEjCXWbufD / vf5nf75fw / s9wc + c3Z86cIwUATzv9hzdndNi1s7OSTr5qVPkqpAqUkAWWUgWOEnnSErlCifb6W7ObjlLnQmTpVzv372sd9MuKhk219Gj4DUrs0n + 3wWS1 + lWja46u7YiWmo479atS6aP + 8KiUQm7Xf3GH / qZyfdwSComtTkRs3dJHbC2z7camvwXAk2gAgCQ6d + LvC9JD9b2diOytT9G9lFI9dLmrlKLIPZpXgX750Xb9CbNBNxHr9X / POt00rJGO3KCEWmNZYq1jiZWld9jbzPcCSDAaACC + ZPFEu6eSsp++Uj9YOaqf / qXrp + t99G9fb33Mafqu4Nqhfw7LdZOwQil9lGKFFPLTkAwtfWDmtWvM9wCIAxoAIDbk + VPsA6SSgy2lBumr3SG6doh + Haxf7nA8Wsu9JSHEUv2zXGpJ8ZFy5EdOWC166E57RdNXAewXGgCgldx78rmV24co4Yx0hBihf4mG6tPRQP3b5N6HR / xV6tcH + me + SFlyUUipRU5V7qLS0qtrol8G0BI0AMBeFBfPDjlZSwaFLHGMPuGMVJYYqY + H6i + lRb8DHtGgP80 + VEq8bQkxT4bEO87OgR + Vlo6LmK8D + AYaAOAriqfe2EHU1x6tfzOOFo4 + 6UtxpC4H / T69P0Wfanhbf8jNFZaYqzLFW6W32u5tBQAaDQAC7ZwSu2O6kMc5Sp0oLTFanzSG6vLnj8whlSjhPpa4UErdEAjxSr0QZY / Osrc3fQ0IIBoABEpx8V + zRLudJ + iTwClKiRN1yT3hh5q + iKBxbw + 8q18v6ebvRVWZ + zrzCBAkNABIeeMn3DBAhZ3TlFKn6vR4 / WJWPppTK5SYKy35tJLW06V3 / uZjUwdSEg0AUo47S7995bYT9Yf52fodPkaXekW / ArSKuz7B00rJp2VNThmjA0g1NABICcWX2vkqIk + WQp2l07P1y13 + FogVd1nkF / XxiUZHPPrIPfbmaBnwLxoA + Na5E + zOaSFxntLnf52eoF / cy0ciuPslvKw / PB9pbEx77OF7f70hWgb8hQYAvuKunR + S9WP0G7dYv3vde / o8j49kcvTrTf1RWhoOhx / 4z + 2 / 3hQtA95HAwDPK77Sbi + q5blCqEt0erJ + caUPL3IXI3pBCvlgWr165L77bHfnRMCzaADgSbZtW4tXN62 + N0FfY12k36kssws / qdUfrk / oj9h7NvdRz7A1MryIBgCeUjzlhv7CiVyqw4n61aWpCPibuxXyvVKKu0tn2ktMDUg6GgAk3VlT7eyMOjFWf0BeptPjolUgJb2lpJxRl6YefGKGXW1qQFLQACBpzp9oD7Gk + K5 + F16kUx7bQ5Ds0B++90nLmvHgndctMjUgoWgAkFDFxXa6aifO02 + 8K3XK1T4gxNtKiVurcvMffOaWq + pMDYg7GgAkxHmX2T1CEX21r4R7f79rtArgKzbpT + TbIg1p / 2ZtASQCDQDi6vwp9mEyIq6WUozVKc / sA / tWr19zHGnd / NDM696OloDYowFAzLmP8C1ZLU5SEfEj / Q4705QBtN67SombZfXA + 0pLx7m7FwIxQwOAmHE34Wm3q6JEKvVTnfaLVgHEwFIlxV + q2uffwzwBxAoNAPabu1KfqpJXSKmu1mm3aBVAHGyUQv691sq89fE7f15pakCb0ACgzS6 + 2M5tSJPfU0Jdo9OCaBVAAuxUQvwrvT7jj / ff / 8sKUwNahQYArRbdelf8VCpxlX4HsUQvkDw79O / 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";
            byte[] imageBytes = System.Convert.FromBase64String(myImage);
            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
        return null;
        //return new BitmapImage( iVBORw0KGgoAAAANSUhEUgAAAgAAAAIACAYAAAD0eNT6AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAjqAAAI6gAW78HIYAAEt + SURBVHhe7d0JYBXVvfjxc + berJBAEgKEXRERkN0dF1xaF9S6EHADAiq2trWtfXa1MmgXW7s9bW3lqeBaJbjv1iUqrqCACuLCvi8hQMieO + edyT2uBEjCXWbufD / vf5nf75fw / s9wc + c3Z86cIwUATzv9hzdndNi1s7OSTr5qVPkqpAqUkAWWUgWOEnnSErlCifb6W7ObjlLnQmTpVzv372sd9MuKhk219Gj4DUrs0n + 3wWS1 + lWja46u7YiWmo479atS6aP + 8KiUQm7Xf3GH / qZyfdwSComtTkRs3dJHbC2z7camvwXAk2gAgCQ6d + LvC9JD9b2diOytT9G9lFI9dLmrlKLIPZpXgX750Xb9CbNBNxHr9X / POt00rJGO3KCEWmNZYq1jiZWld9jbzPcCSDAaACC + ZPFEu6eSsp++Uj9YOaqf / qXrp + t99G9fb33Mafqu4Nqhfw7LdZOwQil9lGKFFPLTkAwtfWDmtWvM9wCIAxoAIDbk + VPsA6SSgy2lBumr3SG6doh + Haxf7nA8Wsu9JSHEUv2zXGpJ8ZFy5EdOWC166E57RdNXAewXGgCgldx78rmV24co4Yx0hBihf4mG6tPRQP3b5N6HR / xV6tcH + me + SFlyUUipRU5V7qLS0qtrol8G0BI0AMBeFBfPDjlZSwaFLHGMPuGMVJYYqY + H6i + lRb8DHtGgP80 + VEq8bQkxT4bEO87OgR + Vlo6LmK8D + AYaAOAriqfe2EHU1x6tfzOOFo4 + 6UtxpC4H / T69P0Wfanhbf8jNFZaYqzLFW6W32u5tBQAaDQAC7ZwSu2O6kMc5Sp0oLTFanzSG6vLnj8whlSjhPpa4UErdEAjxSr0QZY / Osrc3fQ0IIBoABEpx8V + zRLudJ + iTwClKiRN1yT3hh5q + iKBxbw + 8q18v6ebvRVWZ + zrzCBAkNABIeeMn3DBAhZ3TlFKn6vR4 / WJWPppTK5SYKy35tJLW06V3 / uZjUwdSEg0AUo47S7995bYT9Yf52fodPkaXekW / ArSKuz7B00rJp2VNThmjA0g1NABICcWX2vkqIk + WQp2l07P1y13 + FogVd1nkF / XxiUZHPPrIPfbmaBnwLxoA + Na5E + zOaSFxntLnf52eoF / cy0ciuPslvKw / PB9pbEx77OF7f70hWgb8hQYAvuKunR + S9WP0G7dYv3vde / o8j49kcvTrTf1RWhoOhx / 4z + 2 / 3hQtA95HAwDPK77Sbi + q5blCqEt0erJ + caUPL3IXI3pBCvlgWr165L77bHfnRMCzaADgSbZtW4tXN62 + N0FfY12k36kssws / qdUfrk / oj9h7NvdRz7A1MryIBgCeUjzlhv7CiVyqw4n61aWpCPibuxXyvVKKu0tn2ktMDUg6GgAk3VlT7eyMOjFWf0BeptPjolUgJb2lpJxRl6YefGKGXW1qQFLQACBpzp9oD7Gk + K5 + F16kUx7bQ5Ds0B++90nLmvHgndctMjUgoWgAkFDFxXa6aifO02 + 8K3XK1T4gxNtKiVurcvMffOaWq + pMDYg7GgAkxHmX2T1CEX21r4R7f79rtArgKzbpT + TbIg1p / 2ZtASQCDQDi6vwp9mEyIq6WUozVKc / sA / tWr19zHGnd / NDM696OloDYowFAzLmP8C1ZLU5SEfEj / Q4705QBtN67SombZfXA + 0pLx7m7FwIxQwOAmHE34Wm3q6JEKvVTnfaLVgHEwFIlxV + q2uffwzwBxAoNAPabu1KfqpJXSKmu1mm3aBVAHGyUQv691sq89fE7f15pakCb0ACgzS6 + 2M5tSJPfU0Jdo9OCaBVAAuxUQvwrvT7jj / ff / 8sKUwNahQYArRbdelf8VCpxlX4HsUQvkDw79O / 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);
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