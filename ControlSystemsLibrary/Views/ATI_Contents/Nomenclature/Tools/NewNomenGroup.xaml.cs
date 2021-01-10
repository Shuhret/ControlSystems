using ControlSystemsLibrary.Classes;
using ControlSystemsLibrary.Views.ATI_Contents;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ControlSystemsLibrary.Views.Tools
{



    public partial class NewNomenGroup : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged =======================================================================================================

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        private delegate void PrivateDelegate();

        #region Конструкторы =================================================================================================================

        // Конструктор (1-Перегрузка: Для создания -------------------------------------------------------------------------------------------
        public NewNomenGroup(ref Guid GroupID, AfterItemCreationDelegate AfterCreationDelegate)
        {
            this.GroupID = GroupID;
            this.AfterCreationDelegate = AfterCreationDelegate;
            CreateMode = true;
            
            InitializeComponent();
            ButtonCreateEnabled = false;
        }

        // Конструктор (2-Перегрузка: Для изменения ------------------------------------------------------------------------------------------
        public NewNomenGroup(ref Guid GroupID, AfterItemCreationDelegate AfterCreationDelegate, NomenclatureClass EditableGroup)
        {
            this.GroupID = GroupID;
            this.AfterCreationDelegate = AfterCreationDelegate;
            this.EditableGroup = EditableGroup;
            CreateMode = false;
            InitializeComponent();
            ButtonCreateEnabled = true;
        }

        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


        #region Поля и свойства ==============================================================================================================

        // Объект Делегата
        AfterItemCreationDelegate AfterCreationDelegate;

        // ID открытой Группы 
        Guid GroupID;

        // Изменяемая группа
        NomenclatureClass EditableGroup;

        // Режим Создание/Изменение ----------------------------------------------------------------------------------------------------------
        private bool createMode = false;
        public bool CreateMode
        {
            get => createMode;
            set
            {
                createMode = value;
                if (value == true)
                {
                    HeaderText = "Создание новой группы";
                    ButtonText = "Создать";
                }
                else
                {
                    HeaderText = "Изменение группы";
                    ButtonText = "Изменить";
                }
                OnPropertyChanged();
            }
        }

        // Текст кнопки Создать/Изменить -----------------------------------------------------------------------------------------------------
        private string buttonText;
        public string ButtonText
        {
            get => buttonText;
            set
            {
                buttonText = value;
                OnPropertyChanged();
            }
        }


        // Текст для зоголовка окна в зависимости от режима ----------------------------------------------------------------------------------
        private string headerText;
        public string HeaderText
        {
            get => headerText;
            set
            {
                headerText = value;
                OnPropertyChanged();
            }
        }

        // Enabled кнопки "Создать" ----------------------------------------------------------------------------------------------------------
        private bool buttonCreateEnabled;
        public bool ButtonCreateEnabled
        {
            get => buttonCreateEnabled;
            set
            {
                buttonCreateEnabled = value;
                OnPropertyChanged();
            }
        }



        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


        #region События ======================================================================================================================

        // Событие: Click для кнопки создать -------------------------------------------------------------------------------------------------
        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if(CreateMode == true) // Если режим "Создание"
            {
                CreateNewGroup();
            }
            else // Если режим "Изменение"
            {
                EditGroup();
            }
            
        }

        // Событие: Loaded для окна создания новой группы ------------------------------------------------------------------------------------
        private void CreateNomenGroupUCLoaded(object sender, RoutedEventArgs e)
        {
            if (CreateMode == true)
                NomenGroupNameTextBox.Clear();
            else
            {
                NomenGroupNameTextBox.Text = EditableGroup.Name;

                NomenGroupNameTextBox.SelectionStart = 0;
                NomenGroupNameTextBox.SelectionLength = NomenGroupNameTextBox.Text.Length;
            }
            NomenGroupNameTextBox.Focus();
        }

        // Событие: TextCanged для TextBox-а -------------------------------------------------------------------------------------------------
        private void GroupNameTextCanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text != "" && (sender as TextBox).Text != null && (sender as TextBox).Text != string.Empty)
                ButtonCreateEnabled = true;
            else
                ButtonCreateEnabled = false;
        }

        // Событие: KeyDown для TextBox-а ----------------------------------------------------------------------------------------------------
        private void NomenGroupNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) // Если нажатая клавиша = Enter
            {
                if (NomenGroupNameTextBox.Text != "")
                    ButtonCreate_Click(this, null);
            }
        }


        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


        #region Методы =======================================================================================================================
        
        // Метод: Создает новую группу -------------------------------------------------------------------------------------------------------
        private void CreateNewGroup()
        {
            NomenclatureClass NC = new NomenclatureClass();
            NC.ID = Guid.NewGuid();
            NC.GroupID = GroupID;
            NC.Name = NomenGroupNameTextBox.Text;
            NC.GroupNomen = false;

            if (DataBaseRequest.CreateNomenclatureGroup(NC))
            {
                NomenGroupNameTextBox.Clear();
                AfterCreationDelegate(NC);
                NomenGroupNameTextBox.Focus();
            }
        }

        // Метод: Изменяет группу ------------------------------------------------------------------------------------------------------------
        private void EditGroup()
        {
            NomenclatureClass NC = new NomenclatureClass();
            NC.ID = EditableGroup.ID;
            NC.GroupID = EditableGroup.GroupID;
            NC.Name = NomenGroupNameTextBox.Text;
            NC.GroupNomen = false;

            if (DataBaseRequest.UpdateNomenclatureGroup(NC))
            {
                NomenGroupNameTextBox.Clear();
                AfterCreationDelegate(NC);
                NomenGroupNameTextBox.Focus();
            }
        }

        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
    }
}
