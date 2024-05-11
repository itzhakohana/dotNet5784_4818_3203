using PL.EngineerPages;
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
using static System.Net.Mime.MediaTypeNames;

namespace PL.UserPages
{
    /// <summary>
    /// Interaction logic for UsersViewPage.xaml
    /// </summary>
    public partial class UsersViewPage : Page
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();



        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Loading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register("Loading", typeof(bool), typeof(UsersViewPage), new PropertyMetadata(null));



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


        private async void refreshPage()
        {
            Loading = true;
            var users = await Task.Run(() => s_bl.User.ReadAll());
            UsersList = users!.ToList();
            Loading = false;
        }

        public UsersViewPage(BO.User user)
        {
            InitializeComponent();
            Loading = false;
            CurrentUser = user;
            UsersList = s_bl.User.ReadAll()?.ToList();
        }

        private void AddNewUser_BtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserPages.UserPage(CurrentUser, ""));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UsersList = s_bl.User.ReadAll()?.ToList();
        }



        private void UserOptions_ComboBoxItemSelected(object sender, RoutedEventArgs e)
        {
            if (CurrentUser.UserType != BO.UserType.Admin)
            {
                MessageBox.Show("You dont have configuration permissions", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (sender is ComboBoxItem comboBoxItem)
            {
                try
                {
                    // Access the DataContext (which should be a User)
                    BO.User user = comboBoxItem.DataContext as BO.User;

                    if ((string)comboBoxItem.Content == "Edit")
                    {
                        NavigationService.Navigate(new UserPage(CurrentUser, user.Password));
                    } 
                    
                    if ((string)comboBoxItem.Content == "View Engineer")
                    {
                        if (user.Engineer != null)
                        {
                            NavigationService.Navigate(new EngineerPage(CurrentUser, user.Engineer!.Id)); 
                        }
                        else
                            MessageBox.Show("No engineer is assigned to this user", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    if ((string)comboBoxItem.Content == "Delete")
                    {  
                        try
                        {
                            s_bl.User.Delete(user.Id);
                            refreshPage();
                            MessageBox.Show("Deletion Successful", "Success", MessageBoxButton.OK, MessageBoxImage.None);                            
                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                    if (user != null)
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load user! " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UserSelected_listViewItemSelected(object sender, MouseButtonEventArgs e)
        {
            if (CurrentUser.UserType == BO.UserType.Admin)
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
        }

        private void RefreshUsersList_BtnClick(object sender, RoutedEventArgs e)
        {
            refreshPage();
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
                            UsersList = UsersList.OrderBy(e => e.UserName).ToList();
                            break;
                        case "Creation Date":
                            UsersList = UsersList.OrderBy(e => e.CreationDate).ToList();
                            break;
                        case "User Type":
                            UsersList = UsersList.OrderBy(e => e.UserType).ToList();
                            break;
                        case "Last Activity":
                            UsersList = UsersList.OrderByDescending(e => e.LastLoginDate).ToList();
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
                if (textBox.Text != "")
                {
                    UsersList = (from user in UsersList
                                 where user.UserName.StartsWith(textBox.Text, StringComparison.OrdinalIgnoreCase)
                                 select user).ToList(); 
                }
                else
                    refreshPage();
            }
        }
    }
}
