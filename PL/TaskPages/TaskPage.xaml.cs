using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL.TaskPages
{
    /// <summary>
    /// Interaction logic for TaskPage.xaml
    /// </summary>
    public partial class TaskPage : Page
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private int _id;



        public List<BO.TaskInList>? CurrentTaskDependencies
        {
            get { return (List<BO.TaskInList>?)GetValue(CurrentTaskDependenciesProperty); }
            set { SetValue(CurrentTaskDependenciesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTaskDependencies.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTaskDependenciesProperty =
            DependencyProperty.Register("CurrentTaskDependencies", typeof(List<BO.TaskInList>), typeof(TaskPage), new PropertyMetadata(null));



        public List<BO.TaskInList>? AvailableDependencies
        {
            get { return (List<BO.TaskInList>?)GetValue(AvailableDependenciesProperty); }
            set { SetValue(AvailableDependenciesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvailableDependencies.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvailableDependenciesProperty =
            DependencyProperty.Register("AvailableDependencies", typeof(List<BO.TaskInList>), typeof(TaskPage), new PropertyMetadata(null));



        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskPage), new PropertyMetadata(null));


        public List<BO.EngineerInTask>? AvailableEngineers
        {
            get { return (List<BO.EngineerInTask>?)GetValue(AvailableEngineersProperty); }
            set { SetValue(AvailableEngineersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvailableEngineersProperty =
            DependencyProperty.Register("AvailableEngineers", typeof(List<BO.EngineerInTask>), typeof(TaskPage), new PropertyMetadata(null));



        public TaskPage(BO.User CurrentUser, int id = 0)
        {
            InitializeComponent();            
            if (id == 0)
            {
                Task = new BO.Task() {Engineer = null };
                AvailableDependencies = s_bl.Task.ReadAllTasksInList(t => t.Id != Task.Id)?.ToList();
                CurrentTaskDependencies = null;
                _id = id;
            }
            else
            {
                Task = s_bl.Task.Read(id)!;
                AvailableEngineers = s_bl.Engineer.ReadEngineersInTask(id)?.ToList();
                AvailableDependencies = s_bl.Task.ReadAllTasksInList(t => t.Id != Task.Id && !s_bl.Task.CheckDependency(t.Id, Task.Id))?.ToList();
                CurrentTaskDependencies = Task.Dependencies;
                _id = id;
            }
        }

        private void ResetChanges_btnClick(object sender, RoutedEventArgs e)
        {
            if (_id != 0)
            {
                Task = s_bl.Task.Read(_id)!;
            }
            else
            {
                Task = new BO.Task() { Engineer = null, Dependencies = null };
            }
            CurrentTaskDependencies = Task.Dependencies;
        }

        private void AddOrUpdateTask_btnClick(object sender, RoutedEventArgs e)
        {
            Task.Dependencies = CurrentTaskDependencies;
            try
            {
                if (_id == 0)
                {
                    s_bl.Task.Add(Task);
                    NavigationService.GoBack();
                    MessageBox.Show("Successfuly Added Task", default, MessageBoxButton.OK, MessageBoxImage.None);
                }
                else
                {
                    s_bl.Task.Update(Task);
                    NavigationService.GoBack();
                    MessageBox.Show("Successfuly Updated Task", default, MessageBoxButton.OK, MessageBoxImage.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoBack_BtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void EnteredKey_RequiredDaysFieldChanged(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void ComplexitySelectionCHanged_ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AvailableEngineers = s_bl.Engineer.ReadEngineersInTask(Task.Complexity)?.ToList();
            
            if (Task.Engineer != null)
            {
                var eng = s_bl.Engineer.Read(Task.Engineer.Id);
                if(eng != null && eng.Level < Task.Complexity)
                    Task.Engineer = null;
            }
        }

        private void ShowAddDependenciesWindow_BtnClick(object sender, RoutedEventArgs e)
        {
            AddDependenciesBorder.Visibility = Visibility.Visible;
            if (CurrentTaskDependencies != null)
                CurrentTaskDependencies = (from t in CurrentTaskDependencies
                                           select t).ToList();
        }
        private void ShowViewDependenciesWindow_BtnClick(object sender, RoutedEventArgs e)
        {
            CurrentDependenciesBorder.Visibility = Visibility.Visible;
            AddDependenciesBorder.Visibility = Visibility.Visible;
            if (CurrentTaskDependencies != null) 
                CurrentTaskDependencies = (from t in CurrentTaskDependencies
                                           select t).ToList();
        }
        private void CloseDependenciesWindow_BtnClick(object sender, RoutedEventArgs e)
        {
            AddDependenciesBorder.Visibility = Visibility.Collapsed;            
            CurrentDependenciesBorder.Visibility = Visibility.Collapsed;
        }       

        private void AddDependency_BtnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                BO.TaskInList task = btn.DataContext as BO.TaskInList;
                if (task != null)
                {
                    if (CurrentTaskDependencies is null) CurrentTaskDependencies = new List<BO.TaskInList>();
                    if (!CurrentTaskDependencies.Contains(task))
                    {
                        //var tmp = CurrentTaskDependencies;
                        //tmp.Add(task);
                        //CurrentTaskDependencies = tmp;
                        CurrentTaskDependencies.Add(task);
                        CurrentTaskDependencies = (from t in CurrentTaskDependencies
                                                   select t).ToList();
                    }
                    //AvailableDependencies = s_bl.Task.ReadAllTasksInList(t => t.Id != Task.Id && !s_bl.Task.CheckDependency(t.Id, Task.Id) && t.Id != task.Id)?.ToList();
                    AvailableDependencies = (from t in AvailableDependencies
                                             where t.Id != task.Id
                                             select t).ToList();                   
                }
            }
        }
        private void RemoveDependency_BtnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                BO.TaskInList task = btn.DataContext as BO.TaskInList;
                if (task != null)
                {
                    if (CurrentTaskDependencies.Contains(task))
                    {
                        CurrentTaskDependencies = (from t in CurrentTaskDependencies
                                                   where t != task
                                                   select t).ToList();
                        AvailableDependencies?.Add(task);
                        AvailableDependencies = (from t in AvailableDependencies
                                                 select t).ToList();
                    }
                    //AvailableDependencies = s_bl.Task.ReadAllTasksInList(t => t.Id != Task.Id && !s_bl.Task.CheckDependency(t.Id, Task.Id))?.ToList();
                }
            }
        }

        private void EngineerSelected_ListView(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
