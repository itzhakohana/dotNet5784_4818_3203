using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void showEngineerList_btnClick(object sender, RoutedEventArgs e)
        {
            new Engineer.EngineerListWindow().ShowDialog();
        }

        private void initiateDataBase_btnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult =
                MessageBox.Show("Are you sure you want to initialize with random data?", "Validate Action", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbResult == MessageBoxResult.Yes)
            {
                s_bl.InitializeDataBase();
                MessageBox.Show("Successfuly Initialized Data-Base", "Success", MessageBoxButton.OK, MessageBoxImage.None);
            }
        }

        private void resetDataBase_btnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult =
                MessageBox.Show("Are you sure you want to reset the data-base?", "Validate Action", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbResult == MessageBoxResult.Yes)
            {
                s_bl.Reset();
                MessageBox.Show("Reset Successfuly Completed", "Success", MessageBoxButton.OK, MessageBoxImage.None);
            }
        }
    }
}