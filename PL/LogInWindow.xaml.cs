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
            Loading = false;
            CurrentUser = new BO.User();
            OnShutDown += s_bl.stopClock;
        }

        async private void AttemptLogIn_btnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Loading = true;
                string tmpPassword = CurrentUser.Password, tmpName = CurrentUser.UserName;
                var tmp = await Task.Run(() => s_bl.User.LogIn(tmpPassword, tmpName));
                CurrentUser = tmp;                
                new MainWindow(CurrentUser).Show();
                this.WindowState = WindowState.Minimized;
                Loading = false;
                CurrentUser = new BO.User();
            }
            catch (Exception ex)
            {
                Loading = false;
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                        
        }

        private void PasswordUpdated_TextBoxChanged(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
                return;
            }           
        }

        private void Exit_btnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimize_btnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private async void WindowClosed(object sender, EventArgs e)
        {
            s_bl.SaveClock();
            OnShutDown.Invoke(this, EventArgs.Empty);
            Application.Current.Shutdown();            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
