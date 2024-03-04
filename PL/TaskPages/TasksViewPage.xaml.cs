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

        public IEnumerable<BO.Task>? TaskList
        {
            get { return (IEnumerable<BO.Task>?)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.Task>), typeof(TasksViewPage), new PropertyMetadata(null));


        public TasksViewPage()
        {
            InitializeComponent();
            TaskList = s_bl.Task.ReadAllTasks();
        }

        private void TaskOptions_ComboBoxItemSelected(object sender, RoutedEventArgs e)
        {

            if (sender is ComboBoxItem comboBoxItem)
            {
                // Access the DataContext (which should be a Task)
                BO.Task task = comboBoxItem.DataContext as BO.Task;

                if((string)comboBoxItem.Content == "Edit")
                {
                    NavigationService.Navigate(new TaskPage(task!.Id));
                }

                if ((string)comboBoxItem.Content == "Delete")
                {

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
                NavigationService.Navigate(new TaskPage(task.Id));
        }

        private void SelectionChanged_FilterBox(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddNewTask_BtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskPage(0));
        }
    }
}
