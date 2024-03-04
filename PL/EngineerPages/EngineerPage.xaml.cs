using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<BO.TaskInEngineer> AvailableTasks { get; set; }

        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Engineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerPage), new PropertyMetadata(null));



        public EngineerPage(int id = 0)
        {
            InitializeComponent();
            _engineerId = id;
            if (id == 0)
            { 
                Engineer = new BO.Engineer() {Level = (BO.EngineerExperience)1 };
                AvailableTasks = new ObservableCollection<BO.TaskInEngineer>();
                _inAddMode = true;
                tasksComboBox.ItemsSource = AvailableTasks;
            }
            else
            {
                Engineer = s_bl.Engineer.Read(id)!;
                AvailableTasks = new ObservableCollection<BO.TaskInEngineer> (from t in s_bl.Task.ReadAllAvailableTasks(id)
                                  select new BO.TaskInEngineer() { Id = t.Id, Alias = t.Alias });
                _inAddMode = false;
                tasksComboBox.ItemsSource = AvailableTasks;
            }
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
    }
}
