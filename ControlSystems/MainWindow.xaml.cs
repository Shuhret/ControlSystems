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
using ControlSystemsLibrary;
using ControlSystemsLibrary.Views.Authorization;
using ControlSystemsLibrary.Resources.Styles.WindowStyle;


namespace ControlSystems
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoginControl = new LoginControl(WorkerGrid);
        }


        #region Свойства -----------------------------------------------------------------------------------------------------------------------------------


        public UserControl LoginControl
        {
            get { return (UserControl)GetValue(LoginControlProperty); }
            set { SetValue(LoginControlProperty, value); }
        }
        public static readonly DependencyProperty LoginControlProperty = DependencyProperty.Register("LoginControl", typeof(UserControl), typeof(MainWindow), new PropertyMetadata(null));

        #endregion -----------------------------------------------------------------------------------------------------------------------------------------

    }
}
