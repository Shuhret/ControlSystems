using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShuhratControlLibrary.Controls
{
    /// <summary>
    /// Логика взаимодействия для BarcodeUC.xaml
    /// </summary>
    public partial class BarcodeUC : UserControl, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        #endregion


        public BarcodeUC()
        {
            InitializeComponent();
        }


        private BitmapSource bacodeImage;
        public BitmapSource BacodeImage
        {
            get => bacodeImage;
            set
            {
                bacodeImage = value;
                OnPropertyChanged();
            }
        }

        private bool readiness;
        public bool Readiness
        {
            get
            {
                if (readiness == false)
                {
                    Storyboard sb = this.FindResource("ReadinessFalse") as Storyboard;
                    sb.Begin();
                }
                return readiness;
            }
            set
            {
                readiness = value;
                OnPropertyChanged();
            }
        }

        private string unitName;
        public string UnitName
        {
            get => unitName;
            set
            {
                unitName = value;
                OnPropertyChanged();
            }
        }

        private string barcodeType;
        public string BarcodeType
        {
            get => barcodeType;
            set
            {
                barcodeType = value;
                OnPropertyChanged();
            }
        }

        private string barcode;
        public string Barcode
        {
            get => barcode;
            set
            {
                barcode = value;
                OnPropertyChanged();
            }
        }



        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Parent is StackPanel)
                ((StackPanel)Parent).Children.Remove(this);
            if (Parent is WrapPanel)
                ((WrapPanel)Parent).Children.Remove(this);
        }
    }
}
