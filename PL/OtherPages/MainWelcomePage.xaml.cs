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

namespace PL.OtherPages
{
    /// <summary>
    /// Interaction logic for MainWelcomePage.xaml
    /// </summary>
    public partial class MainWelcomePage : Page
    {
        public MainWelcomePage()
        {
            InitializeComponent();
        }

        private void HowToUseOpener_btnClick(object sender, MouseButtonEventArgs e)
        {
            HowToUseGuide.Open();
        }

        private void AboutTheMakersOpener_btnClick(object sender, MouseButtonEventArgs e)
        {
            AboutTheMakersBox.Open();
        }

        private void CloseGuidesButton_Click(object sender, RoutedEventArgs e)
        {
            HowToUseGuide.Visibility = Visibility.Collapsed;
            AboutTheMakersBox.Visibility = Visibility.Collapsed;
        }
    }
}
