namespace PL.Engineer; 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;



public partial class EngineerWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private readonly bool _inAddMode;
    private readonly int _engineerId;
    public List<BO.Task>? AvailableTasks { get; set; }

    public BO.Engineer MyEngineer
    {
        get { return (BO.Engineer)GetValue(MyEngineerProperty); }
        set { SetValue(MyEngineerProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyEngineer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyEngineerProperty =
        DependencyProperty.Register("MyEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

    public EngineerWindow(int id = 0)
    {
        InitializeComponent();
        if (id == 0)
        {
            MyEngineer = new BO.Engineer();
            _inAddMode = true;
        }
        else
            try
            {
                MyEngineer = s_bl.Engineer.Read(id)
                            ?? throw new BO.BlDoesNotExistException($"Engineer with Id {id} not found");
                _inAddMode = false;
                _engineerId = id;
            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
    }

    private void addOrUpdateEngineer_btnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (_inAddMode)
            {
                s_bl.Engineer.Add(MyEngineer);
                this.Close();
                MessageBox.Show("Successfuly Added Engineer", default, MessageBoxButton.OK, MessageBoxImage.None);                
            }
            else 
            { 
                s_bl.Engineer.Update(MyEngineer);
                this.Close();
                MessageBox.Show("Successfuly Updated Engineer", default, MessageBoxButton.OK, MessageBoxImage.None);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void resetChanges_btnClick(object sender, RoutedEventArgs e)
    {
        if (_inAddMode)
            MyEngineer = new BO.Engineer();
        else
            MyEngineer = s_bl.Engineer.Read(_engineerId)!;
    }

    private void changedExpertiseLevel_selectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (MyEngineer.Level == BO.EngineerExperience.None)
            MyEngineer.Level = BO.EngineerExperience.Beginner;
    }

    private void enteredKey_idFieldChanged(object sender, TextCompositionEventArgs e)
    {
        if (!int.TryParse(e.Text, out _))
        {
            e.Handled = true; 
        }
    }
}
