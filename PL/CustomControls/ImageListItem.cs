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
    
    public class ImageListItem : ListBoxItem
    {



        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageListItem), new PropertyMetadata(null));



        public BitmapImage BitImage
        {
            get { return (BitmapImage)GetValue(BitImageProperty); }
            set { SetValue(BitImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BitImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BitImageProperty =
            DependencyProperty.Register("BitImage", typeof(BitmapImage), typeof(ImageListItem), new PropertyMetadata(null));



        static ImageListItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageListItem), new FrameworkPropertyMetadata(typeof(ImageListItem)));
        }
    }
}
