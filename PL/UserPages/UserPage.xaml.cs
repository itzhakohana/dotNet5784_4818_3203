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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private string _password;



        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(UserPage), new PropertyMetadata(null));




        public BO.User? myUser
        {
            get { return (BO.User?)GetValue(MyUserProperty); }
            set { SetValue(MyUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyUserProperty =
            DependencyProperty.Register("MyUser", typeof(BO.User), typeof(UserPage), new PropertyMetadata(null));


        public UserPage(BO.User user, string password)
        {
            InitializeComponent();
            CurrentUser = user;
            _password = password;
            if (password != "")
            {
                myUser = s_bl.User.Read(u => u.Password == password);    
                if (myUser is null) { }
                {
                    NavigationService.GoBack();
                    MessageBox.Show("Failed to load user", "Error", MessageBoxButton.OK, MessageBoxImage.Error);                    
                }                
            }
            else
            {
                myUser = new BO.User();
            }
        }

        private void UserTypeSelectionChanged_ComboBox(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EngineerSelected_ListView(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
