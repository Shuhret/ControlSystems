using ControlSystemsLibrary.Classes;
using ControlSystemsLibrary.Views.ATI_Contents.Nomenclature.Tools;
using ControlSystemsLibrary.Views.Tools;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace ControlSystemsLibrary.Views.ATI_Contents
{

    public delegate void AfterItemCreationDelegate(NomenclatureClass LastCreatedNomenclature);


    public partial class ATI_Nomenclature : UserControl, INotifyPropertyChanged
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


        // Конструктор ***********************************************************************************************************************
        public ATI_Nomenclature()
        {
            InitializeComponent();
            AllSelectedElementsList.ListChanged += AllSelectedElementsList_ListChanged;
        }




        #region Поля и свойства ==============================================================================================================
        private bool viewTableMode = true;
        public bool ViewTableMode
        {
            get => viewTableMode;
            set
            {
                viewTableMode = value;
                OnPropertyChanged();
            }
        }

        bool update = true;


        // Окно создания новой номенклатуры --------------------------------------------------------------------------------------------------
        private CreateNomenclature createNomenclature;
        // Окно создания новой группы номенклатуры -------------------------------------------------------------------------------------------
        private NewNomenGroup newNomenGroup;



        // Список всей номенклатуры из базы данных -------------------------------------------------------------------------------------------
        public BindingList<NomenclatureClass> AllNomenclaturesList = new BindingList<NomenclatureClass>();

        // Список номенклатуры для вывода в DataGrid -----------------------------------------------------------------------------------------
        public BindingList<NomenclatureClass> ShowNomenclatureList = new BindingList<NomenclatureClass>();

        // ID текущей группы -----------------------------------------------------------------------------------------------------------------
        public static Guid CurrentGroupID = new Guid("00000000-0000-0000-0000-000000000000");

        // ID элемента -----------------------------------------------------------------------------------------------------------------------
        public static Guid ID;

        // ID редактиуемого элемента ---------------------------------------------------------------------------------------------------------
        private Guid editableElementID;
        public Guid EditableElementID
        {
            get => editableElementID;
            set
            {
                editableElementID = value;
                OnPropertyChanged();
            }
        }

        // Количество выбранных Номенклатур --------------------------------------------------------------------------------------------------
        private int selectedNomenclaturesCount;
        public int SelectedNomenclaturesCount
        {
            get => selectedNomenclaturesCount;
            set
            {
                selectedNomenclaturesCount = value;
                if (value > 0)
                    ListOfSelectedButtonVisibility = Visibility.Visible;
                else
                    ListOfSelectedButtonVisibility = Visibility.Collapsed;
                OnPropertyChanged();
            }
        }

        // Количество вырезанных элементов ---------------------------------------------------------------------------------------------------
        private int countOfCutElements = 0;
        public int CountOfCutElements
        {
            get => countOfCutElements;
            set
            {
                countOfCutElements = value;
                if (value > 0)
                    PasteButtonVisibility = Visibility.Visible;
                else
                    PasteButtonVisibility = Visibility.Collapsed;
                OnPropertyChanged();
            }
        }

        // Видимость кнопки "Выбранные" ------------------------------------------------------------------------------------------------------
        private Visibility listOfSelectedButtonVisibility = Visibility.Collapsed;
        public Visibility ListOfSelectedButtonVisibility
        {
            get => listOfSelectedButtonVisibility;
            set
            {
                listOfSelectedButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        // Видимость кнопки "Вставить" -------------------------------------------------------------------------------------------------------
        private Visibility pasteButtonVisibility = Visibility.Collapsed;
        public Visibility PasteButtonVisibility
        {
            get => pasteButtonVisibility;
            set
            {
                pasteButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        // Поле: Enabled для кноки "Назад" ---------------------------------------------------------------------------------------------------
        private bool buttonBackEnabled = false;
        public bool ButtonBackEnabled
        {
            get => buttonBackEnabled;
            set
            {
                buttonBackEnabled = value;
                OnPropertyChanged();
            }
        }




        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


        #region События ======================================================================================================================


        // Событие: Loaded ATI_Nomenclature --------------------------------------------------------------------------------------------------
        private void Nom_Loaded(object sender, RoutedEventArgs e)
        {
            if (update == true)
            {
                LoadAllNomenclatures();
                TableRadioButton.IsChecked = true;
                FillingNomenclaturesList();
                update = false;
            }
        }

        // Событие: Unloaded ATI_Nomenclature ------------------------------------------------------------------------------------------------
        private void Nom_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }




        // Событие: Click кнопки "Создать новую номенклатуру" --------------------------------------------------------------------------------
        private void ButtonCreateNomenclature_Click(object sender, RoutedEventArgs e)
        {
            OpenNewNomenclatureCreater();
        }

        // Событие: Click "Закрыть" окно создания новой номенклатуры -------------------------------------------------------------------------
        private void CreateNomenclatureButtonClose_Click(object sender, RoutedEventArgs e)
        {
            CloseNewNomenclatureCreater();
        }

        // Событие: Click кнопки "Создать новую группу" --------------------------------------------------------------------------------------
        private void ButtonCreateNomenGroup_Click(object sender, RoutedEventArgs e)
        {
            OpenNewNomenGroupCreater();
        }

        // Событие: Click на кнопку "Закрыть" окно создания новой группы ---------------------------------------------------------------------
        private void NewGroupButtonClose_Click(object sender, RoutedEventArgs e)
        {
            CloseNewGroupCreater();
        }

        // Событие: Click для кнопки "Изменить" ----------------------------------------------------------------------------------------------
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridNomenclatures.SelectedItem != null)
            {
                NomenclatureClass NC = DataGridNomenclatures.SelectedItem as NomenclatureClass;
                EditableElementID = NC.ID;
                int index = DataGridNomenclatures.SelectedIndex;
                DataGridNomenclatures.SelectedItem = null;
                DataGridNomenclatures.SelectedIndex = index;

                if (DataGridNomenclatures.SelectedItem != null)
                {
                    if (NC.GroupNomen == true)
                    {
                        OpenNewNomenclatureEditor(NC);
                    }
                    else
                    {
                        OpenNomenGroupEditor(NC);
                    }
                }
            }
        }

        // Событие: Click кнопки "Выбранные" -------------------------------------------------------------------------------------------------
        private void ButtonSelectedNomenclatures_Click(object sender, RoutedEventArgs e)
        {
            OpenSelectedNomenclaturesList();
        }

        // Событие: Click для кнопки "Закрыть" Окна выбранных --------------------------------------------------------------------------------
        private void SelectedNomenclaturesButtonClose_Click(object sender, RoutedEventArgs e)
        {
            ToolsGrid.Visibility = Visibility.Collapsed;
            ToolsContentControl.Content = null;
            DataGridNomenclatures.Focus();
        }

        // Событие: Click для кнопки "Вырезать" ----------------------------------------------------------------------------------------------
        private void ButtonCut_Click(object sender, RoutedEventArgs e)
        {
            CutElements();
        }

        // Событие: Click для кнопки "Лист вырезанных" ---------------------------------------------------------------------------------------
        private void ButtonCutList_Click(object sender, RoutedEventArgs e)
        {

        }

        // Событие: Click для кнопки "Вставить" ----------------------------------------------------------------------------------------------
        private void ButtonPaste_Click(object sender, RoutedEventArgs e)
        {
            PasteElements();
        }

        // Событие: Click для кнопки "Назад" -------------------------------------------------------------------------------------------------
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            FillingNomenclaturesListBack();
        }

        // Событие: Клик на Теги (Акция, Фокус и Новинка) ------------------------------------------------------------------------------------
        private void UpdateTagsClick(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox ch = sender as CheckBox;
                NomenclatureClass item = ch.DataContext as NomenclatureClass;
                switch (ch.Name)
                {
                    case "TagAksia":
                        DataBaseRequest.UpdateTagAksia(item); break;
                    case "TagFocus":
                        DataBaseRequest.UpdateTagFocus(item); break;
                    case "TagNew":
                        DataBaseRequest.UpdateTagNew(item); break;
                }
            }
        }









        // Событие: DoubleClick на DataGridRow -----------------------------------------------------------------------------------------------
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            NomenclatureClass NC = row.Item as NomenclatureClass;

            if (NC.GroupNomen == false)
            {
                CurrentGroupID = NC.ID;
                FillingNomenclaturesList();
            }
            else
            {
                MessageBox.Show("ID: " + NC.ID.ToString());
            }
        }

        // Событие: PreviewKeyDown для DataGridRow -------------------------------------------------------------------------------------------
        private void DataGridRow_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            // Если нажато: Ctrl+Down
            if (e.Key == Key.Down && e.KeyboardDevice.Modifiers == ModifierKeys.Control) // Control + Up
            {
                NomenclatureClass NC = (sender as DataGridRow).DataContext as NomenclatureClass;
                if (NC.GroupNomen == false)
                {
                    CurrentGroupID = NC.ID;
                    FillingNomenclaturesList();
                }
                e.Handled = true;
            }
            // Если нажато: Enter
            if (e.Key == Key.Enter)
            {
                NomenclatureClass NC = (sender as DataGridRow).DataContext as NomenclatureClass;
                if (NC.GroupNomen == false)
                {
                    CurrentGroupID = NC.ID;
                    FillingNomenclaturesList();
                }
                e.Handled = true;
            }
            // Если нажато: Пробел
            if (e.Key == Key.Space)
            {
                NomenclatureClass NC = (sender as DataGridRow).DataContext as NomenclatureClass;
                if (NC.Selected == null || NC.Selected == true)
                    NC.Selected = false;
                else if (NC.Selected == false)
                    NC.Selected = true;
                e.Handled = true;
            }
            // Если нажато: Delete
            if (e.Key == Key.Delete)
            {
                e.Handled = true;
            }
            
        }

        // Событие: PreviewKeyDown для DataGrid номенклатуры ---------------------------------------------------------------------------------
        private void DataGridNomenclatures_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Если нажато: Control+Up
            if (e.Key == Key.Up && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                DataGrid DG = sender as DataGrid;
                NomenclatureClass NC = DG.SelectedItem as NomenclatureClass;
                if (CurrentGroupID != new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    FillingNomenclaturesListBack();
                }
                e.Handled = true;
            }
            if (e.Key == Key.Back)
            {
                DataGrid DG = sender as DataGrid;
                NomenclatureClass NC = DG.SelectedItem as NomenclatureClass;
                if (CurrentGroupID != new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    FillingNomenclaturesListBack();
                }
                e.Handled = true;
            }
            if (e.Key == Key.Home)
            {
                DataGrid DG = sender as DataGrid;
                DG.SelectedIndex = 0;
                if (DG.SelectedItems.Count > 0)
                    DG.CurrentCell = new DataGridCellInfo(DG.Items[0], DG.Columns[3]);
                e.Handled = true;
            }
            if (e.Key == Key.End)
            {
                DataGrid DG = sender as DataGrid;
                DG.SelectedIndex = DG.Items.Count - 1;

                if (DG.SelectedItems.Count > 0)
                    DG.CurrentCell = new DataGridCellInfo(DG.Items[DG.Items.Count - 1], DG.Columns[3]);
                e.Handled = true;
            }
            if (e.Key == Key.F2)
            {
                ButtonEdit_Click(null, null);
                e.Handled = true;
            }
            if (e.Key == Key.Space)
            {
                //NomenclatureClass NC = (sender as DataGridRow).DataContext as NomenclatureClass;
                foreach (NomenclatureClass DGR in DataGridNomenclatures.SelectedItems)
                {
                    if (DGR.Selected == null || DGR.Selected == false)
                        DGR.Selected = true;
                    else if (DGR.Selected == true)
                    {
                        DGR.Selected = false;
                    }
                }
                e.Handled = true;
            }
            if (e.Key == Key.X && e.KeyboardDevice.Modifiers == ModifierKeys.Control) // Control + X = Вырезать
            {
                CutElements(); // Вырезать
                e.Handled = true;
            }
            if (e.Key == Key.V && e.KeyboardDevice.Modifiers == ModifierKeys.Control) // Control + V = Вставить
            {
                PasteElements(); // Вставить
                e.Handled = true;
            }
        }

        // Событие: KeyDown для окна "Создать новую номенклатуру" ----------------------------------------------------------------------------
        private void CreateNomenclature_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                //CloseNewNomenclatureCreater();
            }
        }

        // Событие: KeyDown для окна создания новой группы -----------------------------------------------------------------------------------
        private void NewNomenGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                (sender as NewNomenGroup).NomenGroupNameTextBox.Clear();
                //CloseNewGroupCreater();
            }
        }

        // Событие: KeyDown для ATI_Nomenclature ---------------------------------------------------------------------------------------------
        private void Nom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.G)
            {
                if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    //OpenNewNomenGroupCreater();
                }
            }
            if (e.Key == Key.N)
            {
                if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    //OpenNewNomenclatureCreater();
                }
            }
            if (e.Key == Key.Escape)
            {
                ToolsGrid.Visibility = Visibility.Collapsed;
                ToolsContentControl.Content = null;
                //SelectLastCreated(EditableElementID);
                e.Handled = true;
            }
        }

        // Событие: Loaded для DataGrid ------------------------------------------------------------------------------------------------------
        private void DataGridNomenclatures_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is DataGrid)
            {
                DataGrid DG = sender as DataGrid;
                DG.Focus();
                if (DG.Items.Count > 0)
                {
                    DG.CurrentCell = new DataGridCellInfo(DG.Items[0], DG.Columns[3]);
                    DG.ScrollIntoView(DG.SelectedItem);
                }
            }
        }


        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


        #region Методы =======================================================================================================================

        // Метод: Загрузка всей номенклатуры и групп из базы данных --------------------------------------------------------------------------
        private void LoadAllNomenclatures()
        {
            Action action = () =>
            {
                AllNomenclaturesList = DataBaseRequest.GetAllNomenclatures();
            };
            Dispatcher.Invoke(action);
        }

        // Метод: Заполняет BindingList "NomenclaturesList" из базы данных по GroupID --------------------------------------------------------
        private void FillingNomenclaturesList()
        {
            ShowNomenclatureList = DataBaseRequest.GetAllMainNomenclatures(CurrentGroupID);



            if (CurrentGroupID == new Guid("00000000-0000-0000-0000-000000000000"))
                ButtonBackEnabled = false;
            else
                ButtonBackEnabled = true;



            if (ViewTableMode == true)
            {
                DataGridNomenclatures.ItemsSource = ShowNomenclatureList;
                DataGridSelectionUpdate();

                DataGridNomenclatures.SelectedIndex = 0;
                if (DataGridNomenclatures.SelectedItems.Count > 0)
                {
                    DataGridNomenclatures.CurrentCell = new DataGridCellInfo(DataGridNomenclatures.Items[0], DataGridNomenclatures.Columns[3]);
                    DataGridNomenclatures.Focus();
                }
                else
                {
                    DataGridNomenclatures.Focus();
                }
            }
            else
            {
                RootGrid.DataContext = ShowNomenclatureList;
            }
        }




        // Метод: Заполняет BindingList "NomenclaturesList" из базы данных по GroupID (Назад) ------------------------------------------------
        private void FillingNomenclaturesListBack()
        {
            ShowNomenclatureList.Clear();
            ShowNomenclatureList = DataBaseRequest.GetAllNomenclaturesBack(ref CurrentGroupID, ref ID);
            DataGridNomenclatures.ItemsSource = ShowNomenclatureList;
            DataGridSelectionUpdate();

            if (CurrentGroupID == new Guid("00000000-0000-0000-0000-000000000000"))
                ButtonBackEnabled = false;
            else
                ButtonBackEnabled = true;

            int index = 0;
            foreach (NomenclatureClass NC in DataGridNomenclatures.Items)
            {
                if (NC.ID == ID)
                {
                    DataGridNomenclatures.SelectedIndex = index;
                    if (DataGridNomenclatures.SelectedItems.Count > 0)
                    {
                        DataGridNomenclatures.CurrentCell = new DataGridCellInfo(DataGridNomenclatures.Items[index], DataGridNomenclatures.Columns[3]);
                        DataGridNomenclatures.Focus();
                    }
                    break;
                }
                index++;
            }
        }






        // Метод: Открывает окно "Создать новую номенклатуру" (Для создания) -----------------------------------------------------------------
        private void OpenNewNomenclatureCreater()
        {
            ToolsGrid.Visibility = Visibility.Visible;
            AfterItemCreationDelegate AfterCreationDelegate = AfterItemCreationMethod;
            ToolsContentControl.Content = createNomenclature = new CreateNomenclature(true, AfterCreationDelegate, CurrentGroupID);
            createNomenclature.ButtonClose.Click += CreateNomenclatureButtonClose_Click;
            createNomenclature.KeyDown += CreateNomenclature_KeyDown;
        }

        // Метод: Открывает окно "Создать новую номенклатуру" (Для изменения) ----------------------------------------------------------------
        private void OpenNewNomenclatureEditor(NomenclatureClass EditableNomenclature)
        {
            ToolsGrid.Visibility = Visibility.Visible;
            AfterItemCreationDelegate AfterCreationDelegate = AfterItemCreationMethod;
            ToolsContentControl.Content = createNomenclature = new CreateNomenclature(false, AfterCreationDelegate, EditableNomenclature);
            createNomenclature.ButtonClose.Click += CreateNomenclatureButtonClose_Click;
            createNomenclature.KeyDown += CreateNomenclature_KeyDown;
        }

        // Метод: Открывает окно "Создать новую группу" (для изменения) ----------------------------------------------------------------------
        private void OpenNomenGroupEditor(NomenclatureClass EditableGroup)
        {
            ToolsGrid.Visibility = Visibility.Visible;
            AfterItemCreationDelegate AfterCreationDelegate = AfterItemCreationMethod;
            ToolsContentControl.Content = newNomenGroup = new NewNomenGroup(ref CurrentGroupID, AfterCreationDelegate, EditableGroup);
            newNomenGroup.ButtonClose.Click += NewGroupButtonClose_Click;
            newNomenGroup.KeyDown += NewNomenGroup_KeyDown;
        }

        // Метод: Который выполянется после создания/изменения элемента (передается делегатом) <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        private void AfterItemCreationMethod(NomenclatureClass LastCreatedNomenclature)
        {
            AllNomenclaturesList.Add(LastCreatedNomenclature);
            AddNomenclatureToNomenclaturesList(LastCreatedNomenclature);
            FillingNomenclaturesList();
            SelectLastCreated(LastCreatedNomenclature);
        }

        // Метод: Проверяет "NomenclaturesList" и добавляет элемент в список -----------------------------------------------------------------
        private void AddNomenclatureToNomenclaturesList(NomenclatureClass LastCreatedNomenclature)
        {
            bool ok = true;
            foreach (NomenclatureClass NCList in ShowNomenclatureList)
            {
                if (NCList.ID == LastCreatedNomenclature.ID)
                {
                    ShowNomenclatureList.Remove(NCList);
                    ok = false;
                    break;
                }
            }
            if (ok == true)
            {
                ShowNomenclatureList.Add(LastCreatedNomenclature);
            }
        }

        // Метод: Выберает созданного/измененного последним элемента (1-перегрузка)-----------------------------------------------------------
        private void SelectLastCreated(NomenclatureClass CreatedNomenclature)
        {
            int index = 0;
            foreach (NomenclatureClass NC in DataGridNomenclatures.Items)
            {
                if (NC.ID == CreatedNomenclature.ID)
                {
                    DataGridNomenclatures.SelectedIndex = index;

                    if (DataGridNomenclatures.SelectedItems.Count > 0)
                    {
                        DataGridNomenclatures.Focus();
                        DataGridNomenclatures.CurrentCell = new DataGridCellInfo(DataGridNomenclatures.Items[index], DataGridNomenclatures.Columns[3]);
                        DataGridNomenclatures.ScrollIntoView(DataGridNomenclatures.SelectedItem);
                    }
                    break;
                }
                index++;
            }
        }

        // Метод: После закрытия окна "Создать новую номенклатуру" ---------------------------------------------------------------------------
        private void CloseNewNomenclatureCreater()
        {
            ToolsGrid.Visibility = Visibility.Collapsed;
            ToolsContentControl.Content = null;
            DataGridNomenclatures.Focus();
        }

        // Метод: Открывает окно "Создать новую группу" (для создания) -----------------------------------------------------------------------
        private void OpenNewNomenGroupCreater()
        {
            ToolsGrid.Visibility = Visibility.Visible;
            AfterItemCreationDelegate AfterCreationDelegate = AfterItemCreationMethod;
            ToolsContentControl.Content = newNomenGroup = new NewNomenGroup(ref CurrentGroupID, AfterCreationDelegate);
            newNomenGroup.ButtonClose.Click += NewGroupButtonClose_Click;
            newNomenGroup.KeyDown += NewNomenGroup_KeyDown;
        }

        // Метод: После закрытия окна "Создать новую группу" ---------------------------------------------------------------------------------
        private void CloseNewGroupCreater()
        {
            ToolsGrid.Visibility = Visibility.Collapsed;
            ToolsContentControl.Content = null;
            DataGridNomenclatures.Focus();
        }

        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::







        #region SELECT =======================================================================================================================
        // Список для выбранных элементов ----------------------------------------------------------------------------------------------------
        private BindingList<NomenclatureClass> AllSelectedElementsList = new BindingList<NomenclatureClass>();

        // Событие: При изменении списка выбранных элементов ---------------------------------------------------------------------------------
        private void AllSelectedElementsList_ListChanged(object sender, ListChangedEventArgs e)
        {
            SetSelectedCount();
        }

        // Переменная для изменения списка выбранных -----------------------------------------------------------------------------------------
        private bool UpdateSelected = true;

        // Событие: Check и Uncheck на CheckBox Выбора номенклатуры --------------------------------------------------------------------------
        private void Select(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;

            if (checkbox.Name == "MainSelectorCheckBox")
            {
                if (UpdateSelected == true)
                {
                    SelcectAllItemsDataGrid(checkbox);
                }
            }
            else
            {
                if (UpdateSelected == true)
                {
                    NomenclatureClass NC = checkbox.DataContext as NomenclatureClass;
                    DefineSelectedElement(NC);
                    UpdateMainCheckBoxChecked();
                }
            }
            ClearCutList();
        }

        // Метод: Выберает все элементы в DataGrid -------------------------------------------------------------------------------------------
        private void SelcectAllItemsDataGrid(CheckBox checkbox)
        {
            foreach (NomenclatureClass NC in DataGridNomenclatures.Items)
            {
                UpdateSelected = false;
                NC.Selected = checkbox.IsChecked;
                UpdateSelected = true;
                DefineSelectedElement(NC);
            }
        }

        // Метод: Определяет что и как отмечено ----------------------------------------------------------------------------------------------
        private void DefineSelectedElement(NomenclatureClass NC)
        {
            if (NC.GroupNomen == true) // Если номенклатура
            {
                if (NC.Selected == true) // Если CheckBox isChecked = true
                {
                    if (ExistenceCheck(NC.ID) == true)
                    {
                        AddNomenclature(NC); // Вызов метода: Добавить номенклатуру в список
                    }
                }
                if (NC.Selected == false)// Если CheckBox isChecked = false
                {
                    RemoveNomenclature(NC); // Вызов метода: Удалить номенклатуру из списка
                }
            }
            if (NC.GroupNomen == false) // Если группа
            {
                if (NC.Selected == true) // Если CheckBox isChecked = true
                {
                    if (ExistenceCheck(NC.ID) == true)
                    {
                        AddGroup(NC); // Вызов метода: Добавить группу в список
                    }
                }
                if (NC.Selected == false)// Если CheckBox isChecked = false
                {
                    RemoveGroup(NC); // Вызов метода: Удалить группу из списка
                }
            }
        }

        // Метод: Обновляет состояние "IsChecked" главного CheckBox-а ------------------------------------------------------------------------
        private void UpdateMainCheckBoxChecked()
        {
            int CheckedTrueCount = 0;
            int CheckedNullCount = 0;
            foreach (NomenclatureClass NC in DataGridNomenclatures.Items)
            {
                if (NC.Selected == true)
                    CheckedTrueCount++;
                if (NC.Selected == null)
                    CheckedNullCount++;
            }
            UpdateSelected = false;
            if (DataGridNomenclatures.Items.Count > 0 && CheckedTrueCount == DataGridNomenclatures.Items.Count)
            {
                MainSelectorCheckBox.IsChecked = true;
            }
            else if ((CheckedNullCount > 0 || CheckedTrueCount > 0) && CheckedTrueCount != DataGridNomenclatures.Items.Count)
            {
                MainSelectorCheckBox.IsChecked = null;
            }
            else if (DataGridNomenclatures.Items.Count == 0 || CheckedTrueCount == 0)
            {
                MainSelectorCheckBox.IsChecked = false;
            }
            UpdateSelected = true;
        }

        // Метод: Проверяет на наличие или отсуствие в списке "AllSelectedElementsList" ------------------------------------------------------
        private bool ExistenceCheck(Guid ID)
        {
            bool ok = true;
            foreach (NomenclatureClass SelList in AllSelectedElementsList)
            {
                if (ID == SelList.ID)
                {
                    ok = false;
                }
            }
            return ok;
        }

        // Метод: Если номенклатура отмечена Checked -----------------------------------------------------------------------------------------
        private void AddNomenclature(NomenclatureClass NC)
        {
            NC.Selected = true;
            NC.Icon = (Viewbox)Icons.GetIcon("Номенклатура");
            AllSelectedElementsList.Add(NC);
            SearchAndCheckParentGroup(NC.GroupID);
        }

        // Метод: Если номенклатура отмечена Unchecked ---------------------------------------------------------------------------------------
        private void RemoveNomenclature(NomenclatureClass NC)
        {
            Guid GroupID;
            foreach (NomenclatureClass SelList in AllSelectedElementsList)
            {
                if (NC.ID == SelList.ID)
                {
                    GroupID = SelList.GroupID;
                    AllSelectedElementsList.Remove(SelList);
                    SearchAndUnCheckParentGroup(GroupID);
                    break;
                }
            }
        }

        // Метод: Если группа отмечена Checked -----------------------------------------------------------------------------------------------
        private void AddGroup(NomenclatureClass NC)
        {
            NC.Icon = (Viewbox)Icons.GetIcon("Папка");
            AllSelectedElementsList.Add(NC);
            SearchAndCheckGroupChildren(NC.ID);
            SearchAndCheckParentGroup(NC.ID);
        }

        // Метод: Если группа отмечена Unchecked ---------------------------------------------------------------------------------------------
        private void RemoveGroup(NomenclatureClass NC)
        {
            Guid groupID = NC.GroupID;
            Guid iD = NC.ID;
            AllSelectedElementsList.Remove(NC);

            SearchAndUnCheckGroupChildren(iD);
            SearchAndUnCheckParentGroup(iD);
        }

        // Метод: Находит все child-ы группы и отмечает Checked ------------------------------------------------------------------------------
        private void SearchAndCheckGroupChildren(Guid GroupID)
        {
            UpdateSelected = false;
            foreach (NomenclatureClass ListDB in GetSelectedGroupChildren(GroupID))
            {
                if (ListDB.GroupNomen == true) // Если номенклатура
                {
                    if (ExistenceCheck(ListDB.ID) == true)
                    {
                        ListDB.Selected = true;
                        AddNomenclature(ListDB); // Вызов метода: Добавить номенклатуру в список
                    }
                    else
                    {
                        foreach (NomenclatureClass SelList in AllSelectedElementsList)
                        {
                            if (SelList.ID == ListDB.ID)
                            {
                                SelList.Selected = true;
                            }
                        }
                    }
                }
                else // Если эта группа
                {
                    if (ExistenceCheck(ListDB.ID) == true)
                    {
                        ListDB.Selected = true;
                        AddGroup(ListDB); // Вызов метода: Добавить номенклатуру в список
                    }
                    else
                    {
                        foreach (NomenclatureClass SelList in AllSelectedElementsList)
                        {
                            if (SelList.ID == ListDB.ID)
                            {
                                SelList.Selected = true;
                            }
                        }
                    }
                }

            }
            UpdateSelected = true;

        }

        // Метод: Находит все child-ы группы и отмечает Unchecked ----------------------------------------------------------------------------
        private void SearchAndUnCheckGroupChildren(Guid GroupID)
        {
            UpdateSelected = false;

            foreach (NomenclatureClass SelList in AllSelectedElementsList.ToArray())
            {
                if (SelList.GroupID == GroupID)
                {
                    if (SelList.GroupNomen == true) // Если номенклатура
                    {
                        SelList.Selected = false;
                        RemoveNomenclature(SelList);
                    }
                    else //Если группа
                    {
                        RemoveGroup(SelList);
                    }
                }
            }




            UpdateSelected = true;
        }

        // Метод: Находит все родительские группы отмечает в соответствии (+) ----------------------------------------------------------------
        private void SearchAndCheckParentGroup(Guid GroupID)
        {
            if (GroupID != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                UpdateSelected = false;

                NomenclatureClass Group = GetParentNomenclatureGroup(GroupID);
                int GroupChildrenCountDB = GetNomenclatureGroupChildrenCount(GroupID);
                int GroupChildrenCountSelectList = 0;

                foreach (NomenclatureClass SelList in AllSelectedElementsList)
                {
                    if (SelList.GroupID == GroupID && SelList.Selected == true)
                    {
                        GroupChildrenCountSelectList++;
                    }
                }
                if (GroupChildrenCountSelectList == GroupChildrenCountDB)
                {
                    if (ExistenceCheck(GroupID))
                    {
                        Group.Selected = true;

                        Group.Icon = (Viewbox)Icons.GetIcon("Папка");

                        AllSelectedElementsList.Add(Group);
                    }
                    else
                    {
                        foreach (NomenclatureClass SelList in AllSelectedElementsList)
                        {
                            if (SelList.ID == GroupID)
                            {
                                SelList.Selected = true;
                            }
                        }
                    }
                }
                if (GroupChildrenCountSelectList < GroupChildrenCountDB)
                {
                    if (ExistenceCheck(GroupID))
                    {
                        Group.Selected = null;

                        Group.Icon = (Viewbox)Icons.GetIcon("Папка");

                        AllSelectedElementsList.Add(Group);
                    }
                    else
                    {
                        foreach (NomenclatureClass SelList in AllSelectedElementsList)
                        {
                            if (SelList.ID == GroupID)
                            {
                                SelList.Selected = null;
                            }
                        }
                    }
                }
                if (Group.GroupID != new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    SearchAndCheckParentGroup(Group.GroupID);
                }
                UpdateSelected = true;
            }
        }

        // Метод: Находит все родительские группы отмечает в соответствии (-) ----------------------------------------------------------------
        private void SearchAndUnCheckParentGroup(Guid GroupID)
        {
            UpdateSelected = false;

            NomenclatureClass Group = GetParentNomenclatureGroup(GroupID);
            Guid groupID = Group.GroupID;
            int GroupChildrenCountDB = GetNomenclatureGroupChildrenCount(GroupID);
            int GroupChildrenCountSelectList = 0;
            bool SelectNull = false;

            foreach (NomenclatureClass SelList in AllSelectedElementsList)
            {
                if (SelList.GroupID == GroupID && SelList.Selected == true)
                {
                    GroupChildrenCountSelectList++;
                }
                if (SelList.GroupID == GroupID && SelList.Selected == null)
                {
                    SelectNull = true;
                }
            }
            if (GroupChildrenCountSelectList > 0)
            {
                if (GroupChildrenCountDB == GroupChildrenCountSelectList)
                {
                    foreach (NomenclatureClass SelList in AllSelectedElementsList)
                    {
                        if (SelList.ID == Group.ID)
                        {
                            SelList.Selected = true;
                        }
                    }
                }
                else
                {
                    foreach (NomenclatureClass SelList in AllSelectedElementsList)
                    {
                        if (SelList.ID == Group.ID)
                        {
                            SelList.Selected = null;
                        }
                    }
                }
            }
            else if (SelectNull == true)
            {
                foreach (NomenclatureClass SelList in AllSelectedElementsList)
                {
                    if (SelList.ID == Group.ID)
                    {
                        SelList.Selected = null;
                    }
                }
            }
            else
            {
                foreach (NomenclatureClass SelList in AllSelectedElementsList)
                {
                    if (SelList.ID == Group.ID)
                    {
                        AllSelectedElementsList.Remove(SelList);
                        break;
                    }
                }
            }

            if (groupID != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                SearchAndUnCheckParentGroup(Group.GroupID);
            }

            UpdateSelected = true;
        }

        // Метод: Возвращает родительскую группу элемента ------------------------------------------------------------------------------------
        private NomenclatureClass GetParentNomenclatureGroup(Guid GroupID)
        {
            NomenclatureClass GroupParent = new NomenclatureClass();
            foreach (NomenclatureClass NC in AllNomenclaturesList)
            {
                if (GroupID == NC.ID)
                {
                    if (NC.GroupNomen == true)
                    {
                        NC.Icon = (Viewbox)Icons.GetIcon("Номенклатура");
                    }
                    else
                    {
                        NC.Icon = (Viewbox)Icons.GetIcon("Папка");
                    }
                    GroupParent = NC;
                }
            }
            return GroupParent;
        }

        // Метод: Возвращает все Child-ы группы ----------------------------------------------------------------------------------------------
        private BindingList<NomenclatureClass> GetSelectedGroupChildren(Guid GroupID)
        {
            BindingList<NomenclatureClass> list = new BindingList<NomenclatureClass>();
            foreach (NomenclatureClass NC in AllNomenclaturesList)
            {
                if (GroupID == NC.GroupID)
                {
                    if (NC.GroupNomen == true)
                    {
                        NC.Icon = (Viewbox)Icons.GetIcon("Номенклатура");
                    }
                    else
                    {
                        NC.Icon = (Viewbox)Icons.GetIcon("Папка");
                    }
                    list.Add(NC);
                }
            }
            return list;
        }

        // Метод: Возвращает количество child-ов ---------------------------------------------------------------------------------------------
        private int GetNomenclatureGroupChildrenCount(Guid GroupID)
        {
            int count = 0;
            foreach (NomenclatureClass NC in AllNomenclaturesList)
            {
                if (GroupID == NC.GroupID)
                {
                    count++;
                }
            }
            return count;
        }

        // Метод: Отмечает выбранные согласно списку AllSelectedElementsList -----------------------------------------------------------------
        private void DataGridSelectionUpdate()
        {
            if (DataGridNomenclatures.Items.Count > 0)
            {
                UpdateSelected = false;
                foreach (NomenclatureClass DGItem in DataGridNomenclatures.Items)
                {
                    if (AllSelectedElementsList.Count > 0)
                    {
                        foreach (NomenclatureClass SelList in AllSelectedElementsList)
                        {
                            if (SelList.ID == DGItem.ID)
                            {
                                DGItem.Selected = SelList.Selected;
                            }
                        }
                    }
                    else
                    {
                        DGItem.Selected = false;
                    }
                    foreach (NomenclatureClass SelList in AllNomenclaturesList)
                    {
                        if (SelList.ID == DGItem.ID)
                        {
                            DGItem.CutOut = SelList.CutOut;
                        }
                    }
                }
                UpdateSelected = true;
                UpdateMainCheckBoxChecked();
            }
        }

        // Метод: Считает количество выбранных номенклатур и присваивает значение к переменной "SelectedCount" -------------------------------
        private void SetSelectedCount()
        {
            int count = 0;
            foreach (NomenclatureClass SelList in AllSelectedElementsList)
            {
                if (SelList.GroupNomen == true)
                {
                    count++;
                }
            }
            SelectedNomenclaturesCount = count;
        }

        // Метод: Открывает окно с выбранными номенклатурами ---------------------------------------------------------------------------------
        private void OpenSelectedNomenclaturesList()
        {
            ToolsGrid.Visibility = Visibility.Visible;
            SelectedNomenclatures SelectedNomens = new SelectedNomenclatures(ref AllSelectedElementsList);
            ToolsContentControl.Content = SelectedNomens;
            SelectedNomens.ButtonClose.Click += SelectedNomenclaturesButtonClose_Click;
        }


        private void CleareSelectList()
        {
            SelectedNomenclaturesCount = 0;
            AllSelectedElementsList.Clear();
            foreach(NomenclatureClass NC in AllNomenclaturesList)
            {
                NC.Selected = false;
            }
            DataGridSelectionUpdate();
        }


        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


        #region Вырезать/Вставить ============================================================================================================

        // Список для основных групп вырезанных элементов ------------------------------------------------------------------------------------

        private BindingList<NomenclatureClass> CutMainGroupList = new BindingList<NomenclatureClass>();

        private void CutElements()
        {
            CleareSelectList();
            bool one = true;
            foreach (NomenclatureClass NC in DataGridNomenclatures.SelectedItems)
            {
                NC.Selected = false;
                EditableElementID = NC.ID;
                if (NC.CutOut == true)
                {
                    if (CheckCutParentGroup(NC) == true)
                    {
                        NC.CutOut = !NC.CutOut;
                        CheckCutNomenclaturesList(NC);
                    }
                    else
                    {
                        if (one)
                        {
                            MessageBox.Show("Группа в которой находится элелемент вырезано!", "Группа вырезано");

                            one = false;
                        }
                    }
                }
                else
                {
                    NC.CutOut = !NC.CutOut;
                    CheckCutNomenclaturesList(NC);
                }
            }
            
            //SelectLastCreated(EditableElementID);
        }

        private void PasteElements()
        {
            bool flag = false;
            foreach (NomenclatureClass NC in CutMainGroupList)
            {
                if (NC.GroupID != CurrentGroupID)
                {
                    if (NC.ID != CurrentGroupID)
                    {
                        flag = DataBaseRequest.UpdateGroupID(NC.ID, CurrentGroupID);
                        foreach(NomenclatureClass Nlist in AllNomenclaturesList)
                        {
                            if (Nlist.ID == NC.ID)
                                Nlist.GroupID = CurrentGroupID;
                        }
                    }
                }
                else
                {
                    flag = true;
                }
            }
            if (flag == true)
            {
                CountOfCutElements = 0;
                FillingNomenclaturesList();
                SelectPastedElements();
                ClearCutList();
            }
        }

        private void SelectPastedElements()
        {
            foreach (NomenclatureClass item in DataGridNomenclatures.Items)
            {
                foreach (NomenclatureClass NC in CutMainGroupList)
                {
                    if (item.ID == NC.ID)
                    {
                        DataGridNomenclatures.SelectedItem = item;
                    }
                }
            }
        }

        private void ClearCutList()
        {
            CutMainGroupList.Clear();
            foreach (NomenclatureClass Nlist in AllNomenclaturesList)
            {
                Nlist.CutOut = false;
            }
            foreach (NomenclatureClass DGlist in DataGridNomenclatures.Items)
            {
                DGlist.CutOut = false;
            }
            CountOfCutElements = 0;
        }


        // Метод: Отмечает вырезанный элемент в списке "AllNomenclaturesList" ----------------------------------------------------------------
        private void CheckCutNomenclaturesList(NomenclatureClass NC)
        {
            foreach (NomenclatureClass list in AllNomenclaturesList)
            {
                if (list.ID == NC.ID)
                {
                    list.CutOut = NC.CutOut;

                    GetParentGroup(NC);

                    if (NC.GroupNomen == false)
                        CutChildren(NC);
                }
            }
            CheckCutCount();
        }


        private void GetParentGroup(NomenclatureClass NC)
        {

            bool flag = false;

            if (NC.GroupID != new Guid("00000000-0000-0000-0000-000000000000") && NC.CutOut == true)
            {
                foreach (NomenclatureClass NList in AllNomenclaturesList)
                {
                    if (NC.GroupID == NList.ID)
                    {
                        if (NList.CutOut == true)
                        {
                            GetParentGroup(NList);
                        }
                        else
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                flag = true;
            }
            if (flag == true)
            {
                CutMainGroupList.Add(NC);
            }
        }

        // Метод: Считает количество вырезанных и присваивает значение к переменной "CutCount" -----------------------------------------------
        private void CheckCutCount()
        {
            int count = 0;
            foreach (NomenclatureClass NC in AllNomenclaturesList)
            {
                if (NC.CutOut == true)
                {
                    count++;
                }
            }
            CountOfCutElements = count;
        }

        // Метод: Проверяет вырезано-ли родителская группа вырезаемого элемента --------------------------------------------------------------
        private bool CheckCutParentGroup(NomenclatureClass NC)
        {
            bool ok = true;
            foreach (NomenclatureClass Nlist in AllNomenclaturesList)
            {
                if (Nlist.ID == NC.GroupID && Nlist.CutOut == true)
                    ok = false;
            }
            return ok;
        }


        // Метод: Вырезает все Child-ы группы ------------------------------------------------------------------------------------------------
        private void CutChildren(NomenclatureClass NC)
        {
            foreach (NomenclatureClass Nlist in AllNomenclaturesList)
            {
                if (Nlist.GroupID == NC.ID)
                {
                    Nlist.CutOut = NC.CutOut;
                    if (Nlist.GroupNomen == false)
                        CutChildren(Nlist);
                }
            }
        }

        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        private void ChangeViewRadioButtonCheck(object sender, RoutedEventArgs e)
        {
            RadioButton RB = sender as RadioButton;
            ViewTableMode = (bool)RB.IsChecked;
            ChangeView();
        }

        private void ChangeView()
        {
            if (ViewTableMode == true)
            {
                DataGridNomenclatures.Visibility = Visibility.Visible;
                RootGrid.Visibility = Visibility.Collapsed;
                FillingNomenclaturesList();
                HeaderBorder.Height = 40;
                HeaderBorder.BorderThickness = new Thickness(0);
                HeaderGrid.Margin = new Thickness(0);
            }
            else
            {
                RootGrid.Visibility = Visibility.Visible;
                DataGridNomenclatures.Visibility = Visibility.Collapsed;
                ListBoxNomenclatures.Focus();
                HeaderBorder.Height = 55;
                HeaderBorder.BorderThickness = new Thickness(0, 0, 0, 1);
                HeaderGrid.Margin = new Thickness(0, 0, 0, 14);

                RootGrid.DataContext = ShowNomenclatureList;
                FillingNomenclaturesList();
            }
        }
    }












































}
