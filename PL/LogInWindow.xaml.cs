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
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EventHandler OnShutDown;
        public EventHandler OnSuccessfulLogIn;

        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(LogInWindow), new PropertyMetadata(null));



        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("MyProperty", typeof(string), typeof(LogInWindow), new PropertyMetadata(null));



        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Loading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register("Loading", typeof(bool), typeof(LogInWindow), new PropertyMetadata(null));

        public LogInWindow()
        {
            InitializeComponent();
            NameTextBox.Focus();
            Loading = false;
            OnSuccessfulLogIn += ResetLogInWindow_OnSuccessfulLogIn!;

            //for testing purposes, we make sure an admin user with the name "test" always exists upon startup
            if (s_bl.User.Read(u => u.UserName == "test" && u.Password == "1234" && u.UserType == BO.UserType.Admin) == null)
                s_bl.User.Add(new BO.User() { UserName = "test", Password = "1234", UserType = BO.UserType.Admin });            
            
            CurrentUser = new BO.User();
            OnShutDown += s_bl.stopClock!;
        }

        void ResetLogInWindow_OnSuccessfulLogIn(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            PasswordBox.Password = "";            
            CurrentUser = new BO.User();
        }

        async private void AttemptLogIn_btnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Loading = true;
                string tmpPassword = CurrentUser.Password, tmpName = CurrentUser.UserName;
                var tmp = await Task.Run(() => s_bl.User.LogIn(tmpPassword, tmpName));
                CurrentUser = tmp;
                MainWindow mainWindow =  new MainWindow(CurrentUser);
                mainWindow.Closed += MaximizeOnMainWindowClosed_Closed;
                mainWindow.Show(); 
                OnSuccessfulLogIn?.Invoke(this, EventArgs.Empty);
                Loading = false;
            }
            catch (Exception ex)
            {
                Loading = false;
                this.WindowState = WindowState.Normal;
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                        
        }

        private void MaximizeOnMainWindowClosed_Closed(object? sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void Exit_btnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimize_btnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }   

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }       

        private void PasswordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (!int.TryParse(e.Text, out _))
                {
                    e.Handled = true;
                    return;
                }
                CurrentUser.Password = passwordBox.Password;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (!int.TryParse(passwordBox.Password, out _))
                {
                    passwordBox.Password = "";
                }
                CurrentUser.Password = passwordBox.Password;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                s_bl.SaveClock();
                OnShutDown.Invoke(this, EventArgs.Empty);
                Application.Current.Shutdown();
            }
            else
                e.Cancel = true;
        }

        private void WindowStateChanged(object sender, EventArgs e)
        {
            if(this.WindowState != WindowState.Minimized) 
            {
                NameTextBox.Focus();
            }
        }
    }
}
