using BlApi;
using PL.ProjectPages;
using PL.TaskPages;
using System;
using System.Collections.Generic;
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

namespace PL.ProjectPages
{
    /// <summary>
    /// Interaction logic for ProjectPage.xaml
    /// </summary>
    public partial class ProjectPage : Page
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        async private void refreshData()
        {
            try
            {
                Loading = true;

                //***********************
                
                var ganttTasks = await Task.Run(() => (from t in s_bl.Task.ReadAllTasks()?.ToList().OrderBy(t => t.ScheduledDate).ThenBy(t => t.RequiredEffortTime) select t).ToList());
                AllTasks = ganttTasks;
                Dates = new List<string>();
                if (s_bl.DateControl.GetStartDate() != null)
                {
                    DateTime tempDate = s_bl.DateControl.GetStartDate()!.Value;
                    foreach (BO.Task t in AllTasks)
                    {
                        Dates.Add(tempDate.ToShortDateString() + " - " + (tempDate + new TimeSpan(7, 0, 0, 0)).ToShortDateString());
                        tempDate += new TimeSpan(7, 0, 0, 0);
                    }
                }
                //***********************


                _projectStarted = s_bl.Task.ProjectHasStarted();

                var engs = await Task.Run(() => s_bl.Engineer.ReadAll());
                _engineersAmount = engs != null ? engs.Count() : 0;

                var tasks = await Task.Run(() => s_bl.Task.ReadAllTasks());
                _tasksAmount = tasks != null ? tasks.Count() : 0;

                var users = await Task.Run(() => s_bl.User.ReadAll());
                _usersAmount = users != null ? users.Count() : 0;

                TasksInProgress = await Task.Run(() => s_bl.Task.ReadAllTasksInList(t => t.Status == BO.Status.OnTrack || (t.Status == BO.Status.InJeopardy && s_bl.Task.Read(t.Id)!.StartDate != null))?.ToList());

                if (CurrentUser.Engineer != null)
                {
                    int tmpUserId = CurrentUser.Id;
                    AvailableTasks = await Task.Run(() => s_bl.Task.ReadAllAvailableTasks(tmpUserId)?.ToList());
                }                       
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    Loading = false;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
            finally { Loading = false; }          
        }


        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Loading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register("Loading", typeof(bool), typeof(ProjectPage), new PropertyMetadata(null));



        public List<BO.Task>? AllTasks
        {
            get { return (List<BO.Task>?)GetValue(AllTasksProperty); }
            set { SetValue(AllTasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllTasksProperty =
            DependencyProperty.Register("AllTasks", typeof(List<BO.Task>), typeof(ProjectPage), new PropertyMetadata(null));



        public List<BO.TaskInList>? AvailableTasks
        {
            get { return (List<BO.TaskInList>?)GetValue(AvailableTasksProperty); }
            set { SetValue(AvailableTasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvailableTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvailableTasksProperty =
            DependencyProperty.Register("AvailableTasks", typeof(List<BO.TaskInList>), typeof(ProjectPage), new PropertyMetadata(null));


        public List<BO.TaskInList>? TasksInProgress
        {
            get { return (List<BO.TaskInList>?)GetValue(TasksInProgressProperty); }
            set { SetValue(TasksInProgressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TasksInProgress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TasksInProgressProperty =
            DependencyProperty.Register("TasksInProgress", typeof(List<BO.TaskInList>), typeof(ProjectPage), new PropertyMetadata(null));



        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(ProjectPage), new PropertyMetadata(null));





        private bool _projectStarted
        {
            get { return (bool)GetValue(_projectStartedProperty); }
            set { SetValue(_projectStartedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _projectStarted.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _projectStartedProperty =
            DependencyProperty.Register("_projectStarted", typeof(bool), typeof(ProjectPage), new PropertyMetadata(null));



        private int _usersAmount
        {
            get { return (int)GetValue(_usersAmountProperty); }
            set { SetValue(_usersAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _usersAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _usersAmountProperty =
            DependencyProperty.Register("_usersAmount", typeof(int), typeof(ProjectPage), new PropertyMetadata(null));



        private int _tasksAmount
        {
            get { return (int)GetValue(_tasksAmountProperty); }
            set { SetValue(_tasksAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _tasksAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _tasksAmountProperty =
            DependencyProperty.Register("_tasksAmount", typeof(int), typeof(ProjectPage), new PropertyMetadata(null));

        private int _engineersAmount
        {
            get { return (int)GetValue(_engineersAmountProperty); }
            set { SetValue(_engineersAmountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _engineersAmount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _engineersAmountProperty =
            DependencyProperty.Register("_engineersAmount", typeof(int), typeof(ProjectPage), new PropertyMetadata(null));



        public List<string> Dates
        {
            get { return (List<string>)GetValue(DatesProperty); }
            set { SetValue(DatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Dates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DatesProperty =
            DependencyProperty.Register("Dates", typeof(List<string>), typeof(ProjectPage), new PropertyMetadata(null));



        public ProjectPage(BO.User currentUser)
        {
            InitializeComponent();
            CurrentUser = currentUser;
            refreshData();            
        }

        private void StartProject_btnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartProjectPage());            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        async private void ResetAllData_BtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("All data will be permenantly deleted, Are you sure you want to proceed?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {                    
                    Loading = true;
                    await Task.Run(() => s_bl.Reset());
                    Loading = false;
                    MessageBox.Show("Reset Successfull", default, MessageBoxButton.OK, MessageBoxImage.None);
                    refreshData();
                }
            }
            catch (Exception ex)
            {
                Loading = false;
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { Loading = false; }
        }

        async private void RandomizeAllData_BtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result;
                if(s_bl.Task.ProjectHasStarted())
                {
                    result = MessageBox.Show("Cannot add data to an ongoing project. randomizing the data-base requires reseting the current project, do you want to proceed?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        result = MessageBox.Show("All data will be permenantly deleted, Are you sure you want to proceed?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            Loading = true;
                            await Task.Run(() => s_bl.Reset()); 
                        }
                    }
                }
                else
                {
                    result = MessageBox.Show("The data base will be initiated with randomly generated data, are you sure you want to proceed?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                }
                if (result == MessageBoxResult.Yes)
                {
                    Loading = true;
                    await Task.Run(() => s_bl.InitializeDataBase());
                    Loading = false;
                    MessageBox.Show("Randomazation Successfull", default, MessageBoxButton.OK, MessageBoxImage.None);
                    refreshData();
                }                
            }
            catch (Exception ex)
            {
                Loading = false;
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { Loading = false; }
        }

        private void AvalableTasks_ButtonOptionSelected(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                BO.TaskInList? task = btn.DataContext as BO.TaskInList;
                if (task != null)
                {
                    if (btn.Content is not null and (object)(string)"View")
                    {
                        NavigationService.Navigate(new TaskPage(CurrentUser, task!.Id));
                    }

                    if (btn.Content is not null and (object)(string)"Take")
                    {
                        try
                        {
                            s_bl.Task.UpdateAssignedEngineer(CurrentUser.Id, task.Id);
                            refreshData();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
      
        private void GanttTaskRectangle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!e.Handled) { }
        }
        

        private void GanttTaskSelected_ListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BO.Task task = (sender as ListView)?.SelectedItem as BO.Task;
            if (task != null)
                NavigationService.Navigate(new TaskPage(CurrentUser, task.Id));
        }

        private void PageLoaded_Loaded(object sender, RoutedEventArgs e)
        {
            refreshData();
        }

        private void ReloadPage_BtnClick(object sender, RoutedEventArgs e)
        {
            refreshData();
        }
    }
}
