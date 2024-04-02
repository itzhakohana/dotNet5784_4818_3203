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
    /// Interaction logic for GanttSchedulePage.xaml
    /// </summary>
    public partial class GanttSchedulePage : Page
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Loading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register("Loading", typeof(bool), typeof(GanttSchedulePage), new PropertyMetadata(null));



        public List<BO.Task>? AllTasks
        {
            get { return (List<BO.Task>?)GetValue(AllTasksProperty); }
            set { SetValue(AllTasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllTasksProperty =
            DependencyProperty.Register("AllTasks", typeof(List<BO.Task>), typeof(GanttSchedulePage), new PropertyMetadata(null));

        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(GanttSchedulePage), new PropertyMetadata(null));

        public List<string> Dates
        {
            get { return (List<string>)GetValue(DatesProperty); }
            set { SetValue(DatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Dates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DatesProperty =
            DependencyProperty.Register("Dates", typeof(List<string>), typeof(GanttSchedulePage), new PropertyMetadata(null));

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

        public GanttSchedulePage(BO.User user)
        {
            InitializeComponent();
            CurrentUser = user;
            refreshData();
        }

        private void GanttTaskRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
