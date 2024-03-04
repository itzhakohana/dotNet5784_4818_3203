using PL.CustomControls;
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
        private bool isResizing = false;
        private Point resizeStartPoint;
        public readonly BO.UserType CurrentUser;

        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, value); }
        } 

        // Using a DependencyProperty as the backing store for Loading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register("Loading", typeof(bool), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
            Loading = false;
            CurrentUser = BO.UserType.Admin;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        private void GridClick_btnClick(object sender, RoutedEventArgs e)
        {
            var myClickedButton = e.OriginalSource as NavigationButton;
            if (myClickedButton != null && myClickedButton.NextPage != null)
                MainFrame.NavigationService.Navigate(myClickedButton.NextPage);
        }

        private void Exit_btnClick(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Minimize_btnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        //private void showEngineerList_btnClick(object sender, RoutedEventArgs e)
        //{
        //    new Engineer.EngineerListWindow().ShowDialog();
        //}

        //private void initiateDataBase_btnClick(object sender, RoutedEventArgs e)
        //{

        //    MessageBoxResult mbResult =
        //        MessageBox.Show("Are you sure you want to initialize with random data?", "Validate Action", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (mbResult == MessageBoxResult.Yes)
        //    {
        //        Loading = true;
        //        s_bl.InitializeDataBase();
        //        Loading = false;
        //        MessageBox.Show("Successfuly Initialized Data-Base", "Success", MessageBoxButton.OK, MessageBoxImage.None);
        //    }
        //    ;
        //}

        //private void resetDataBase_btnClick(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult mbResult =
        //        MessageBox.Show("Are you sure you want to reset the data-base?", "Validate Action", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (mbResult == MessageBoxResult.Yes)
        //    {
        //        s_bl.Reset();
        //        MessageBox.Show("Reset Successfuly Completed", "Success", MessageBoxButton.OK, MessageBoxImage.None);
        //    }
        //}
    }
}