using System.ComponentModel;
using System.Windows;
using BL;
namespace PL.PO;

public class GanttTask : INotifyPropertyChanged
{
    private Thickness _margin;
    public Thickness Margin
    {
        get { return _margin; }
        set
        {
            _margin = value;
            OnPropertyChanged("Margin");
        }
    }
    private int _width;
    public int Width
    {
        get { return _width; }
        set
        {
            _width = value;
            OnPropertyChanged("Width");
        }
    }
    private int _id;
    public int Id
    {
        get { return _id; }
        set
        {
            _id = value;
            OnPropertyChanged("Id");
        }
    }
    private string _alias;
    public string Alias
    {
        get { return _alias; }
        set
        {
            _alias = value;
            OnPropertyChanged("Alias");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public GanttTask(int id)
    {
        //BlApi.IBl s_bl = BlApi.Factory.Get();
        //BO.Task task = s_bl.Task.Read(t => t.Id == id)!;
        //Margin = new Thickness((((int)(task.ScheduledDate - s_bl.DateControl.GetStartDate()).Value.TotalDays) * 20),0,0,0);   
        //Width = (int)((task.ForecastDate - task.ScheduledDate).Value.TotalDays) * 20;
        //Id = task.Id;
        //Alias = task.Alias;
    }
}
