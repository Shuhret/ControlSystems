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

namespace ControlSystemsLibrary.Views
{
    /// <summary>
    /// Логика взаимодействия для SelectUserInterface.xaml
    /// </summary>
    public partial class SelectUserInterface : UserControl
    {
        private ContentControl LoginCC;
        private Grid WorkerGrid;
        private string ConnectionString;
        private string UserControlName;

        public SelectUserInterface(ContentControl LoginCC, Grid WorkerGrid, string ConnectionString, string UserControlName)
        {
            this.LoginCC = LoginCC;
            this.WorkerGrid = WorkerGrid;
            this.ConnectionString = ConnectionString;
            this.UserControlName = UserControlName;

            InitializeComponent();
        }
    }
}
