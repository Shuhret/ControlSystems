using ControlSystemsLibrary.Views.ATI_Contents;
using ControlSystemsLibrary.Views.Authorization;
using ShuhratControlLibrary.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

namespace ControlSystemsLibrary.Views.Admin
{


    public partial class Administrator : UserControl, INotifyPropertyChanged
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

        #region == ПОЛЯ ===================================================================================================

        private string ConnectionString;

        LoginControl LoginCC;
       
        #endregion --------------------------------------------------------------------------------------------------------



        #region == КОНСТРУКТОР ============================================================================================
        public Administrator(LoginControl LoginCC, string ConnectionString)
        {
            this.LoginCC = LoginCC;
            this.ConnectionString = ConnectionString;

            InitializeComponent();

            ConnectionName = XmlClass.GetCurrentUserName();
            ConnectionName += " - ";
            ConnectionName += XmlClass.GetSelectedConnectionName();
        }

        #endregion --------------------------------------------------------------------------------------------------------



        private string connectionName;
        public string ConnectionName
        {
            get => connectionName;
            set
            {
                connectionName = value;
                OnPropertyChanged();
            }
        }



        #region Открывание меню ************************************************************************************

        int LastSelectMenuPage = 1;
        private void Menu_MouseEnter(object sender, MouseEventArgs e)
        {
            if (MenuGrid.Width == 66)
            {
                MenuTC.SelectedIndex = LastSelectMenuPage;
                Storyboard sb = this.FindResource("OpenMenu") as Storyboard;
                sb.Begin(); // Запустить анимацию


            }
        }

        private void Menu_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MenuGrid.Width == 310 && MenuCheckBox.IsChecked == false)
            {
                Storyboard sb = this.FindResource("CloseMenu") as Storyboard;
                sb.Begin(); // Запустить анимацию

            }
        }

        private void MenuMouseMove(object sender, MouseEventArgs e)
        {
            Menu_MouseEnter(sender, e);
        }

        private void Menu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MenuGrid.Width == 66 && e.WidthChanged)
            {
                LastSelectMenuPage = MenuTC.SelectedIndex;
                MenuTC.SelectedItem = null;
            }
        }

        private void GridWorker_MouseMove(object sender, MouseEventArgs e)
        {
            Menu_MouseLeave(sender, e);
        }

        private void MenuCheckBoxCheck(object sender, RoutedEventArgs e)
        {

            //Storyboard sb = this.FindResource("CheckLine") as Storyboard;
            //sb.Begin(); // Запустить анимацию
        }
        #endregion




        // Событие: Клик кнопки с меню
        private void MenuButtonsClick(object sender, EventArgs e)
        {
            if (sender is ButtonCheckBox)
            {
                ButtonCheckBox BCB = sender as ButtonCheckBox;
                if (CheckOpenATI(GetATIName(BCB.Name)))
                {
                    AdminTabItem ATI = new AdminTabItem();
                    ATI.Name = GetATIName(BCB.Name);
                    ATI.Icon = Icons.GetIcon(GetATIIconName(BCB.Name));
                    ATI.IconSize = 18;
                    ATI.HeaderText = GetATIHeaderText(BCB.Name);
                    ATI.StateColor = GetATIStateColor(BCB.Name);
                    ATI.Content = GetATIContent(BCB.Name);
                    AdminTabControl.Items.Add(ATI);
                }
            }
        }

        private bool CheckOpenATI(string ATIName)
        {
            bool ok = true;
            if (AdminTabControl.Items.Count > 0)
            {
                foreach (AdminTabItem ATI in AdminTabControl.Items)
                {
                    if (ATIName == ATI.Name)
                    {
                        AdminTabControl.SelectedItem = ATI;
                        ok = false;
                        break;
                    }
                }
            }
            return ok;
        }

        private string GetATIName(string ButtonName)
        {
            switch (ButtonName)
            {
                case "MenuButtonDocumentsJournal": return "DocumentsJournalATI";
                case "MenuButtonZayavkaPostavshiku": return "ZayavkaPostavshikuATI";
                case "MenuButtonPostuplenie": return "PostuplenieATI";
                case "MenuButtonVozvratPostavshiku": return "VozvratPostavshikuATI";
                case "MenuButtonOtchetPostavshiku": return "OtchetPostavshikuTI";

                case "MenuButtonNomenclature": return "NomenclatureTI";
                default: return "";
            }
        }

        private string GetATIIconName(string ButtonName)
        {
            switch(ButtonName)
            {
                case "MenuButtonDocumentsJournal": return "DocumentsJournal";
                case "MenuButtonZayavkaPostavshiku": return "Заявка поставщику";
                case "MenuButtonPostuplenie": return "Поступление";
                case "MenuButtonVozvratPostavshiku": return "Возврат поставщику";
                case "MenuButtonOtchetPostavshiku": return "Отчет поставщику";

                case "MenuButtonNomenclature": return "Номенклатура";
                default: return "DefaultIcon";
            }
        }

        private string GetATIHeaderText(string ButtonName)
        {
            switch (ButtonName)
            {
                case "MenuButtonDocumentsJournal": return "Журнал документов";
                case "MenuButtonZayavkaPostavshiku": return "Заявка поставщику";
                case "MenuButtonPostuplenie": return "Поступление";
                case "MenuButtonVozvratPostavshiku": return "Возврат поставщику";
                case "MenuButtonOtchetPostavshiku": return "Отчет поставщику";

                case "MenuButtonNomenclature": return "Номенклатура";
                default: return "Хуевознает";
            }
        }

        private SolidColorBrush GetATIStateColor(string ButtonName)
        {
            switch (ButtonName)
            {
                //case "MenuButtonDocumentsJournal": return GetColors.Get("Yellow-002");
                case "MenuButtonZayavkaPostavshiku": return GetColors.Get("Light-005");
                case "MenuButtonPostuplenie": return GetColors.Get("Blue-002");
                case "MenuButtonVozvratPostavshiku": return GetColors.Get("Purple-001");
                case "MenuButtonOtchetPostavshiku": return GetColors.Get("Green-002");

                
                default: return null; 
            }
        }

        private UserControl GetATIContent(string ButtonName)
        {

            switch (ButtonName)
            {
                //case "MenuButtonDocumentsJournal": return "Журнал документов";
                //case "MenuButtonZayavkaPostavshiku": return "Заявка поставщику";
                //case "MenuButtonPostuplenie": return new ATIC_Postuplenie();
                //case "MenuButtonVozvratPostavshiku": return "Возврат поставщику";
                //case "MenuButtonOtchetPostavshiku": return "Отчет поставщику";

                case "MenuButtonNomenclature": return new ATI_Nomenclature();
                default: return null;
            }
        }

        private void TopButtonClick(object sender, RoutedEventArgs e)
        {
            LoginCC.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
