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
    /// <summary>
    /// Interaction logic for MilestonePage.xaml
    /// </summary>
    public partial class MilestonePage : Page
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private int _id;


        public BO.Milestone Milestone
        {
            get { return (BO.Milestone)GetValue(MilestoneProperty); }
            set { SetValue(MilestoneProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Milestone.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MilestoneProperty =
            DependencyProperty.Register("Milestone", typeof(BO.Milestone), typeof(MilestonePage), new PropertyMetadata(null));



        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentUserProperty =
            DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(MilestonePage), new PropertyMetadata(null));


        public bool ProjectHasStarted
        {
            get { return (bool)GetValue(ProjectHasStartedProperty); }
            set { SetValue(ProjectHasStartedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjctHasStarted.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectHasStartedProperty =
            DependencyProperty.Register("ProjectHasStarted", typeof(bool), typeof(MilestonePage), new PropertyMetadata(null));

        public MilestonePage(BO.User user, int id)
        {
            InitializeComponent();
            _id = id;
            CurrentUser = user;
            Milestone = s_bl.Milestone.Read(_id) ?? new BO.Milestone();
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

        private void GoBack_BtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void UpdateMilestone_btnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Milestone.Update(Milestone.Id, Milestone.Alias, Milestone.Description, Milestone.Remarks);
                NavigationService.GoBack();
                MessageBox.Show("Successfuly Updated milestone", default, MessageBoxButton.OK, MessageBoxImage.None);
            }            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetChanges_btnClick(object sender, RoutedEventArgs e)
        {
            Milestone = s_bl.Milestone.Read(_id) ?? new BO.Milestone();
        }
    }
}
