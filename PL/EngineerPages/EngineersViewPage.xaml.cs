using PL.Engineer;
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

namespace PL.EngineerPages
{
    /// <summary>
    /// Interaction logic for EngineersViewPage.xaml
    /// </summary>
    public partial class EngineersViewPage : Page
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.None;
        public IEnumerable<BO.Engineer>? EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }
        // Using a DependencyProperty as the backing store for EngineerList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineersViewPage), new PropertyMetadata(null));

        public EngineersViewPage()
        {
            InitializeComponent();
            EngineerList = s_bl.Engineer.ReadAll();
            //EngineerList = (from t in s_bl.Engineer.ReadAll()
            //                select new PL.EngineerInList() 
            //                {Id = t.Id, Name = t.Name, Cost = t.Cost, Email= t.Email, Level = t.Level, Task = t.Task});
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
            NavigationService.Navigate(new EngineerPage(engineer!.Id));           
            EngineerList = s_bl.Engineer.ReadAll();
        }

        private void AddNewEngineer_BtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EngineerPage());
        }

        private void EngineerOptions_ComboBoxItemSelected(object sender, MouseButtonEventArgs e)
        {
            if (sender is ComboBoxItem comboBoxItem)
            {
                // Access the DataContext (which should be a Task)
                BO.Engineer engineer = comboBoxItem.DataContext as BO.Engineer;

                if ((string)comboBoxItem.Content == "Edit")
                {
                    NavigationService.Navigate(new EngineerPage(engineer!.Id));
                }

                if ((string)comboBoxItem.Content == "Delete")
                {

                }

                if (engineer != null)
                {

                }
            }
        }

        private void DeletesAllEngineers_BtnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
