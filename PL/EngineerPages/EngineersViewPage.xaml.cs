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
using PL;
using PL.TaskPages;
using System.IO;

namespace PL.EngineerPages
{
    /// <summary>
    /// Interaction logic for EngineersViewPage.xaml
    /// </summary>
    public partial class EngineersViewPage : Page
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.None;

        public BitmapImage MyImage
        {
            get { return (BitmapImage)GetValue(MyImageProperty); }
            set { SetValue(MyImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyImageProperty =
            DependencyProperty.Register("MyImage", typeof(BitmapImage), typeof(EngineersViewPage), new PropertyMetadata(null));

        public IEnumerable<BO.Engineer>? EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }
        // Using a DependencyProperty as the backing store for EngineerList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineersViewPage), new PropertyMetadata(null));



        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(EngineersViewPage), new PropertyMetadata(null));



        public BO.User NewUser
        {
            get { return (BO.User)GetValue(NewUserProperty); }
            set { SetValue(NewUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewUserProperty =
            DependencyProperty.Register("NewUser", typeof(BO.User), typeof(EngineersViewPage), new PropertyMetadata(null));


        public EngineersViewPage(BO.User CurrentUser)
        {
            InitializeComponent(); 
            NewUser = new BO.User();
            this.CurrentUser = CurrentUser;
            EngineerList = s_bl.Engineer.ReadAll();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            NavigationService.GoBack();            
            if (mainWindow != null)
            {
                // Accessing the property
                mainWindow.Loading = true;
                //Thread.Sleep(4000);
                //mainWindow.Loading = false;
            }
        }

        private void SelectionChanged_FilterBox(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Experience == BO.EngineerExperience.None) ?
                 s_bl.Engineer.ReadAll() : s_bl.Engineer.ReadAll(e => e.Level == Experience);
        }

        private void ItemSelected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
            NavigationService.Navigate(new EngineerPage(CurrentUser ,engineer!.Id));           
            EngineerList = s_bl.Engineer.ReadAll();
        }

        private void AddNewEngineer_BtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EngineerPage(CurrentUser));
        }

        private void EngineerOptions_ComboBoxItemSelected(object sender, MouseButtonEventArgs e)
        {
            if (sender is ComboBoxItem comboBoxItem)
            {
                // Access the DataContext (which should be a Task)
                BO.Engineer engineer = comboBoxItem.DataContext as BO.Engineer;
                if (engineer != null)
                {
                    switch ((string)comboBoxItem.Content)
                    {
                        case "Edit":
                            NavigationService.Navigate(new EngineerPage(CurrentUser, engineer!.Id));
                            break;
                        case "Delete":
                            string msg = "";
                            if (CurrentUser.UserType != BO.UserType.Admin)
                                msg = "You dont have permission to delete";
                            else if (s_bl.Task.ProjectHasStarted())
                                msg = "Project has stared. cannot delete";
                            else
                            {
                                try
                                {
                                    s_bl.Engineer.Delete(engineer.Id);
                                    EngineerList = s_bl.Engineer.ReadAll();
                                    MessageBox.Show("Deletion Successful", "Success", MessageBoxButton.OK, MessageBoxImage.None);
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case "Add User":
                            if (CurrentUser.UserType != BO.UserType.Admin)
                            {
                                MessageBox.Show("You dont have permission to creat users", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            NewUser.Id = engineer.Id;
                            NewUser.UserType = BO.UserType.Engineer;
                            NewUser.Engineer = engineer;
                            AddUserBorder.Visibility = Visibility.Visible;                              
                            break;
                    }                                
                }
            }
        }

        private void DeletesAllEngineers_BtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("All Engineers will be deleted, Are you sure you want to proceed?", "Are you sure?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes) 
                { 
                    s_bl.Engineer.Reset();
                    MessageBox.Show("All Engineers Successfuly Deleted", "Success", MessageBoxButton.OK, MessageBoxImage.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseAddUserPopup_btnClick(object sender, RoutedEventArgs e)
        {
            AddUserBorder.Visibility = Visibility.Collapsed;
        }

        private void CreatUser_btnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.User.Add(NewUser);
                MessageBox.Show("Successfuly Created a New User Profile", "Success", MessageBoxButton.OK, MessageBoxImage.None);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            NewUser = new BO.User();
            AddUserBorder.Visibility = Visibility.Collapsed;
        }

        private void PasswordUpdated_TextBoxChanged(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            EngineerList = s_bl.Engineer.ReadAll();
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
                            EngineerList = EngineerList.OrderBy(e => e.Name);
                            break;
                        case "Level":
                            EngineerList = EngineerList.OrderBy(e => e.Level);
                            break;
                        case "Cost":
                            EngineerList = EngineerList.OrderBy(e => e.Cost);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        static BitmapImage BytesToImage(byte[] bytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private void TextChanged_SearchTextBox(object sender, TextChangedEventArgs e)
        {
            if (sender is CustomControls.CustomTextBox textBox)
            {
                EngineerList = (from eng in EngineerList
                                where eng.Name.StartsWith(textBox.Text, StringComparison.OrdinalIgnoreCase)
                                select eng);
            }
        }
    }
}
