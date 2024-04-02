using PL.TaskPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(UserPage), new PropertyMetadata(null));



        public bool InAddMode
        {
            get { return (bool)GetValue(InAddModeProperty); }
            set { SetValue(InAddModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InAddMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InAddModeProperty =
            DependencyProperty.Register("InAddMode", typeof(bool), typeof(UserPage), new PropertyMetadata(null));



        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(UserPage), new PropertyMetadata(null));




        public BO.User MyUser
        {
            get { return (BO.User)GetValue(MyUserProperty); }
            set { SetValue(MyUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyUserProperty =
            DependencyProperty.Register("MyUser", typeof(BO.User), typeof(UserPage), new PropertyMetadata(null));




        public List<BO.Engineer>? AvailableEngineers
        {
            get { return (List<BO.Engineer>?)GetValue(AvailableEngineersProperty); }
            set { SetValue(AvailableEngineersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvailableEngineersProperty =
            DependencyProperty.Register("AvailableEngineers", typeof(List<BO.Engineer>), typeof(UserPage), new PropertyMetadata(null));


        public UserPage(BO.User user, string password)
        {
            InitializeComponent();
            CurrentUser = user;
            Password = password;            
            AvailableEngineers = s_bl.Engineer.ReadAll(e => s_bl.User.Read(e.Id) is null)?.ToList();
            if (password != "")
            {
                InAddMode = false;
                MyUser = s_bl.User.Read(u => u.Password == password);    
                if (MyUser is null)
                {
                    NavigationService.GoBack();                                      
                }                
            }
            else
            {
                MyUser = new BO.User() {UserType = BO.UserType.Engineer};
                InAddMode = true;
            }
        }

        private void UserTypeSelectionChanged_ComboBox(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EngineerSelected_ListView(object sender, SelectionChangedEventArgs e)
        {
            BO.Engineer engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
            if (engineer != null)
            {
                textBox.Text = engineer.Name;
                MyUser.Engineer = engineer;
            }
        }

        private void ResetChanges_btnClick(object sender, RoutedEventArgs e)
        {
            if (InAddMode)
                MyUser = new BO.User() { UserType = BO.UserType.Engineer };
            else
            {
                MyUser = s_bl.User.Read(u => u.Password == Password);
            }
        }

        private void AddOrUpdateUser_btnClick(object sender, RoutedEventArgs e)
        {
           
            try
            {
                if (InAddMode)
                {
                    s_bl.User.Add(MyUser!);
                    NavigationService.GoBack();
                    MessageBox.Show("Successfuly Added User", default, MessageBoxButton.OK, MessageBoxImage.None);
                }
                else
                {
                    s_bl.User.Update(MyUser!);
                    NavigationService.GoBack();
                    MessageBox.Show("Successfuly Updated User", default, MessageBoxButton.OK, MessageBoxImage.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoBack_BtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void PasswordUpdated_TextBoxChanged(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void NoEngineerSelected_checkBoxUnchecked(object sender, RoutedEventArgs e)
        {
            MyUser.Engineer = null;
        }
    }
}
