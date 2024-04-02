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

namespace PL.UserPages
{
    /// <summary>
    /// Interaction logic for UsersViewPage.xaml
    /// </summary>
    public partial class UsersViewPage : Page
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(UsersViewPage), new PropertyMetadata(null));



        public List<BO.User>? UsersList
        {
            get { return (List<BO.User>?)GetValue(UsersListProperty); }
            set { SetValue(UsersListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UsersList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsersListProperty =
            DependencyProperty.Register("UsersList", typeof(List<BO.User>), typeof(UsersViewPage), new PropertyMetadata(null));



        public UsersViewPage(BO.User user)
        {
            InitializeComponent();
            CurrentUser = user;
            UsersList = s_bl.User.ReadAll()?.ToList();
        }

        private void AddNewUser_BtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserPages.UserPage(CurrentUser, ""));
        }

        private void UserSelected_listViewItemSelected(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BO.User user = (sender as ListView)?.SelectedItem as BO.User;
                if (user != null)
                    NavigationService.Navigate(new UserPages.UserPage(CurrentUser, user.Password));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load user! " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);                
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UsersList = s_bl.User.ReadAll()?.ToList();
        }
    }
}
