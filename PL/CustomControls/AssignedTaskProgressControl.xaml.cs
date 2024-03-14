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

namespace PL.CustomControls
{
    /// <summary>
    /// Interaction logic for AssignedTaskProgressControl.xaml
    /// </summary>
    public partial class AssignedTaskProgressControl : UserControl
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();



        public TimeSpan TimeLeft
        {
            get { return (TimeSpan)GetValue(TimeLeftProperty); }
            set { SetValue(TimeLeftProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimeLeft.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeLeftProperty =
            DependencyProperty.Register("TimeLeft", typeof(TimeSpan), typeof(AssignedTaskProgressControl), new PropertyMetadata(null));



        public BO.TaskInEngineer CurrentTask
        {
            get { return (BO.TaskInEngineer)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.TaskInEngineer), typeof(AssignedTaskProgressControl), new PropertyMetadata(null));



        public BO.Task CurrentTaskDetailed
        {
            get { return (BO.Task)GetValue(CurrentTaskDetailedProperty); }
            set { SetValue(CurrentTaskDetailedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTaskDetailed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTaskDetailedProperty =
            DependencyProperty.Register("CurrentTaskDetailed", typeof(BO.Task), typeof(AssignedTaskProgressControl), new PropertyMetadata(null));



        private bool _canReportProgress
        {
            get { return (bool)GetValue(_canReportProgressProperty); }
            set { SetValue(_canReportProgressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanStartWork.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _canReportProgressProperty =
            DependencyProperty.Register("_canReportProgress", typeof(bool), typeof(AssignedTaskProgressControl), new PropertyMetadata(null));



        private bool _inDanger
        {
            get { return (bool)GetValue(_inDangerProperty); }
            set { SetValue(_inDangerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for _inDanger.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _inDangerProperty =
            DependencyProperty.Register("_inDanger", typeof(bool), typeof(AssignedTaskProgressControl), new PropertyMetadata(null));



        private void reloadData()
        {
            try
            {
                //CurrentTaskDetailed = s_bl.Task.Read(CurrentTask.Id) ?? new BO.Task();
                if (CurrentTaskDetailed != null)
                {
                    TimeLeft = s_bl.Task.TimeLeftForTask(CurrentTaskDetailed.Id);
                    _inDanger = CurrentTaskDetailed.Status == BO.Status.InJeopardy;
                    _canReportProgress = (CurrentTaskDetailed.Status == BO.Status.InJeopardy || CurrentTaskDetailed.Status == BO.Status.OnTrack || s_bl.Task.CanStartWork(CurrentTaskDetailed.Id));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public AssignedTaskProgressControl()
        {
            reloadData();
            InitializeComponent();            
        }

        private void UpdateProgress_btnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn) 
            {
                try
                {
                    switch (btn.Content)
                    {
                        case "Complete Task":
                            CurrentTaskDetailed.Status = BO.Status.Done;
                            s_bl.Task.Update(CurrentTaskDetailed);
                            break;
                        case "Start Work":
                            CurrentTaskDetailed.Status = BO.Status.OnTrack;
                            s_bl.Task.Update(CurrentTaskDetailed);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
