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
    public partial class NomenPropertyUC : UserControl, INotifyPropertyChanged
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


        public NomenPropertyUC()
        {
            InitializeComponent();
        }



        private bool readiness;
        public bool Readiness
        {
            get
            {
                CheckReadiness();
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




        private void CheckReadiness()
        {
            if(PropertyComboBox.SelectedItem != null && ValueComboBox.SelectedItem != null)
            {
                Readiness = true;
            }
            else
            {
                Readiness = false;
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(Parent is StackPanel)
                ((StackPanel)Parent).Children.Remove(this);
        }


    }
}
