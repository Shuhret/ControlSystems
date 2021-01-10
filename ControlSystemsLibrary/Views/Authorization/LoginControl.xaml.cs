using ControlSystemsLibrary.Views.Admin;
using ShuhratControlLibrary.Controls;
using ShuhratControlLibrary.Controls.Loaders;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ControlSystemsLibrary.Views.Authorization
{
    public partial class LoginControl : UserControl, INotifyPropertyChanged
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
        LoadingCubes LC = new LoadingCubes() { CubesColor = GetColors.Get("Blue-004") };

       
        
        public ContentControl LoginCC;
        
        LoginUC LoginPanel = new LoginUC();
        ConnectionsListUC ConnectionListPanel = new ConnectionsListUC();
        CreateConnectionUC CreateConnectionPanel = new CreateConnectionUC();




        Grid WorkerGrid;
        #endregion --------------------------------------------------------------------------------------------------------
        #region == КОНСТРУКТОР ============================================================================================

        public LoginControl(Grid WorkerGrid)
        {
            this.WorkerGrid = WorkerGrid;

            InitializeComponent();

            LoginUserControl = LoginPanel;
            LoginPanel.Loaded += LoginPanel_Loaded;
            SetEvents();

            LoginPanel.ButtonConnectionName.Foreground = GetColors.Get("Dark-003");

            LoginPanel.txbxUserName.Text = XmlClass.GetCurrentUserName();
            LoginPanel.ButtonConnectionName.Content = XmlClass.GetSelectedConnectionName();

            if (LoginPanel.ButtonConnectionName.Content.ToString() == "")
            {
                ShowMessage("Подключение не создано. Создайте новое подключение.", "Red-001");
                LoginPanel.ButtonConnectionName.Foreground = GetColors.Get("Red-001");
                LoginUserControl = CreateConnectionPanel;
            }
            else
            {
                LoginPanel.ButtonConnectionName.Foreground = GetColors.Get("Dark-003");
                ShowMessage("");
            }
        }

        #endregion --------------------------------------------------------------------------------------------------------




        #region == СОБЫТИЯ ================================================================================================

        // Событие: Клик кнопки "АВТОРИЗАЦИЯ" (окно: Login ) --------------------------------------------------------------
        private void ButtonAuthorization_Click(object sender, RoutedEventArgs e)
        {
            ShowMessage(string.Empty);
            Loader = LC;
            LC.WaitText = "Установка соединения...";

            LoginPanel.txbxUserName.IsEnabled = false;
            LoginPanel.psbxUserPassword.IsEnabled = false;

            LoginPanel.ButtonAuthorization.IsEnabled = false;
            LoginPanel.ButtonConnectionName.IsEnabled = false;

            new Thread(() => { UserAuthorization(); }).Start();
        }

        // Событие: Клик кнопки "Подключение: Название подключения" (окно: Login) -----------------------------------------
        private void ButtonConnectionName_Click(object sender, RoutedEventArgs e)
        {
            ConnectionListPanel.ButtonSelectedConnectionCheck.IsEnabled = false;
            ShowMessage("");
            ConnectionListPanel.stpConnectionList.Children.Clear();
            Loader = LC;
            LC.WaitText = "Загрузка списка подключений...";

            new Thread(() => { LoadAllConnections(); }).Start();

            //Thread thread = new Thread(new ThreadStart(LoadAllConnections));
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();

            LoginUserControl = ConnectionListPanel;
        }

        // Событие: Клик кнопки "X" и "Назад" (окно: Connection List) -----------------------------------------------------
        private void CloseConnectionList_Click(object sender, RoutedEventArgs e)
        {
            LoginUserControl = LoginPanel;
            LoginPanel.ButtonConnectionName.Content = XmlClass.GetSelectedConnectionName();
            if (LoginPanel.ButtonConnectionName.Content.ToString() == "")
            {
                ShowMessage("Подключение не создано. Создайте новое подключение.", "Red-001");
                LoginPanel.ButtonConnectionName.Foreground = GetColors.Get("Red-001");
                LoginPanel.ButtonAuthorization.IsEnabled = LoginTextChanged();
            }
            else
            {
                LoginPanel.ButtonConnectionName.Foreground = GetColors.Get("Dark-003");
                LoginPanel.ButtonAuthorization.IsEnabled = LoginTextChanged();
                ShowMessage("");
            }
        }

        // Событие: Клик кнопки "Проверить соединение" (окно: Connection List) --------------------------------------------
        private void ButtonSelectedConnectionCheck_Click(object sender, RoutedEventArgs e)
        {
            ShowMessage("");
            Loader = LC;
            LC.WaitText = "Установка соединения...";

            ConnectionListPanel.stpConnectionList.IsEnabled = false;
            ConnectionListPanel.ButtonCloseConnectionList.IsEnabled = false;
            ConnectionListPanel.ButtonBackToAuthorization.IsEnabled = false;
            ConnectionListPanel.ButtonSelectedConnectionCheck.IsEnabled = false;
            ConnectionListPanel.ButtonGoToCreateConnection.IsEnabled = false;

            //new Thread(() => { CheckSelectedConnection(); }) { IsBackground = true }.Start();
            new Thread(() => { CheckSelectedConnection(); }).Start();

        }

        // Событие: Клик кнопки "Создать подключение" (окно: Connection List) ---------------------------------------------
        private void GoToCreateConnection_Click(object sender, RoutedEventArgs e)
        {
            LoginUserControl = CreateConnectionPanel;
            CreateConnectionPanel.ConnectionCreateModeCheckBox.IsChecked = false;
            ClearTextBoxs(); // Вызов метода: Очистка TextBox-ов и PasswordBox-ов (окно: Create Connection)
            ShowMessage("Cоздайте новое подключение.", "Blue-004");
        }

        // Событие: Клик кнопки "Назад" (окно: Create Connection) ---------------------------------------------------------
        private void ButtonBackToConnectionList_Click(object sender, RoutedEventArgs e)
        {
            ConnectionListPanel.ButtonSelectedConnectionCheck.IsEnabled = false;
            ShowMessage("");
            ConnectionListPanel.stpConnectionList.Children.Clear();
            Loader = LC;
            LC.WaitText = "Загрузка списка подключений...";

            new Thread(() => { LoadAllConnections(); }).Start();

            //Thread thread = new Thread(new ThreadStart(LoadAllConnections));
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();

            LoginUserControl = ConnectionListPanel;
        }

        // Событие: Клик кнопки "X" (окно: Create Connection) -------------------------------------------------------------
        private void ButtonCloseCreatedConnectionPanel_Click(object sender, RoutedEventArgs e)
        {
            LoginUserControl = LoginPanel;
            LoginPanel.ButtonConnectionName.Content = XmlClass.GetSelectedConnectionName();
            if (LoginPanel.ButtonConnectionName.Content.ToString() == "")
            {
                ShowMessage("Подключение не создано. Создайте новое подключение.", "Red-001");
                LoginPanel.ButtonConnectionName.Foreground = GetColors.Get("Red-001");
                LoginPanel.ButtonAuthorization.IsEnabled = LoginTextChanged();
            }
            else
            {
                LoginPanel.ButtonConnectionName.Foreground = GetColors.Get("Dark-003");
                LoginPanel.ButtonAuthorization.IsEnabled = LoginTextChanged();
                ShowMessage("");
            }
        }

        // Событие: Клик кнопки "Вствить из буфера" (окно: Create Connection) ---------------------------------------------
        private void ButtonPasteConnString_Click(object sender, RoutedEventArgs e)
        {
            CreateConnectionPanel.txbxConnectionString.Clear();
            CreateConnectionPanel.txbxConnectionString.Text = Clipboard.GetText();
        }

        // Событие: Клик кнопки "Проверить соединение" (окно: Create Connection) ------------------------------------------
        private void ButtonCreatedConnectionCheck_Click(object sender, RoutedEventArgs e)
        {
            string conName = CreateConnectionPanel.txbxConnectionName.Text;
            ShowMessage("");
            Loader = LC;
            LC.WaitText = "Проверка соединения...";

            CreateConnectionPanel.ButtonCloseCreatedConnectionPanel.IsEnabled = false;
            CreateConnectionPanel.BorderConnValues.IsEnabled = false;
            CreateConnectionPanel.BorderButtons.IsEnabled = false;

            if (XmlClass.CheckConnectionName(conName) == false)
                new Thread(() => { CheckCreatedConnection(); }).Start();
            else
            {
                ShowMessage("Подключение с названием " + '"' + conName + '"' + " уже создано.\nИзмените название для подключения и попробуйте снова.", "Red-001");

                CreateConnectionPanel.ButtonCloseCreatedConnectionPanel.IsEnabled = true;
                CreateConnectionPanel.BorderConnValues.IsEnabled = true;
                CreateConnectionPanel.BorderButtons.IsEnabled = true;
                Loader = null;
            }
        }

        // Событие: Клик кнопки "Сохранить" (окно: Create Connection) -----------------------------------------------------
        private void ButtonSaveCreatedConnection_Click(object sender, RoutedEventArgs e)
        {
            ShowMessage("");
            Loader = LC;
            LC.WaitText = "Сохранение подключения...";
            new Thread(() => { SaveConnection(); }) { IsBackground = true }.Start();
        }

        // Событие: ЧекБокс перключение режима создания подключения: Create Connection) -----------------------------------
        private void ConnectionCreateModeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox ch = sender as CheckBox;
                if (ch.IsChecked == true)
                {
                    CreateConnectionPanel.grdCreateConnectionString.Visibility = Visibility.Visible;
                    CreateConnectionPanel.stpCreateConnection.Visibility = Visibility.Hidden;
                    CreateConnectionPanel.ButtonPasteConnString.IsEnabled = true;
                    CreateConnectionPanel.ButtonCreatedConnectionCheck.IsEnabled = CheckTexts();
                }
                else
                {
                    CreateConnectionPanel.grdCreateConnectionString.Visibility = Visibility.Hidden;
                    CreateConnectionPanel.stpCreateConnection.Visibility = Visibility.Visible;
                    CreateConnectionPanel.ButtonPasteConnString.IsEnabled = false;
                    CreateConnectionPanel.ButtonCreatedConnectionCheck.IsEnabled = CheckTexts();
                }
            }
        }

        // Событие: Изменение текста TextBox-ов (окно: Create Connection) -------------------------------------------------
        private void ConnTextChanged(object sender, TextChangedEventArgs e)
        {
            CreateConnectionPanel.ButtonCreatedConnectionCheck.IsEnabled = CheckTexts();
        }

        // Событие: Изменение пароля PasswordBox-а (окно: Create Connection) ----------------------------------------------
        private void ConnPasswordChanged(object sender, RoutedEventArgs e)
        {
            CreateConnectionPanel.ButtonCreatedConnectionCheck.IsEnabled = CheckTexts();
        }

        // Событие: Изменение пароля PasswordBox-а (окно: Login) ----------------------------------------------------------
        private void PsbxUserPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ShowMessage(string.Empty);
            LoginPanel.ButtonAuthorization.IsEnabled = LoginTextChanged();
        }

        // Событие: Изменение текста TextBox-а (окно: Login) --------------------------------------------------------------
        private void TxbxUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowMessage(string.Empty);
            LoginPanel.ButtonAuthorization.IsEnabled = LoginTextChanged();
        }

        // Событие: Нажатие Enter на PasswordBox (окно: Login ) -----------------------------------------------------------
        private void PsbxUserPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (LoginPanel.ButtonConnectionName.Content.ToString() != "")
                {

                    if (LoginPanel.txbxUserName.Text == "" || LoginPanel.txbxUserName.Text == string.Empty)
                        LoginPanel.txbxUserName.Focus();
                    else
                        ButtonAuthorization_Click(null, e);
                }
                else
                {
                    ShowMessage("Подключение не создано. Создайте новое подключение.", "Red-001");
                    LoginPanel.ButtonConnectionName.Foreground = GetColors.Get("Red-001");
                    LoginUserControl = CreateConnectionPanel;
                }
            }
        }

        // Событие: Нажатие Enter на TextBox (окно: Login ) ---------------------------------------------------------------
        private void TxbxUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (LoginPanel.ButtonConnectionName.Content.ToString() != "")
                {
                    if (LoginPanel.psbxUserPassword.Password == "" || LoginPanel.psbxUserPassword.Password == string.Empty)
                        LoginPanel.psbxUserPassword.Focus();
                    else
                        ButtonAuthorization_Click(null, e);
                }
                else
                {
                    ShowMessage("Подключение не создано. Создайте новое подключение.", "Red-001");
                    LoginPanel.ButtonConnectionName.Foreground = GetColors.Get("Red-001");
                    LoginUserControl = CreateConnectionPanel;
                }
            }
        }

        // Событие: Запрет Пробоел на TextBox (окно: Login ) --------------------------------------------------------------
        private void TxbxUserName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        // Событие: Запрет Пробоел на PasswordBox (окно: Login ) ----------------------------------------------------------
        private void PsbxUserPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        // Событие: Выбор подключения (окно: Connection List) -------------------------------------------------------------
        private void CRB_Checked(object sender, RoutedEventArgs e)
        {
            Loader = LC;
            LC.WaitText = "Выбор подключения";
            RadioButton RB = sender as RadioButton;
            new Thread(() => { WriteSelectedConnection(sender); }) { IsBackground = true}.Start();
            ShowMessage("Выбрано подключение: " + '"' + RB.Content.ToString() + '"', "Blue-004");
        }

        // Событие: Удаление подключения (окно: Connection List) ----------------------------------------------------------
        private void CRB_Deleted(object sender, EventArgs e)
        {
            Loader = LC;
            LC.WaitText = "Удаление подключения";
            new Thread(() => { DeleteConnection(sender); }) { IsBackground = true }.Start();
        }

        // Событие: Изменении свойства "IsEnabled" кнопки сохранения (окно: Create Connection) ----------------------------
        private void BtnSaveConnection_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Button button = sender as Button;
            if (button.IsEnabled == true)
                button.Foreground = GetColors.Get("Green-003");
            else
                button.Foreground = GetColors.Get("Dark-003");

        }

        // Событие: При загрузке "LoginPanel" (окно: Login ) --------------------------------------------------------------
        private void LoginPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoginPanel.txbxUserName.Text == "")
            {
                LoginPanel.txbxUserName.Focus();
                LoginPanel.txbxUserName.SelectionStart = LoginPanel.txbxUserName.Text.Length;
            }
            else
            {
                LoginPanel.psbxUserPassword.Focus();
                //LoginPanel.psbxUserPassword.se = LoginPanel.psbxUserPassword.Text.Length;
            }
            if (LoginPanel.ButtonConnectionName.Content.ToString() == "")
                LoginPanel.ButtonAuthorization.IsEnabled = false;
        }








        #endregion --------------------------------------------------------------------------------------------------------




        #region == МЕТОДЫ =================================================================================================

        // Метод: Присваивает события элементам ---------------------------------------------------------------------------
        private void SetEvents()
        {
            LoginPanel.ButtonAuthorization.Click += ButtonAuthorization_Click;
            LoginPanel.ButtonConnectionName.Click += ButtonConnectionName_Click;
            LoginPanel.txbxUserName.TextChanged += TxbxUserName_TextChanged;
            LoginPanel.txbxUserName.KeyDown += TxbxUserName_KeyDown;
            LoginPanel.txbxUserName.PreviewKeyDown += TxbxUserName_PreviewKeyDown;
            LoginPanel.psbxUserPassword.PasswordChanged += PsbxUserPassword_PasswordChanged;
            LoginPanel.psbxUserPassword.KeyDown += PsbxUserPassword_KeyDown;
            LoginPanel.psbxUserPassword.PreviewKeyDown += PsbxUserPassword_PreviewKeyDown;

            ConnectionListPanel.ButtonCloseConnectionList.Click += CloseConnectionList_Click;
            ConnectionListPanel.ButtonBackToAuthorization.Click += CloseConnectionList_Click;
            ConnectionListPanel.ButtonSelectedConnectionCheck.Click += ButtonSelectedConnectionCheck_Click;
            ConnectionListPanel.ButtonGoToCreateConnection.Click += GoToCreateConnection_Click;

            CreateConnectionPanel.ButtonBackToConnectionList.Click += ButtonBackToConnectionList_Click;
            CreateConnectionPanel.ButtonCloseCreatedConnectionPanel.Click += ButtonCloseCreatedConnectionPanel_Click;
            CreateConnectionPanel.ButtonCreatedConnectionCheck.Click += ButtonCreatedConnectionCheck_Click;
            CreateConnectionPanel.ButtonSaveCreatedConnection.Click += ButtonSaveCreatedConnection_Click;
            CreateConnectionPanel.ButtonSaveCreatedConnection.IsEnabledChanged += BtnSaveConnection_IsEnabledChanged;
            CreateConnectionPanel.ButtonPasteConnString.Click += ButtonPasteConnString_Click;
            CreateConnectionPanel.ConnectionCreateModeCheckBox.Checked += ConnectionCreateModeCheckBox_Checked;
            CreateConnectionPanel.ConnectionCreateModeCheckBox.Unchecked += ConnectionCreateModeCheckBox_Checked;
            CreateConnectionPanel.txbxConnectionName.TextChanged += ConnTextChanged;
            CreateConnectionPanel.txbxConnectionString.TextChanged += ConnTextChanged;
            CreateConnectionPanel.txbxDataBase.TextChanged += ConnTextChanged;
            CreateConnectionPanel.txbxServerName.TextChanged += ConnTextChanged;
            CreateConnectionPanel.txbxUserID.TextChanged += ConnTextChanged;
            CreateConnectionPanel.psbxUserPassword.PasswordChanged += ConnPasswordChanged;
        }

        // Метод: Сохранение создаваемого подключения ---------------------------------------------------------------------
        private void SaveConnection()
        {
            SqlConnectionStringBuilder ConnectionStringBuilder = new SqlConnectionStringBuilder() { Pooling = true };

            try
            {
                bool ok = false;

                Action aa = () => { ok = (bool)CreateConnectionPanel.ConnectionCreateModeCheckBox.IsChecked; };
                Dispatcher.Invoke(aa);

                if (ok)
                {
                    Action aC = () => { ConnectionStringBuilder.ConnectionString = CreateConnectionPanel.txbxConnectionString.Text; };
                    Dispatcher.Invoke(aC);
                }
                else
                {
                    Action aC = () =>
                    {
                        ConnectionStringBuilder.DataSource = CreateConnectionPanel.txbxServerName.Text;
                        ConnectionStringBuilder.InitialCatalog = CreateConnectionPanel.txbxDataBase.Text;
                        ConnectionStringBuilder.UserID = CreateConnectionPanel.txbxUserID.Text;
                        ConnectionStringBuilder.Password = CreateConnectionPanel.psbxUserPassword.Password;
                    };
                    Dispatcher.Invoke(aC);
                }
                Action action = () =>
                {
                    XmlClass.AddConnectSetting(CreateConnectionPanel.txbxConnectionName.Text, ConnectionStringBuilder.ConnectionString);
                    Loader = null;
                    CreateConnectionPanel.ButtonSaveCreatedConnection.IsEnabled = false;
                    ClearTextBoxs();
                    ShowMessage("Подключение успешно сохранено.", "Green-003");
                };
                Dispatcher.Invoke(action);


            }
            catch (Exception ex)
            {
                Action action = () =>
                {
                    Loader = null;
                    CreateConnectionPanel.ButtonSaveCreatedConnection.IsEnabled = false;
                    ShowMessage(ex.Message, "Red-001");
                };
                Dispatcher.Invoke(action);

            }
        }

        // Метод: Проверки создаваемого подключения -----------------------------------------------------------------------
        private void CheckCreatedConnection()
        {
            SqlConnectionStringBuilder ConnectionStringBuilder = new SqlConnectionStringBuilder() { Pooling = true };
            bool ok = false;
            Action aa = () =>
            {
                ok = (bool)CreateConnectionPanel.ConnectionCreateModeCheckBox.IsChecked;
            };
            Dispatcher.Invoke(aa);
            try
            {
                if (ok) // Если режим создания подключения "Строка подключения"
                {
                    Action aC = () =>
                    {
                        try
                        {
                            ConnectionStringBuilder.ConnectionString = CreateConnectionPanel.txbxConnectionString.Text;
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message, "Red-001");
                        }
                    };
                    Dispatcher.Invoke(aC);
                }
                else
                {
                    Action aC = () =>
                    {
                        ConnectionStringBuilder.DataSource = CreateConnectionPanel.txbxServerName.Text;
                        ConnectionStringBuilder.InitialCatalog = CreateConnectionPanel.txbxDataBase.Text;
                        ConnectionStringBuilder.UserID = CreateConnectionPanel.txbxUserID.Text;
                        ConnectionStringBuilder.Password = CreateConnectionPanel.psbxUserPassword.Password;
                    };
                    Dispatcher.Invoke(aC);
                }
                using (SqlConnection conn = new SqlConnection(ConnectionStringBuilder.ConnectionString))
                {
                    conn.Open();
                    conn.Close();
                }

                Action ac = () =>
                {
                    ShowMessage("Соединение установлено.", "Green-003");
                    CreateConnectionPanel.ButtonSaveCreatedConnection.IsEnabled = true;
                    CreateConnectionPanel.ButtonCloseCreatedConnectionPanel.IsEnabled = true;
                    CreateConnectionPanel.BorderConnValues.IsEnabled = true;
                    CreateConnectionPanel.BorderButtons.IsEnabled = true;

                    Loader = null;
                };
                Dispatcher.Invoke(ac);
            }
            catch (Exception ex)
            {
                Action ss = () => { ShowMessage(ex.Message, "Red-001"); }; Dispatcher.Invoke(ss);
            }
            Action action = () =>
            {
                CreateConnectionPanel.ButtonCloseCreatedConnectionPanel.IsEnabled = true;
                CreateConnectionPanel.BorderConnValues.IsEnabled = true;
                CreateConnectionPanel.BorderButtons.IsEnabled = true;
                Loader = null;
            };
            Dispatcher.Invoke(action);

        }

        // Метод: Проверки соединения выбранного подключения --------------------------------------------------------------
        private bool CheckSelectedConnection()
        {
            bool ok = false;

            using (SqlConnection conn = new SqlConnection(Crypt.Decrypt(XmlClass.GetSelectedConnectionString())))
            {
                try
                {
                    conn.Open();
                    conn.Close();
                    Action a = () => { ShowMessage("Соединение установлено.", "Green-003"); }; Dispatcher.Invoke(a);
                    ok = true;
                }
                catch (Exception ex)
                {
                    Action aa = () => { ShowMessage(ex.Message, "Red-001"); }; Dispatcher.Invoke(aa);
                    ok = false;
                }
            }
            Action action = () =>
            {
                ConnectionListPanel.stpConnectionList.IsEnabled = true;
                ConnectionListPanel.ButtonCloseConnectionList.IsEnabled = true;
                ConnectionListPanel.ButtonBackToAuthorization.IsEnabled = true;
                ConnectionListPanel.ButtonSelectedConnectionCheck.IsEnabled = true;
                ConnectionListPanel.ButtonGoToCreateConnection.IsEnabled = true;

                LoginPanel.ButtonConnectionName.IsEnabled = true;
                LoginPanel.txbxUserName.IsEnabled = true;
                LoginPanel.psbxUserPassword.IsEnabled = true;

                if (LoginTextChanged())
                    LoginPanel.ButtonAuthorization.IsEnabled = true;

                Loader = null;
            };
            Dispatcher.Invoke(action);
            return ok;
        }

        // Метод: Запись выбранного подключения в Xml-файл, файл конфигурации ---------------------------------------------
        void WriteSelectedConnection(object sender)
        {
            Action ac = () =>
            {
                XmlClass.SetSelectConnection(((ConnectionRB)sender).Content.ToString());
                LoginPanel.ButtonConnectionName.Content = ((ConnectionRB)sender).Content.ToString();
                Loader = null;
            };
            Dispatcher.Invoke(ac);
        }

        // Метод: Вывод сообщения (Перегрузка 1 - Без указания цвета текста) ----------------------------------------------
        private void ShowMessage(string Text)
        {
            txbxMessage.Text = Text;
            txbxMessage.Foreground = GetColors.Get("Red-001");
            //Loader = null;
        }

        // Метод: Вывод сообщения (Перегрузка 1 - Без указания цвета текста) ----------------------------------------------
        private void ShowMessage(string Text, string TextColorName)
        {
            txbxMessage.Text = Text;
            txbxMessage.Foreground = GetColors.Get(TextColorName);
            //Loader = null;
        }

        // Метод: Проверка заполнения TextBox-ов (окно: Create Connection) ------------------------------------------------
        bool CheckTexts()
        {
            bool ok = true;
            ShowMessage(string.Empty);
            if (CreateConnectionPanel.ConnectionCreateModeCheckBox.IsChecked == false)
            {
                if ((CreateConnectionPanel.txbxConnectionName.Text == "" || CreateConnectionPanel.txbxConnectionName.Text == string.Empty) || (CreateConnectionPanel.txbxServerName.Text == "" || CreateConnectionPanel.txbxServerName.Text == string.Empty) || (CreateConnectionPanel.txbxDataBase.Text == "" || CreateConnectionPanel.txbxDataBase.Text == string.Empty) || (CreateConnectionPanel.txbxUserID.Text == "" || CreateConnectionPanel.txbxUserID.Text == string.Empty) || (CreateConnectionPanel.psbxUserPassword.Password == "" || CreateConnectionPanel.psbxUserPassword.Password == string.Empty))
                {
                    ok = false;
                }
            }
            else
            {
                if ((CreateConnectionPanel.txbxConnectionName.Text == "" || CreateConnectionPanel.txbxConnectionName.Text == string.Empty) || (CreateConnectionPanel.txbxConnectionString.Text == "" || CreateConnectionPanel.txbxConnectionString.Text == string.Empty))
                {
                    ok = false;
                }
            }
            if (CreateConnectionPanel.ButtonSaveCreatedConnection.IsEnabled)
                CreateConnectionPanel.ButtonSaveCreatedConnection.IsEnabled = false;
            return ok;
        }

        // Метод: Очищает все TextBox-ы и PasswordBox (окно: Create Connection) -------------------------------------------
        private void ClearTextBoxs()
        {
            CreateConnectionPanel.txbxConnectionName.Clear(); // Очистка текста TextBox-а: "Название подключения"
            CreateConnectionPanel.txbxConnectionString.Clear(); // Очистка текста TextBox-а: "Строка подключения"
            CreateConnectionPanel.txbxDataBase.Clear(); // Очистка текста TextBox-а: "База данных"
            CreateConnectionPanel.txbxServerName.Clear(); // Очистка текста TextBox-а: "Сервер"
            CreateConnectionPanel.txbxUserID.Clear(); // Очистка текста TextBox-а: "Логин"
            CreateConnectionPanel.psbxUserPassword.Clear(); // Очистка пароля PasswordBox-а: "Пароль подключения"
        }

        // Метод: Проверка заполнения TextBox-а и PasswordBox-а (окно: Login) ---------------------------------------------
        bool LoginTextChanged()
        {
            bool ok = true;
            if ((LoginPanel.txbxUserName.Text == "" || LoginPanel.txbxUserName.Text == string.Empty) || (LoginPanel.psbxUserPassword.Password == "" || LoginPanel.psbxUserPassword.Password == string.Empty) || LoginPanel.ButtonConnectionName.Content.ToString() == "")
            {
                ok = false;
            }
            return ok;
        }

        // Метод: Загрузка списка подключений (окно: Connection List) -----------------------------------------------------
        private void LoadAllConnections()
        {

            string connName = XmlClass.GetSelectedConnectionName();

            if (connName == "")
            {
                Action ac = () => { ConnectionListPanel.ButtonSelectedConnectionCheck.IsEnabled = false; };
                Dispatcher.Invoke(ac);

            }
            else
            {
                Action ac = () => { ConnectionListPanel.ButtonSelectedConnectionCheck.IsEnabled = true; };
                Dispatcher.Invoke(ac);
            }

            Action action = () =>
            {

                ArrayList Connectionslist = XmlClass.ReadAllConnectionsName();
                for (int i = 0; i < Connectionslist.Count; i++)
                {
                    ConnectionRB CRB = new ConnectionRB();
                    CRB.FontFamily = new FontFamily("Roboto Condensed");
                    CRB.Foreground = new SolidColorBrush((Color)Application.Current.FindResource("Dark-003"));
                    CRB.Deleted += CRB_Deleted; ;
                    CRB.Checked += CRB_Checked; ;
                    CRB.Content = Connectionslist[i].ToString();
                    ConnectionListPanel.stpConnectionList.Children.Add(CRB);
                    if (CRB.Content.ToString() == connName)
                        CRB.IsChecked = true;
                }

                Loader = null;
                if (ConnectionListPanel.stpConnectionList.Children.Count > 0)
                    ShowMessage("Выберите подключение или создайте новое подключение.", "Blue-004");
                else
                    ShowMessage("Cоздайте новое подключение.", "Red-001");

            }; Dispatcher.Invoke(action);
        }

        // Метод: Удаление подключения (окно: Connection List) ------------------------------------------------------------
        private void DeleteConnection(object sender)
        {
            Action action = () =>
            {
                ConnectionRB CRB = sender as ConnectionRB;
                bool isCheck = (bool)CRB.IsChecked;
                StackPanel sp = CRB.Parent as StackPanel;
                sp.Children.Remove(CRB);
                XmlClass.DeleteConnection(CRB.Content.ToString());
                if (isCheck && sp.Children.Count >= 1)// если удален выбранный и есть еще
                {
                    XmlClass.DeSelectAllConnections();
                    ConnectionRB crb = sp.Children[sp.Children.Count - 1] as ConnectionRB;
                    crb.IsChecked = true;
                }
                Loader = null;
            };
            Dispatcher.Invoke(action);
        }

        // Метод: Возвращает строку удалив пробелы и другие символы -------------------------------------------------------
        private string RemoveSpaces(string InputText)
        {
            string temp = "";
            foreach (char ch in InputText)
            {
                if (ch == ' ' || ch == ',' || ch == '.' || ch == '-' || ch == '_' || ch == '=' || ch == '+' || ch == '|' || ch == '"' || ch == '`' || ch == ')' || ch == '(' || ch == '{' || ch == '}' || ch == '[' || ch == ']' || ch == ';' || ch == ':' || ch == '@' || ch == '#' || ch == '$' || ch == '%' || ch == '&' || ch == '*' || ch == '/' || ch == '>' || ch == '<' || ch == '!' || ch == '?')
                    continue;
                temp += ch;
            }
            return temp;
        }

        // Метод: Сформирирует и возвращает название для интерфейса пользователя ------------------------------------------
        private string GetUserControlName()
        {
            return LoginPanel.txbxUserName.Text + RemoveSpaces(XmlClass.GetSelectedConnectionName());
        }

        // Метод: Проверяет не открыто-ли уже интерфейс пользователя ------------------------------------------------------
        private bool CheckUserInterface()
        {
            bool ok = false;
            foreach (UserControl UC in WorkerGrid.Children)
            {
                if (UC.Name == GetUserControlName())
                {
                    ok = true;
                    break;
                }
            }
            return ok;
        }

        // Метод: Возвращает интерйфейс для пользователя ------------------------------------------------------------------
        private UserControl GetUserInterface(string UserInterfaceName)
        {
            switch (UserInterfaceName)
            {
                case "Администратор": return new Administrator(this, XmlClass.GetSelectedConnectionString()) { Name = GetUserControlName() };
            }
            return new SelectUserInterface(LoginCC, WorkerGrid, XmlClass.GetSelectedConnectionString(), GetUserControlName());
        }

        // Метод: Авторизация пользователья -------------------------------------------------------------------------------
        void UserAuthorization()
        {
            string message = "";
            if (CheckSelectedConnection())
            {
                Action ac = () =>
                {
                    if (DataBaseRequest.CheckAuthorization(LoginPanel.txbxUserName.Text, LoginPanel.psbxUserPassword.Password, ref message))
                    {
                        if (!CheckUserInterface())
                        {
                            XmlClass.WriteCurrentUserName(LoginPanel.txbxUserName.Text.ToString());
                            WorkerGrid.Children.Add(GetUserInterface(DataBaseRequest.GetUserInterfaceName(LoginPanel.txbxUserName.Text.ToString())));
                        }
                        else
                        {
                            foreach (UserControl UC in WorkerGrid.Children)
                            {
                                if (UC.Name == GetUserControlName())
                                    UC.Visibility = Visibility.Visible;
                                else
                                    UC.Visibility = Visibility.Hidden;
                            }
                        }

                        Visibility = Visibility.Hidden;
                        LoginPanel.psbxUserPassword.Clear();
                        ShowMessage("");
                    }
                    else
                    {
                        LoginPanel.psbxUserPassword.SelectAll();
                        
                        LoginPanel.psbxUserPassword.Focus();
                        ShowMessage(message, "Red-001");
                    }
                    Loader = null;
                    LoginPanel.ButtonConnectionName.IsEnabled = true;
                };
                Dispatcher.Invoke(ac);
            }
        }



        #endregion --------------------------------------------------------------------------------------------------------




        #region == СВОЙСТВА ===============================================================================================


        // Свойство: "LoginControl" ---------------------------------------------------------------------------------------
        public UserControl LoginUserControl
        {
            get { return (UserControl)GetValue(LoginUserControlProperty); }
            set { SetValue(LoginUserControlProperty, value); }
        }
        public static readonly DependencyProperty LoginUserControlProperty = DependencyProperty.Register("LoginUserControl", typeof(UserControl), typeof(LoginControl), new PropertyMetadata(null));




        public UserControl Loader
        {
            get { return (UserControl)GetValue(LoaderProperty); }
            set { SetValue(LoaderProperty, value); }
        }

        public static readonly DependencyProperty LoaderProperty = DependencyProperty.Register("Loader", typeof(UserControl), typeof(LoginControl), new PropertyMetadata(null));




        #endregion --------------------------------------------------------------------------------------------------------








    }
}
