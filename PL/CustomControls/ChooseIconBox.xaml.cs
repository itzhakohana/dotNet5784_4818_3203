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
using static System.Net.Mime.MediaTypeNames;

namespace PL.CustomControls
{
    /// <summary>
    /// Interaction logic for ChooseIconBox.xaml
    /// </summary>
    public partial class ChooseIconBox : UserControl
    {

        public event EventHandler<EventArgs> OnIconChosed;

        public BitmapImage MyImage
        {
            get { return (BitmapImage)GetValue(MyImageProperty); }
            set { SetValue(MyImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyImageProperty =
            DependencyProperty.Register("MyImage", typeof(BitmapImage), typeof(ChooseIconBox), new PropertyMetadata(null));



        public ChooseIconBox()
        {
            InitializeComponent();
            MyImage = Util.BytesToImage(System.Convert.FromBase64String(Util.DEFAULT_PROFILE_PICTURE));
        }

        public void Open()
        {
            this.Visibility = Visibility.Visible;
        }
        
        public void Close()
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void GoBack_BtnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Save_btnClick(object sender, RoutedEventArgs e)
        {
            if (IconsCollection.SelectedItem != null && IconsCollection.SelectedItem is ImageListItem image)
            {
                MyImage = Util.ConvertToBitmapImage(image.Image);
                OnIconChosed?.Invoke(this, new EventArgs());
            }
            this.Close();
        }
    }
}
