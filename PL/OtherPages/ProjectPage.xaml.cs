using PL.ProjectPages;
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

namespace PL.OtherPages
{
    /// <summary>
    /// Interaction logic for ProjectPage.xaml
    /// </summary>
    public partial class ProjectPage : Page
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private void RefreshData()
        {
            _projectStarted = s_bl.Task.ProjectHasStarted();

            var engs = s_bl.Engineer.ReadAll();
            _engineersAmount = engs != null ? engs.Count() : 0;

            var tasks = s_bl.Task.ReadAll();
            _tasksAmount = tasks != null ? tasks.Count() : 0;

            var users = s_bl.User.ReadAll();
            _usersAmount = users != null ? users.Count() : 0;
        }

        public BO.UserType _currentUser
        {
            get { return (BO.UserType)GetValue(_currentUserProperty); }
            set { SetValue(_currentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _currentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _currentUserProperty =
            DependencyProperty.Register("_currentUser", typeof(BO.UserType), typeof(ProjectPage), new PropertyMetadata(null));



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



        public ProjectPage()
        {
            InitializeComponent();
            RefreshData();
            _currentUser = BO.UserType.Admin;
            //var navigationWindow = Window.GetWindow(this);
            //var mainWindow = Window.GetWindow(navigationWindow) as MainWindow;
            //if (mainWindow != null)
            //{
            //    _currentUser = mainWindow.User;
            //}
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
                await Task.Run(() => s_bl.Reset());
                MessageBox.Show("Reset Successfull", default, MessageBoxButton.OK, MessageBoxImage.None);
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        async private void RandomizeAllData_BtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() => s_bl.InitializeDataBase());
                MessageBox.Show("Randomazation Successfull", default, MessageBoxButton.OK, MessageBoxImage.None);
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
