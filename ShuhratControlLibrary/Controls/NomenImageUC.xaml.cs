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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShuhratControlLibrary.Controls
{

    public partial class NomenImageUC : UserControl, INotifyPropertyChanged
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


        public NomenImageUC()
        {
            InitializeComponent();

            Description = "";
        }

        //NomenImage

        private BitmapImage nomenImage;

        public BitmapImage NomenImage
        {
            get => nomenImage;
            set
            {
                nomenImage = value;
                OnPropertyChanged();
            }
        }

        private bool mainImage;

        public bool MainImage
        {
            get => mainImage;
            set
            {
                mainImage = value;
                OnPropertyChanged();
            }
        }

        private string description;

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }


        private string imagePath;

        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = value;
                OnPropertyChanged();
            }
        }



        private void PART_DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(Parent is WrapPanel)
                ((WrapPanel)Parent).Children.Remove(this);
        }

       
    }
}
