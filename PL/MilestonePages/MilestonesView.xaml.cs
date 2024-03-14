using PL.CustomControls;
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


        public MilestonesView(BO.User user)
        {
            InitializeComponent();
            MilestonesList = s_bl.Milestone.ReadAll();
            CurrentUser = user;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MilestonesList = s_bl.Milestone.ReadAll();
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
    }
}
