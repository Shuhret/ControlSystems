using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing;
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
using BarcodeLib;

namespace ShuhratControlLibrary.Controls
{
    public partial class BarcodeView : UserControl
    {


        public BarcodeView(string UnitName, string BarcodeType, string Barcode, BitmapSource BacodeImage)
        {
            this.UnitName = UnitName;
            this.BarcodeType = BarcodeType;
            this.Barcode = Barcode;
            this.BacodeImage = BacodeImage;

            InitializeComponent();
            
        }




        public BitmapSource BacodeImage
        {
            get { return (BitmapSource)GetValue(BacodeImageProperty); }
            set { SetValue(BacodeImageProperty, value); }
        }
        public static readonly DependencyProperty BacodeImageProperty =
            DependencyProperty.Register("BacodeImage", typeof(BitmapSource), typeof(BarcodeView), new PropertyMetadata(null));








        public string UnitName
        {
            get { return (string)GetValue(UnitNameProperty); }
            set { SetValue(UnitNameProperty, value); }
        }
        public static readonly DependencyProperty UnitNameProperty =  DependencyProperty.Register("UnitName", typeof(string), typeof(BarcodeView), new PropertyMetadata(string.Empty));


        public string BarcodeType
        {
            get { return (string)GetValue(BarcodeTypeProperty); }
            set { SetValue(BarcodeTypeProperty, value); }
        }
        public static readonly DependencyProperty BarcodeTypeProperty = DependencyProperty.Register("BarcodeType", typeof(string), typeof(BarcodeView), new PropertyMetadata(string.Empty));


        public string Barcode
        {
            get { return (string)GetValue(BarcodeProperty); }
            set { SetValue(BarcodeProperty, value); }
        }
        public static readonly DependencyProperty BarcodeProperty = DependencyProperty.Register("Barcode", typeof(string), typeof(BarcodeView), new PropertyMetadata(string.Empty));

        // Клик: Кнопки "Удалить"
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ((StackPanel)Parent).Children.Remove(this);
        }
        
    }
}
