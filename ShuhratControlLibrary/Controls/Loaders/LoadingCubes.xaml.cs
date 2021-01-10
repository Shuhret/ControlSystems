using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ShuhratControlLibrary.Controls.Loaders
{
    public partial class LoadingCubes : UserControl
    {
        public LoadingCubes()
        {
            InitializeComponent();

            Thread thread = new Thread(new ThreadStart(RunAnimation));
            thread.SetApartmentState(ApartmentState.STA);
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




        public string WaitText
        {
            get { return (string)GetValue(WaitTextProperty); }
            set { SetValue(WaitTextProperty, value); }
        }
        public static readonly DependencyProperty WaitTextProperty = DependencyProperty.Register("WaitText", typeof(string), typeof(LoadingCubes), new PropertyMetadata(string.Empty));


        public SolidColorBrush CubesColor
        {
            get { return (SolidColorBrush)GetValue(CubesColorProperty); }
            set { SetValue(CubesColorProperty, value); }
        }
        public static readonly DependencyProperty CubesColorProperty = DependencyProperty.Register("CubesColor", typeof(SolidColorBrush), typeof(LoadingCubes), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

    }
}
