using PL.CustomControls;
using System.ComponentModel;
using System.Net.Mail;
using System.Net;
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
        private BackgroundWorker clockWorker;
        private BackgroundWorker userUpdaterWorker;

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



        public bool IsArtificialClock
        {
            get { return (bool)GetValue(IsArtificialClockProperty); }
            set { SetValue(IsArtificialClockProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RealTimeClock.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsArtificialClockProperty =
            DependencyProperty.Register("IsArtificialClock", typeof(bool), typeof(MainWindow), new PropertyMetadata(null));



        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, value); }
        } 

        // Using a DependencyProperty as the backing store for Loading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register("Loading", typeof(bool), typeof(MainWindow), new PropertyMetadata(null));

        private void WorkerReloadClock_DoWork(object sender, DoWorkEventArgs e)
        {
            while (clockWorker.CancellationPending == false)
            {
                Thread.Sleep(1000);
                clockWorker.ReportProgress(0);
            }
        }
        private void WorkerReloadClock_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CurrentTime = s_bl.Clock;
            IsArtificialClock = !s_bl.DateControl.GetIsRealTimeClock();
        }

        private void WorkerReloadUser_DoWork(object sender, DoWorkEventArgs e)
        {
            while (userUpdaterWorker.CancellationPending == false)
            {
                Thread.Sleep(3000);
                userUpdaterWorker.ReportProgress(0);
            }            
        }
        private async void WorkerReloadUser_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int tmpId = CurrentUser.Id;
            BO.User? tmpUser = await Task.Run(() => s_bl.User.Read(tmpId) ?? new BO.User());
            CurrentUser = tmpUser;
            CurrentTask = CurrentUser.CurrentTask;
            bool tmpCanStartWork = await Task.Run(() =>
                tmpUser.CurrentTask != null ?
                    s_bl.Task.CanStartWork(tmpUser.CurrentTask.Id) : false
            );
                        
            if (CurrentTask != null) CanUpdateTaskProgress = tmpCanStartWork || CurrentTask.Status == BO.Status.OnTrack;
            else CanUpdateTaskProgress = false;            
        }


        public MainWindow(BO.User user)
        {
            InitializeComponent();
            Loading = false;
            CurrentUser = user;     
            
            //initiating clock background worker
            clockWorker = new BackgroundWorker();
            clockWorker.DoWork += WorkerReloadClock_DoWork;
            clockWorker.ProgressChanged += WorkerReloadClock_ProgressChanged;            
            clockWorker.WorkerReportsProgress = true;
            clockWorker.WorkerSupportsCancellation = true;
            if (clockWorker.IsBusy != true)
                clockWorker.RunWorkerAsync();

            //initiating user-reloader background worker
            userUpdaterWorker = new BackgroundWorker();
            userUpdaterWorker.DoWork += WorkerReloadUser_DoWork;
            userUpdaterWorker.ProgressChanged += WorkerReloadUser_ProgressChanged;
            userUpdaterWorker.WorkerReportsProgress = true;
            userUpdaterWorker.WorkerSupportsCancellation = true;
            if (userUpdaterWorker.IsBusy != true)
                userUpdaterWorker.RunWorkerAsync();            
            Application.Current.Exit += Current_Exit;

        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {           
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
                    //case "Schedule":
                    //    MainFrame.NavigationService.Navigate(new ProjectPages.GanttSchedulePage(CurrentUser));
                    //    break;
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
            this.Close();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {                        
            clockWorker.CancelAsync();         
            userUpdaterWorker.CancelAsync();            
        }

        private void Minimize_btnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_btnClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState != WindowState.Maximized)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
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

        private void ViewCurrentUser_BtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn)
                {
                    switch (btn.Content){
                        case "View":
                            MainFrame.Navigate(new UserPages.UserPage(CurrentUser, CurrentUser.Password));
                            break;
                        case "View Engineer Profile":
                            if(CurrentUser.Engineer is not null)
                                MainFrame.Navigate(new EngineerPages.EngineerPage(CurrentUser, CurrentUser.Engineer.Id));
                            break;
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void WindowClosed(object sender, EventArgs e)
        {
        }

        private void GoToWelcomPage_LogoClicked(object sender, MouseButtonEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new OtherPages.MainWelcomePage());
        }
    }
}