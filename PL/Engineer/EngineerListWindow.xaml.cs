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
using System.Windows.Shapes;

namespace PL.Engineer;

/// <summary>
/// Interaction logic for EngineerListWindow.xaml
/// </summary>
public partial class EngineerListWindow : Window
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
        DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

    public EngineerListWindow()
    {
        InitializeComponent();
        EngineerList = s_bl.Engineer.ReadAll();
    }

    private void engExperienceFilterChanged_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EngineerList = (Experience == BO.EngineerExperience.None) ?
             s_bl.Engineer.ReadAll() : s_bl.Engineer.ReadAll(e => e.Level == Experience);
    }

    private void creatNewEngineer_btnClick(object sender, RoutedEventArgs e)
    {
        new Engineer.EngineerWindow().ShowDialog();
        EngineerList = s_bl.Engineer.ReadAll();
    }

    private void listItemSelected_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        BO.Engineer engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
        new Engineer.EngineerWindow(engineer!.Id).ShowDialog();
        EngineerList = s_bl.Engineer.ReadAll();
    }

    private void ListView_SourceUpdated(object sender, DataTransferEventArgs e)
    {
        EngineerList = s_bl.Engineer.ReadAll();
    }
}
