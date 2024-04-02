using PL.EngineerPages;
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
    /// Interaction logic for TasksViewPage.xaml
    /// </summary>
    public partial class TasksViewPage : Page
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.None;

        public bool ProjectHasStarted { get; set; }
        public IEnumerable<BO.Task>? TaskList
        {
            get { return (IEnumerable<BO.Task>?)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>), typeof(TasksViewPage), new PropertyMetadata(null));

        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(TasksViewPage), new PropertyMetadata(null));


        public TasksViewPage(BO.User CurrentUser)
        {
            InitializeComponent();
            TaskList = s_bl.Task.ReadAllTasks();
            ProjectHasStarted = s_bl.Task.ProjectHasStarted();
            this.CurrentUser = CurrentUser;
        }

        private void TaskOptions_ComboBoxItemSelected(object sender, RoutedEventArgs e)
        {

            if (sender is ComboBoxItem comboBoxItem)
            {
                // Access the DataContext (which should be a Task)
                BO.Task task = comboBoxItem.DataContext as BO.Task;

                if((string)comboBoxItem.Content == "Edit")
                {
                    NavigationService.Navigate(new TaskPage(CurrentUser,task!.Id));
                }

                if ((string)comboBoxItem.Content == "Delete")
                {
                    string msg = "";
                    if (CurrentUser.UserType != BO.UserType.Admin)
                        msg = "You dont have permission to delete";
                    else if (s_bl.Task.ProjectHasStarted())
                        msg = "Project has stared. cannot delete";
                    else
                    {
                        try
                        {
                            s_bl.Task.Delete(task.Id);
                            MessageBox.Show("Deletion Successful", "Success", MessageBoxButton.OK, MessageBoxImage.None);
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (task != null)
                {

                }
            }
        }

        private void ItemSelected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Task task = (sender as ListView)?.SelectedItem as BO.Task;
            if (task != null)
                NavigationService.Navigate(new TaskPage(CurrentUser, task.Id));
        }

        private void SelectionChanged_FilterBox(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Experience == BO.EngineerExperience.None) ?
                 s_bl.Task.ReadAllTasks() : s_bl.Task.ReadAllTasks(t => t.Complexity == Experience);
        }

        private void AddNewTask_BtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskPage(CurrentUser, 0));
        }

        private void DeletesAllTasks_BtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("All Tasks & their Dependencies will be deleted, Are you sure you want to proceed?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    s_bl.Task.Reset();
                    MessageBox.Show("All Tasks Successfuly Deleted", "Success", MessageBoxButton.OK, MessageBoxImage.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TaskList = s_bl.Task.ReadAllTasks();
        }

        private void SelectionChanged_OrederByComboBox(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox myCombo)
            {
                var item = myCombo.SelectedItem as ComboBoxItem;
                if (item is null) return;
                try
                {
                    switch (item.Content)
                    {
                        case "Name":
                            TaskList = TaskList.OrderBy(e => e.Alias);
                            break;
                        case "Complexity":
                            TaskList = TaskList.OrderBy(e => e.Complexity);
                            break;
                        case "Status":
                            TaskList = TaskList.OrderBy(e => e.Status);
                            break;
                        case "Creation Date":
                            TaskList = TaskList.OrderBy(e => e.CreatedAtDate);
                            break;                            
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }
        private void TextChanged_SearchTextBox(object sender, TextChangedEventArgs e)
        {
            if (sender is CustomControls.CustomTextBox textBox)
            {
                TaskList = (from task in TaskList
                            where task.Alias.StartsWith(textBox.Text, StringComparison.OrdinalIgnoreCase)
                                select task);
            }
        }
    }
}
