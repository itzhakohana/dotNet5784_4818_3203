using PL.CustomControls;
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

namespace PL.MilestonePages
{   
    public partial class MilestonesView : Page
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public bool Loading
        {
            get { return (bool)GetValue(LoadingProperty); }
            set { SetValue(LoadingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Loading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadingProperty =
            DependencyProperty.Register("Loading", typeof(bool), typeof(MilestonesView), new PropertyMetadata(null));


        public IEnumerable<BO.Milestone>? MilestonesList
        {
            get { return (IEnumerable<BO.Milestone>?)GetValue(MilestonesListProperty); }
            set { SetValue(MilestonesListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MilestonesList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MilestonesListProperty =
            DependencyProperty.Register("MilestonesList", typeof(IEnumerable<BO.Milestone>), typeof(MilestonesView), new PropertyMetadata(null));



        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(MilestonesView), new PropertyMetadata(null));

        private async void reloadData()
        {
            Dispatcher.Invoke(() => Loading = true);            
            IEnumerable<BO.Milestone>? tmp = null;
            await Task.Run(() => {tmp = s_bl.Milestone.ReadAll().OrderBy(m => m.ForecastDate); });
            Dispatcher.Invoke(() => { Loading = false; MilestonesList = tmp;});            
        }

        public MilestonesView(BO.User user)
        {
            InitializeComponent();
            CurrentUser = user;
            Task.Run(() => reloadData());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            reloadData();
        }

        private void MilestoneOptions_ComboBoxItemSelected(object sender, MouseButtonEventArgs e)
        {

        }

        private void DependentTaskSelectedForView_btn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as IconButton;            
            if (btn != null && btn.DataContext is BO.TaskInList)
            {
                BO.TaskInList task = btn.DataContext as BO.TaskInList;
                NavigationService.Navigate(new TaskPages.TaskPage(CurrentUser, task!.Id));
            }
        }

        private void ItemSelected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Milestone milestone = (sender as ListView)?.SelectedItem as BO.Milestone;
            if (milestone != null)
                NavigationService.Navigate(new MilestonePage(CurrentUser, milestone.Id));
        }

       
    }
}
