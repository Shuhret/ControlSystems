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
using System.Drawing;
using System.Threading;
using System.Windows.Media.Animation;

namespace ShuhratControlLibrary.Controls.Loaders
{
    /// <summary>
    /// Логика взаимодействия для LoaderArc.xaml
    /// </summary>
    public partial class LoaderArc : UserControl
    {
        public LoaderArc()
        {
            InitializeComponent();

            Thread thread = new Thread(new ThreadStart(RunAnimation));
            //thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;

            thread.Start();
        }

        void RunAnimation()
        {
            Action ac = () =>
            {
                Storyboard sb = this.FindResource("loop") as Storyboard; // AAA-Название анимации
                sb.Begin(); // Запустить анимацию
            };
            Dispatcher.Invoke(ac);
        }
    }
}
