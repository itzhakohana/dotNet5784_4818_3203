using Microsoft.Win32;
using PL.TaskPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace PL.EngineerPages
{
    /// <summary>
    /// Interaction logic for EngineerPage.xaml
    /// </summary>
    public partial class EngineerPage : Page
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private readonly bool _inAddMode;
        private readonly int _engineerId;
        public ObservableCollection<BO.TaskInList>? AvailableTasks { get; set; }



        public BitmapImage MyImage
        {
            get { return (BitmapImage)GetValue(MyImageProperty); }
            set { SetValue(MyImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyImageProperty =
            DependencyProperty.Register("MyImage", typeof(BitmapImage), typeof(EngineerPage), new PropertyMetadata(null));


        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(EngineerPage), new PropertyMetadata(null));



        public bool ProjectHasStarted
        {
            get { return (bool)GetValue(ProjectHasStartedProperty); }
            set { SetValue(ProjectHasStartedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjctHasStarted.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectHasStartedProperty =
            DependencyProperty.Register("ProjectHasStarted", typeof(bool), typeof(EngineerPage), new PropertyMetadata(null));


        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Engineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerPage), new PropertyMetadata(null));


        BitmapImage IconBoxImage;


        public EngineerPage(BO.User user, int id = 0)
        {
            InitializeComponent();
            IconsBox.OnIconChosed += IconsBox_IconChosed;
            _engineerId = id;
            NameTextBox.Focus();
            if (id == 0)
            { 
                Engineer = new BO.Engineer() {Level = (BO.EngineerExperience)1, Picture = Util.DEFAULT_PROFILE_PICTURE };
                AvailableTasks = new ObservableCollection<BO.TaskInList>(from t in s_bl.Task.ReadAll(s => s.Complexity <= Engineer.Level && !s.IsMilestone && s.Engineer is null)
                                                                         select new BO.TaskInList() { Id = t.Id, Alias = t.Alias, Description = t.Description, Status = t.Status });
                AvailableTasksDisplay.ItemsSource = AvailableTasks;
                _inAddMode = true;
                //tasksComboBox.ItemsSource = AvailableTasks;
            }
            else
            {
                Engineer = s_bl.Engineer.Read(id)!;
                AvailableTasks = new ObservableCollection<BO.TaskInList>(from t in s_bl.Task.ReadAll(s => s.Complexity <= Engineer.Level && !s.IsMilestone && s.Engineer is null && s.Status != BO.Status.Done)
                                                                         select new BO.TaskInList() { Id = t.Id, Alias = t.Alias, Description = t.Description, Status = t.Status });
                AvailableTasksDisplay.ItemsSource = AvailableTasks;
                _inAddMode = false;
            }
            CurrentUser = user;
            ProjectHasStarted = s_bl.Task.ProjectHasStarted();
            MyImage = Util.BytesToImage(Convert.FromBase64String(Engineer.Picture));
        }

        private void IconsBox_IconChosed(object? sender, EventArgs e)
        {
            MyImage = IconsBox.MyImage;
            byte[] imageBytes = Util.ImageToBytes(MyImage);
            Engineer.Picture = Convert.ToBase64String(imageBytes);
        }

        private void AddOrUpdateEngineer_btnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_inAddMode)
                {
                    s_bl.Engineer.Add(Engineer);
                    NavigationService.GoBack();
                    MessageBox.Show("Successfuly Added Engineer", default, MessageBoxButton.OK, MessageBoxImage.None);
                }
                else
                {
                    s_bl.Engineer.Update(Engineer);
                    NavigationService.GoBack();
                    MessageBox.Show("Successfuly Updated Engineer", default, MessageBoxButton.OK, MessageBoxImage.None);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetChanges_btnClick(object sender, RoutedEventArgs e)
        {
            if (_inAddMode)                
                Engineer = new BO.Engineer();
            else
                Engineer = s_bl.Engineer.Read(_engineerId)!;
            if (Engineer.Picture is null || Engineer.Picture == "")
                Engineer.Picture = Util.DEFAULT_PROFILE_PICTURE;
            MyImage = Util.BytesToImage(Convert.FromBase64String(Engineer.Picture));
        }

        private void GoBack_BtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void EnteredKey_IdFieldChanged(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void LevelChanged_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AvailableTasks = new ObservableCollection<BO.TaskInList>(from t in s_bl.Task.ReadAll(s => s.Complexity <= Engineer.Level && !s.IsMilestone && s.Engineer is null && s.Status != BO.Status.Done)
                                                                     select new BO.TaskInList() { Id = t.Id, Alias = t.Alias, Description = t.Description, Status = t.Status });
            AvailableTasksDisplay.ItemsSource = AvailableTasks;
            if (Engineer.Task != null && Engineer.Level < s_bl.Task.Read(Engineer.Task.Id).Complexity)
                Engineer.Task = null;
        }

        private void TaskSelected_ListView(object sender, SelectionChangedEventArgs e)
        {
            var list = sender as ListView;
            var item = list.SelectedItem;
            if (item is BO.TaskInList)
            {
                BO.TaskInList task = item as BO.TaskInList;
                Engineer = new BO.Engineer()
                {
                    Id = Engineer.Id,
                    Name = Engineer.Name,
                    Cost = Engineer.Cost,
                    Email = Engineer.Email,
                    Level = Engineer.Level,
                    Picture = Engineer.Picture,
                    Task = new BO.TaskInEngineer() { Alias = task!.Alias, Id = task.Id }
                };
            }
        }

        private void ResetAssignedTask_btnClick(object sender, RoutedEventArgs e)
        {
            if (_inAddMode)
                Engineer.Task = null;
            else
            {
                Engineer.Task = null;                
            }
            
        }

        private void UpdateProfilePicture_btnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Files|*.png",
                InitialDirectory = @"C:\"
            };
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri(dialog.FileName, UriKind.Absolute));

                    // Convert the image to a byte array
                    byte[] imageBytes = Util.ImageToBytes(bitmapImage);

                    // Encode the byte array as Base64
                    Engineer.Picture = Convert.ToBase64String(imageBytes);


                    // Convert the Base64 string back to a byte array
                    byte[] imageBytes1 = Convert.FromBase64String(Engineer.Picture);

                    // Convert the byte array to a BitmapImage
                    MyImage = Util.BytesToImage(imageBytes1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void ChoosePreMadeIcon_btnClick(object sender, RoutedEventArgs e)
        {
            IconsBox.Open();
        }
    }
}
