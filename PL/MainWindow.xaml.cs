using PL.CustomControls;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private bool _closingApp = false;
        
        async private void reloadClock() 
        {
            await Task.Run(() => 
            {
                while (!_closingApp)
                {
                    Dispatcher.Invoke(() => CurrentTime = s_bl.Clock);
                };   
                s_bl.SaveClock();
            });            
        }

        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));



        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(MainWindow), new PropertyMetadata(null));



        public BO.Task? CurrentTask
        {
            get { return (BO.Task?)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(MainWindow), new PropertyMetadata(null));



        public bool CanUpdateTaskProgress
        {
            get { return (bool)GetValue(CanUpdateTaskProgressProperty); }
            set { SetValue(CanUpdateTaskProgressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanUpdateTaskProgress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanUpdateTaskProgressProperty =
            DependencyProperty.Register("CanUpdateTaskProgress", typeof(bool), typeof(MainWindow), new PropertyMetadata(null));



        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, value); }
        } 

        // Using a DependencyProperty as the backing store for Loading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register("Loading", typeof(bool), typeof(MainWindow), new PropertyMetadata(null));



        public MainWindow(BO.User user)
        {
            InitializeComponent();
            Loading = false;
            CurrentUser = user;
            CurrentTask = CurrentUser.CurrentTask;
            if (CurrentTask != null) CanUpdateTaskProgress = s_bl.Task.CanStartWork(CurrentTask.Id) || CurrentTask.Status == BO.Status.OnTrack;
            else CanUpdateTaskProgress = false;
            reloadClock();            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        private void GridClick_btnClick(object sender, RoutedEventArgs e)
        {
            var myClickedButton = e.OriginalSource as NavigationButton;
            if (myClickedButton != null)
            {
                //MainFrame.NavigationService.Navigate(myClickedButton.NextPage);
                switch(myClickedButton.Title)
                {
                    case "Project":
                        MainFrame.NavigationService.Navigate(new ProjectPages.ProjectPage(CurrentUser));
                        break;
                    case "Engineers":
                        MainFrame.NavigationService.Navigate(new EngineerPages.EngineersViewPage(CurrentUser));
                        break;
                    case "Tasks":
                        MainFrame.NavigationService.Navigate(new TaskPages.TasksViewPage(CurrentUser));
                        break;
                    case "Users":
                        MainFrame.NavigationService.Navigate(new UserPages.UsersViewPage(CurrentUser));
                        break;
                    case "Milestones":
                        MainFrame.NavigationService.Navigate(new MilestonePages.MilestonesView(CurrentUser));
                        break;
                    case " ":
                        break;

                }
            }

        }

        private void Exit_btnClick(object sender, RoutedEventArgs e)
        {
            _closingApp = true;
            Thread.Sleep(200);
            App.Current.Shutdown();
        }

        private void Minimize_btnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ManageClock_BtnClick(object sender, RoutedEventArgs e)
        {
            var myClickedButton = e.OriginalSource as Button;
            if (myClickedButton != null)
            {
                switch (myClickedButton.Content)
                {
                    case "+ Hour":
                        s_bl.ClockAddHour();
                        break;
                    case "+ Day":
                        s_bl.ClockAddDay();
                        break;
                    case "+ Month":
                        s_bl.ClockAddMonth();
                        break;                    

                }
            }
        }

        private void UpdateTaskProgress_btnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                try
                {
                    switch (btn.Content)
                    {
                        case "Complete Task":
                            CurrentTask!.CompleteDate = s_bl.Clock;
                            s_bl.Task.Update(CurrentTask);
                            break;
                        case "Start Work":
                            CurrentTask!.StartDate = s_bl.Clock;
                            s_bl.Task.Update(CurrentTask);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                CurrentUser = s_bl.User.Read(CurrentUser.Id)!;
                CurrentTask = CurrentUser.CurrentTask;
            }            
        }

        private void ViewTaskCurrentTask_BtnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn )
            {
                if (CurrentTask is not null)
                    MainFrame.NavigationService.Navigate(new TaskPages.TaskPage(CurrentUser, CurrentTask.Id));
            }
        }
    }
}