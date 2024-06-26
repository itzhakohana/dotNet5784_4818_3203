﻿using System;
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
    /// Interaction logic for HowToUseGuide.xaml
    /// </summary>
    public partial class HowToUseGuide : UserControl
    {
        public HowToUseGuide()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void Open()
        {
            this.Visibility = Visibility.Visible;
        }
    }
}
