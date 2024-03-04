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

namespace PL.ProjectPages
{
    /// <summary>
    /// Interaction logic for StartProjectPage.xaml
    /// </summary>
    public partial class StartProjectPage : Page
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();



        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Loading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register("Loading", typeof(bool), typeof(StartProjectPage), new PropertyMetadata(null));



        private DateTime _startDate
        {
            get { return (DateTime)GetValue(_startDateProperty); }
            set { SetValue(_startDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _startDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _startDateProperty =
            DependencyProperty.Register("_startDate", typeof(DateTime), typeof(StartProjectPage), new PropertyMetadata(null));


        private DateTime _deadlineDate
        {
            get { return (DateTime)GetValue(_deadlineDateProperty); }
            set { SetValue(_deadlineDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _deadlineDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _deadlineDateProperty =
            DependencyProperty.Register("_deadlineDate", typeof(DateTime), typeof(StartProjectPage), new PropertyMetadata(null));




        public StartProjectPage()
        {
            InitializeComponent();
            Loading = false;
            if(s_bl.DateControl.GetCurrentDate() is null)
                s_bl.DateControl.SetCurrentDate(DateTime.Now);
        }

        private void GoBack_BtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        async private void StartProject_BtnClick(object sender, RoutedEventArgs e)
        {
            DateTime startDate = _startDate;
            DateTime deadlineDate = _deadlineDate;
            Loading = true;
            try
            {
                await Task.Run(() => {  s_bl.Milestone.StartProject(startDate, deadlineDate);  }) ;

                Dispatcher.Invoke(() =>
                {
                    Loading = false;
                    NavigationService.GoBack();
                    MessageBox.Show("Successfully Started the Project", default, MessageBoxButton.OK, MessageBoxImage.None);
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    Loading = false;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
    }
}

