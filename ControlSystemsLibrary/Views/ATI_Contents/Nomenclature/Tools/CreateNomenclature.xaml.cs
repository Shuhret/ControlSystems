using ControlSystemsLibrary.Classes;
using Microsoft.Win32;
using ShuhratControlLibrary.Controls;
using ShuhratControlLibrary.Controls.Loaders;
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ControlSystemsLibrary.Views.ATI_Contents.Nomenclature.Tools
{

    public partial class CreateNomenclature : UserControl, INotifyPropertyChanged
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


        private delegate void PrivateDelegate();


        #region Констукторы ==================================================================================================================

        // Конструктор 1-Перегрузка (для создания) -------------------------------------------------------------------------------------------
        public CreateNomenclature(bool CreateMode, AfterItemCreationDelegate AfterCreationDelegate, Guid CurrentGroupID)
        {
            this.CreateMode = CreateMode;
            this.AfterCreationDelegate = AfterCreationDelegate;
            this.CurrentGroupID = CurrentGroupID;
            InitializeComponent();
        }

        // Конструктор 2-Перегрузка (для изменения) ------------------------------------------------------------------------------------------
        public CreateNomenclature(bool CreateMode, AfterItemCreationDelegate AfterCreationDelegate, NomenclatureClass EditableNomenclature)
        {
            this.CreateMode = CreateMode;
            this.AfterCreationDelegate = AfterCreationDelegate;
            this.EditableNomenclature = EditableNomenclature;

            InitializeComponent();
        }

        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


        #region Поля и свойства ==============================================================================================================


        // Объект Делегата
        AfterItemCreationDelegate AfterCreationDelegate;

         // Редактируемая номенлатура
        NomenclatureClass EditableNomenclature { get; set; }

        // Режим Создание/Редактирование
        private bool createMode;
        public bool CreateMode
        {
            get
            {
                return createMode;
            }
            set
            {
                createMode = value;
                if (value)
                {
                    ID = Guid.NewGuid();
                    CreateButtonText = "Создать";
                    HeaderText = "Создание новой номенклатуры";
                }
                else
                {
                    CreateButtonText = "Принять изменения";
                    HeaderText = "Редактирование номенклатуры";
                }
                OnPropertyChanged();
            }
        }

        // Enabled кнопки "Создать/Принять изменения"
        private bool createButtonEnabled;
        public bool CreateButtonEnabled
        {
            get
            {
                return createButtonEnabled;
            }
            set
            {
                createButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        // Текст кнопки "Создать/Принять изменения"
        private string createButtonText;
        public string CreateButtonText
        {
            get
            {
                return createButtonText;
            }
            set
            {
                createButtonText = value;
                OnPropertyChanged();
            }
        }

        // Текст заголовка формы
        private string headerText;
        public string HeaderText
        {
            get
            {
                return headerText;
            }
            set
            {
                headerText = value;
                OnPropertyChanged();
            }
        }


        // ID Создаваемой/редактируемой номенклатуры
        private Guid id;
        public Guid ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        // ID группы Создаваемой/редактируемой номенклатуры
        private Guid groupID;
        public Guid CurrentGroupID
        {
            get
            {
                return groupID;
            }
            set
            {
                groupID = value;
                OnPropertyChanged();
            }
        }

        // Артикул Создаваемой/редактируемой номенклатуры
        private string article;
        public string Article
        {
            get
            {
                return article;
            }
            set
            {
                article = value;
                OnPropertyChanged();
            }
        }

        // Название Создаваемой/редактируемой номенклатуры
        private string nomenclatureName = "";
        public string NomenclatureName
        {
            get
            {
                return nomenclatureName;
            }
            set
            {
                nomenclatureName = value;
                OnPropertyChanged();
            }
        }

        // Базовая единица Создаваемой/редактируемой номенклатуры
        private string baseUnitName = "";
        public string BaseUnitName
        {
            get
            {
                return baseUnitName;
            }
            set
            {
                baseUnitName = value;
                OnPropertyChanged();
            }
        }

        // Вес базовой единицы Создаваемой/редактируемой номенклатуры
        private double baseUnitWeight;
        public double BaseUnitWeight
        {
            get
            {
                return baseUnitWeight;
            }
            set
            {
                baseUnitWeight = value;
                
                OnPropertyChanged();
            }
        }

        // Вес базовой единицы Создаваемой/редактируемой номенклатуры для отображения в TextBox-е 
        private string baseUnitWeightText = "";
        public string BaseUnitWeightText
        {
            get
            {
                return baseUnitWeightText;
            }
            set
            {
                baseUnitWeightText = value;
                OnPropertyChanged();
            }
        }

        // Страна происхождения Создаваемой/редактируемой номенклатуры
        private string country;
        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                country = value;
                OnPropertyChanged();
            }
        }

        // Описание Создаваемой/редактируемой номенклатуры
        public string Description { get; set; }

        // Тип Штрих-кода базовой единицы Создаваемой/редактируемой номенклатуры
        public string BarcodeType { get; set; }

        // Штрих-код базовой единицы Создаваемой/редактируемой номенклатуры
        public string Barcode { get; set; }

        // Тег "Акция" Создаваемой/редактируемой номенклатуры
        public bool TagAksia { get; set; }

        // Тег "Новинка" Создаваемой/редактируемой номенклатуры
        public bool TagNew { get; set; }

        // Тег "Фокус" Создаваемой/редактируемой номенклатуры
        public bool TagFocus { get; set; }

        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


        #region События ======================================================================================================================

        // Событие: Loaded (Загрузка формы) --------------------------------------------------------------------------------------------------
        private void CN_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                LoadUnits();
                LoadCountry();
                LoadBarcodeTypes();

                if (CreateMode == false)
                {
                    AssignsEditableValues();
                    SelectBaseUnitItem();
                    SelectCountryItem();

                    LoadBaseBarcode();
                    LoadAdditionalUnits();
                    LoadPropertiesValues();
                    LoadNomenclatureImages();
                }
            })
            { IsBackground = true }.Start();

            ArticleTextBox.Focus();
            ArticleTextBox.SelectionStart = 0;
            ArticleTextBox.SelectionLength = this.ArticleTextBox.Text.Length;
        }



        // Событие: PreviewKeyDown (Для навигации по страницам с помошью Ctrl+Left или Ctrl+Right) -------------------------------------------
        private void CN_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (CreateButtonEnabled)
            {
                if ((e.Key == Key.Right) && (e.KeyboardDevice.Modifiers == ModifierKeys.Control))
                {
                    ButtonNext_Click(null, null);
                }
                if ((e.Key == Key.Left) && (e.KeyboardDevice.Modifiers == ModifierKeys.Control))
                {
                    ButtonBack_Click(null, null);
                }
            }
        }

        // Событие: PreviewTextInput (Для введенияв TextBox только цифр) ---------------------------------------------------------------------
        private void DigitText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            double num;
            string text = e.Text;
            bool flag = false;
            foreach (char ch in ((TextBox)sender).Text)
            {
                if (ch == ',')
                {
                    flag = true;
                    break;
                }
            }
            if ((((TextBox)sender).Text.Length == 0) && (text == "0"))
            {
                ((TextBox)sender).Text = "0,";
                ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
                ((TextBox)sender).SelectionLength = 0;
                flag = true;
                e.Handled = true;
            }
            if (((((TextBox)sender).Text.Length == 0) && (text == ",")) || (text == "."))
            {
                ((TextBox)sender).Text = "0,";
                ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
                ((TextBox)sender).SelectionLength = 0;
                flag = true;
                e.Handled = true;
            }
            else if ((text == ",") && !flag)
            {
                TextBox box1 = (TextBox)sender;
                box1.Text = box1.Text + ",";
                text = ",";
                flag = true;
                ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
                ((TextBox)sender).SelectionLength = 0;
                e.Handled = true;
            }
            if (((text == ".") || (text == ",")) & flag)
            {
                e.Handled = true;
            }
            if (!double.TryParse(text, out num) && !(text == "."))
            {
                e.Handled = true;
            }
        }

        // Событие: MouseLeftButtonDown (Для добавления элементов двойным кликом по StacPanel) -----------------------------------------------
        private void MouseLeftButtonDoubleDownClickForAdd(object sender, MouseButtonEventArgs e)
        {
            if ((e.ClickCount == 2) && (sender is StackPanel))
            {
                StackPanel panel = sender as StackPanel;
                if (panel.Name == "AddUnitsStackPanel")
                {
                    AddAddUnitButton_Click(null, null);
                }
                if (panel.Name == "AddPropertiesStackPanel")
                {
                    AddPropertyButton_Click(null, null);
                }
            }
        }

        // Событие: Checked/Unchecked (Для RadioButton-ов по страничной навигации) -----------------------------------------------------------
        private void RadioButtonsChecked(object sender, RoutedEventArgs e)
        {
            if ((sender is RadioButton) && (WorkGrid != null))
            {
                RadioButton button = sender as RadioButton;
                ShowPage(button.Name);
            }
        }

        // Событие: KeyDown (Для перехода на следующий элемент по нажатию Enter) -------------------------------------------------------------
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((sender is TextBox) && (e.Key == Key.Return))
            {
                TextBox box = sender as TextBox;
                string name = box.Name;
                if (!(name == "ArticleTextBox"))
                {
                    if (name == "NomenNameTextBox")
                    {
                        BaseUnitComboBox.Focus();
                        BaseUnitComboBox.IsDropDownOpen = true;
                    }
                    else if (name == "TextBoxBaseWeight")
                    {
                        ComboBoxCountry.Focus();
                        ComboBoxCountry.IsDropDownOpen = true;
                    }
                }
                else
                {
                    NomenNameTextBox.Focus();
                    NomenNameTextBox.SelectionStart = 0;
                    NomenNameTextBox.SelectionLength = NomenNameTextBox.Text.Length;
                }
            }
        }

        // Событие: GotFocus (Для отображения веса в формате "10.5кг" по потере элементом фокуса) --------------------------------------------
        private void TextBoxBaseWeight_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox box = sender as TextBox;
                if ((box.Name == "TextBoxBaseWeight") && (BaseUnitWeight > 0.0))
                {
                    box.Text = BaseUnitWeight.ToString();
                }
            }
        }

        // Событие: GotFocus (Для отображения веса в формате "10.5" по получении элементом фокуса) -------------------------------------------
        private void TextBoxBaseWeight_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox box = sender as TextBox;
                if (box.Name == "TextBoxBaseWeight")
                {
                    double num;
                    if (double.TryParse(box.Text, out num))
                    {
                        double num2 = Convert.ToDouble(box.Text);
                        if (num2 > 0.0)
                        {
                            BaseUnitWeight = num2;
                            BaseUnitWeightText = BaseUnitWeight.ToString() + " кг";
                            
                        }
                        else
                        {
                            BaseUnitWeight = 0.0;
                            box.Text = string.Empty;
                        }
                    }
                    else
                    {
                        BaseUnitWeight = 0.0;
                    }
                }
            }
        }

        // Событие: TextChanged (Для проверки заполнения основных важных значений для создания номенклатуры) ---------------------------------
        private void TextBoxes_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckMainValues();
        }




        // Событие: Click (Для добавления новой Доп. Единицы измерения) ----------------------------------------------------------------------
        private void AddAddUnitButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.CheckReadinessAddUnits())
            {
                AdditionalUnitsUC AUUC = new AdditionalUnitsUC
                {
                    ID = Guid.NewGuid(),
                    NomenclatureID = ID,
                    BaseUnitName = BaseUnitName,
                    BaseUnitWeight = BaseUnitWeight
                };
                AUUC.UnitsComboBox.SelectionChanged += AdditionalUnitsComboBox_SelectionChanged;
                AUUC.DeleteButton.Click += AdditionalUnitsDeleteButton_Click;

                LoadAddUnitsToComboBox(ref AUUC);
                
                AddUnitsStackPanel.Children.Add(AUUC);
                (AddUnitsStackPanel.Parent as ScrollViewer).ScrollToEnd();
            }
        }

        // Событие: Click (Для Удаления Доп. единицы измерения) ------------------------------------------------------------------------------
        private void AdditionalUnitsDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            AdditionalUnitsUC parent = (((sender as Button).Parent as Grid).Parent as Border).Parent as AdditionalUnitsUC;
            AddUnitsStackPanel.Children.Remove(parent);
            CheckBarcodesAfterChangeUnits();
        }

        // Событие: Click (Для добавления нового штрих-кода) ---------------------------------------------------------------------------------
        private void AddBarcodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReadyForCreateBarcode())
            {
                AddBarcode((BarcodeUnitsComboBox.SelectedItem as ComboBoxItem).Content.ToString(), (BarcodeTypesComboBox.SelectedItem as ComboBoxItem).Content.ToString(), BarcodeTextBox.Text);
            }
        }

        // Событие: Click (Для добавления нового изображения) --------------------------------------------------------------------------------
        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files(*.jpg, *.png) | *.jpg; *.png";

                if (openFileDialog.ShowDialog() == true)
                {
                    AddImageControl(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nПопробуйте загрузить другое изображение.", "Ошибка загрузки изображения");
            }
        }

        // Событие: Click (Для добавления нового свойства) -----------------------------------------------------------------------------------
        private void AddPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckReadinessProperties())
            {

                NomenPropertyUC NPUC = new NomenPropertyUC();
                NPUC.PropertyComboBox.SelectionChanged += new SelectionChangedEventHandler(PropertyComboBox_SelectionChanged);
                LoadPropertiesInComboBox(ref NPUC.PropertyComboBox);

                NPUC.PropertyComboBox.IsDropDownOpen = true;
                AddPropertiesStackPanel.Children.Add(NPUC);
                (AddPropertiesStackPanel.Parent as ScrollViewer).ScrollToEnd();
            }
        }

        // Событие: Click (Для Создания/Изменения номенклатуры) <<<<<<<<<<<<<<<<<<------------------------------------------------------------
        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (CreateMode)
            {
                CreateNomenclatureDB();
            }
        }

        // Событие: Click (Для перехода в следующую страницу) --------------------------------------------------------------------------------
        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < RadioButtonsStackPanel.Children.Count; i++)
            {
                if (RadioButtonsStackPanel.Children[i] is RadioButton)
                {
                    bool? isChecked = (RadioButtonsStackPanel.Children[i] as RadioButton).IsChecked;
                    bool flag3 = true;
                    if ((isChecked.GetValueOrDefault() == flag3) & isChecked.HasValue)
                    {
                        if (i < (RadioButtonsStackPanel.Children.Count - 1))
                        {
                            (RadioButtonsStackPanel.Children[i + 1] as RadioButton).IsChecked = true;
                            break;
                        }
                        if (i == (RadioButtonsStackPanel.Children.Count - 1))
                        {
                            (RadioButtonsStackPanel.Children[0] as RadioButton).IsChecked = true;
                            break;
                        }
                    }
                }
            }
        }

        // Событие: Click (Для перехода в предыдущую страницу) -------------------------------------------------------------------------------
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < RadioButtonsStackPanel.Children.Count; i++)
            {
                if (RadioButtonsStackPanel.Children[i] is RadioButton)
                {
                    bool? isChecked = (RadioButtonsStackPanel.Children[i] as RadioButton).IsChecked;
                    bool flag3 = true;
                    if ((isChecked.GetValueOrDefault() == flag3) & isChecked.HasValue)
                    {
                        if (i == 0)
                        {
                            (RadioButtonsStackPanel.Children[RadioButtonsStackPanel.Children.Count - 1] as RadioButton).IsChecked = true;
                            break;
                        }
                        if (i > 0)
                        {
                            (RadioButtonsStackPanel.Children[i - 1] as RadioButton).IsChecked = true;
                            break;
                        }
                    }
                }
            }
        }




        // Событие: SelectionChanged (Для ComboBox Доп. единица измерения) -------------------------------------------------------------------
        private void AdditionalUnitsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox box = sender as ComboBox;
                AdditionalUnitsUC AUUC = ((box.Parent as Grid).Parent as Border).Parent as AdditionalUnitsUC;
                if (box.SelectedItem != null)
                {
                    string unitName = (box.SelectedItem as ComboBoxItem).Content.ToString();
                    if (CompareToBaseUnit(unitName))
                    {
                        if (CheckWithAdditionalUnits(AUUC.ID, unitName))
                        {
                            AUUC.AddUnitName = unitName;
                            if (CreateMode)
                            {
                                AUUC.QuantityTextBox.Focus();
                                AUUC.QuantityTextBox.SelectionStart = 0;
                                AUUC.QuantityTextBox.SelectionLength = AUUC.QuantityTextBox.Text.Length;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Единица \"" + unitName + "\" уже имеется в списке.", "Исправьте!");
                            box.SelectedItem = null;
                            box.IsDropDownOpen = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Единица \"" + unitName + "\" уставнолена как базовая единица.", "Исправьте!");
                        AUUC.Readiness = false;
                        box.SelectedItem = null;
                        box.IsDropDownOpen = true;
                    }
                }
            }
        }

        // Событие: SelectionChanged (Для ComboBox Базовая единица измерения) ----------------------------------------------------------------
        private void BaseUnitNameComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
            {
                string unitName = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();

                if (CheckWithAdditionalUnits(unitName))
                {
                    BaseUnitName = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();

                    TextBoxBaseWeight.Focus();
                    TextBoxBaseWeight.SelectionStart = 0;
                    TextBoxBaseWeight.SelectionLength = TextBoxBaseWeight.Text.Length;
                }
                else
                {
                    string message = "Единица " + '"' + unitName + '"' + " указано в списке дополнительных единиц.\nУдалить единицу " + '"' + unitName + '"' + " из списка дополнительных единиц?";

                    if (MessageBox.Show(message, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        // Если нажато "Нет"
                        foreach (ComboBoxItem item in (sender as ComboBox).Items)
                        {
                            if (item.Content.ToString() == BaseUnitName)
                            {
                                (sender as ComboBox).SelectedItem = item;
                            }
                        }
                    }
                    else
                    {
                        // Если нажато "Да"
                        RemoveAddedUnit(unitName);
                    }
                }
            }
        }

        // Событие: SelectionChanged (Для ComboBox Страна происхождения) ---------------------------------------------------------------------
        private void CountryComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
            {
                Country = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();
                DescriptionTextBox.Focus();
            }
        }

        // Событие: SelectionChanged (Для ComboBox Свойство ) --------------------------------------------------------------------------------
        private void PropertyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox box = sender as ComboBox;
                ComboBox valueComboBox = (((box.Parent as Grid).Parent as Border).Parent as NomenPropertyUC).ValueComboBox;
                LoadValuesInComboBox(ref valueComboBox, (box.SelectedItem as ComboBoxItem).Content.ToString());
                if (CreateMode)
                {
                    valueComboBox.IsDropDownOpen = true;
                }
            }
        }


        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


        #region Методы =======================================================================================================================

        // Метод: Добавляет Штрих-код в WrapPanel --------------------------------------------------------------------------------------------
        private void AddBarcode(string UnitName, string BarcodeType, string Barcode)
        {
            bool readiness = false;
            BarcodeUC element = new BarcodeUC
            {
                BacodeImage = BarCodeClass.GetBarcodeBitmapSource(ref readiness, BarcodeType, Barcode),
                Readiness = readiness
            };
            if (readiness)
            {
                element.UnitName = UnitName;
                element.BarcodeType = BarcodeType;
                element.Barcode = Barcode;
                AddBarcodesWrapPanel.Children.Add(element);
                (AddBarcodesWrapPanel.Parent as ScrollViewer).ScrollToEnd();
                ClearBarcodeValues();
            }
        }

        // Метод: Добавляет Изображение в WrapPanel из полученной ссылки на файл -------------------------------------------------------------
        private void AddImageControl(string ImagePath)
        {
            NomenImageUC element = new NomenImageUC
            {
                ImagePath = ImagePath,
                Width = 294.5,
                Height = 200.0,
                NomenImage = new BitmapImage(new Uri(ImagePath))
            };
            AddImagesWrapPanel.Children.Add(element);
            (AddImagesWrapPanel.Parent as ScrollViewer).ScrollToEnd();
        }

        // Метод: Добавляет элемент "Свойство и значение" в StackPanel -----------------------------------------------------------------------
        private void AddPropertiesValues(string PropertyName, string ValueName)
        {
            NomenPropertyUC element = new NomenPropertyUC
            {
                Readiness = true
            };
            element.PropertyComboBox.SelectionChanged += new SelectionChangedEventHandler(PropertyComboBox_SelectionChanged);
            LoadPropertiesInComboBox(ref element.PropertyComboBox);
            foreach (ComboBoxItem item in element.PropertyComboBox.Items)
            {
                if (item.Content.ToString() == PropertyName)
                {
                    element.PropertyComboBox.SelectedItem = item;
                }
            }
            foreach (ComboBoxItem item2 in element.ValueComboBox.Items)
            {
                if (item2.Content.ToString() == ValueName)
                {
                    element.ValueComboBox.SelectedItem = item2;
                }
            }
            AddPropertiesStackPanel.Children.Add(element);
            (AddPropertiesStackPanel.Parent as ScrollViewer).ScrollToEnd();
        }

        // Метод: Заполняет форму значениями редактируемой номенклатуры ----------------------------------------------------------------------
        private void AssignsEditableValues()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                ID = EditableNomenclature.ID;
                CurrentGroupID = EditableNomenclature.GroupID;
                Article = EditableNomenclature.Article;
                NomenclatureName = EditableNomenclature.Name;
                BaseUnitName = EditableNomenclature.BaseUnit;
                BaseUnitWeight = EditableNomenclature.WeightBaseUnit;
                Country = EditableNomenclature.CountryOfOrigin;
                Description = EditableNomenclature.Description;
                BarcodeType = EditableNomenclature.BarcodeType;
                Barcode = EditableNomenclature.Barcode;
                TagAksia = EditableNomenclature.Aksia;
                TagFocus = EditableNomenclature.Focus;
                TagNew = EditableNomenclature.New;
            }));
        }

        // Метод: Удаляет Штрих-код(если создан) при удалении уже созданной Доп. Единицы -----------------------------------------------------
        private void CheckBarcodesAfterChangeUnits()
        {
            if (AddBarcodesWrapPanel.Children.Count > 0)
            {
                ArrayList list = new ArrayList();
                foreach (ComboBoxItem item in BarcodeUnitsComboBox.Items)
                {
                    list.Add(item.Content.ToString());
                }
                foreach (BarcodeUC euc in AddBarcodesWrapPanel.Children)
                {
                    bool flag2 = false;
                    foreach (string str in list)
                    {
                        if (euc.UnitName == str)
                        {
                            flag2 = true;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        AddBarcodesWrapPanel.Children.Remove(euc);
                        MessageBox.Show("Также удален штрих код для единицы: " + euc.UnitName, "Предупреждение.");
                    }
                }
            }
        }

        // Метод: Проверяет указаны-ли важные значения для создания номенклатуры -------------------------------------------------------------
        private void CheckMainValues()
        {
            if (BaseUnitWeightText != "" && NomenclatureName != "" && BaseUnitName != "")
            {
                CreateButtonEnabled = true;
            }
            else
            {
                CreateButtonEnabled = false;
            }
        }
        
        // Метод: Проверяет "Готовность" созданных Доп. Единиц измерения ---------------------------------------------------------------------
        private bool CheckReadinessAddUnits()
        {
            bool flag = true;
            if (AddUnitsStackPanel.Children.Count == 0)
            {
                return true;
            }
            foreach (AdditionalUnitsUC AUUC in AddUnitsStackPanel.Children)
            {
                if (!AUUC.Readiness)
                {
                    flag = false;
                }
            }
            return flag;
        }

        // Метод: Проверяет "Готовность" созданных свойств и значений ------------------------------------------------------------------------
        private bool CheckReadinessProperties()
        {
            bool flag = true;
            if (AddPropertiesStackPanel.Children.Count == 0)
            {
                return true;
            }
            foreach (NomenPropertyUC yuc in AddPropertiesStackPanel.Children)
            {
                if (!yuc.Readiness)
                {
                    flag = false;
                }
            }
            return flag;
        }

        // Метод: Проверяет на наличие такой единицы измерения в Доп. Единицах (1-перегрузка) ------------------------------------------------
        private bool CheckWithAdditionalUnits(string UnitName)
        {
            bool flag = true;
            foreach (AdditionalUnitsUC AUUC in AddUnitsStackPanel.Children)
            {
                if (AUUC.AddUnitName == UnitName)
                {
                    AUUC.Readiness = false;
                    flag = AUUC.Readiness;
                    AUUC.Readiness = true;
                }
            }
            return flag;
        }

        // Метод: Проверяет на наличие такой единицы измерения в Доп. Единицах (2-перегрузка) ------------------------------------------------
        private bool CheckWithAdditionalUnits(Guid ID, string UnitName)
        {
            bool readiness = true;
            foreach (AdditionalUnitsUC suc in AddUnitsStackPanel.Children)
            {
                if ((suc.ID != ID) && (suc.AddUnitName == UnitName))
                {
                    suc.Readiness = false;
                    readiness = suc.Readiness;
                    suc.Readiness = true;
                }
            }
            return readiness;
        }

        // Метод: Очищает все значения формы -------------------------------------------------------------------------------------------------
        private void ClearAll()
        {
            ID = Guid.NewGuid();
            ArticleTextBox.Clear();
            NomenNameTextBox.Clear();
            BaseUnitComboBox.SelectedItem = null;
            TextBoxBaseWeight.Clear();
            BaseUnitWeight = 0.0;
            ComboBoxCountry.SelectedItem = null;
            DescriptionTextBox.Clear();
            AddUnitsStackPanel.Children.Clear();
            AddPropertiesStackPanel.Children.Clear();
            AddBarcodesWrapPanel.Children.Clear();
            BarcodeUnitsComboBox.Items.Clear();
            BarcodeTypesComboBox.SelectedItem = null;
            BarcodeTextBox.Clear();
            AddImagesWrapPanel.Children.Clear();
            MainRB.IsChecked = true;
            ArticleTextBox.Focus();
        }

        // Метод: Очищает значения элементов создания штрих-кода -----------------------------------------------------------------------------
        private void ClearBarcodeValues()
        {
            BarcodeUnitsComboBox.SelectedItem = null;
            BarcodeTypesComboBox.SelectedItem = null;
            BarcodeTextBox.Clear();
        }

        // Метод: Сверяет с базовой единицей -------------------------------------------------------------------------------------------------
        private bool CompareToBaseUnit(string UnitName)
        {
            if (BaseUnitName == UnitName)
            {
                return false;
            }
            return true;
        }

        // Метод: Добавляет Доп. единицы измерения в Базу данных -----------------------------------------------------------------------------
        private bool CreateAddUnitsDB()
        {
            bool flag = false;
            if (AddUnitsStackPanel.Children.Count > 0)
            {
                AdditionalUnitsClass aUC = new AdditionalUnitsClass();
                foreach (AdditionalUnitsUC suc in AddUnitsStackPanel.Children)
                {
                    if (suc.Readiness)
                    {
                        aUC.ID = Guid.NewGuid();
                        aUC.NomenclatureID = ID;
                        aUC.AddUnitName = suc.AddUnitName;
                        aUC.Quantity = suc.Quantity;
                        aUC.Weight = suc.Weight;

                        if (AddBarcodesWrapPanel.Children.Count > 0)
                        {
                            foreach (BarcodeUC euc in AddBarcodesWrapPanel.Children)
                            {
                                if (euc.UnitName == suc.AddUnitName)
                                {
                                    aUC.BarcodeType = euc.BarcodeType;
                                    aUC.Barcode = euc.Barcode;
                                }
                            }
                        }

                        flag = DataBaseRequest.CreateAdditionalUnits(aUC);
                    }
                    else
                    {
                        flag = true;
                    }
                }
                return flag;
            }
            return true;
        }

        // Метод: Добавляет Номенклатуру в Базу данных ---------------------------------------------------------------------------------------
        private void CreateNomenclatureDB()
        {
            NomenclatureClass nomen = new NomenclatureClass
            {
                ID = this.ID,
                GroupID = this.CurrentGroupID,
                GroupNomen = true,
                Article = this.ArticleTextBox.Text,
                Name = this.NomenNameTextBox.Text,
                BaseUnit = this.BaseUnitName,
                WeightBaseUnit = this.BaseUnitWeight
            };
            if (ComboBoxCountry.SelectedItem != null)
            {
                nomen.CountryOfOrigin = (this.ComboBoxCountry.SelectedItem as ComboBoxItem).Content.ToString();
            }
            nomen.Description = this.DescriptionTextBox.Text;
            foreach (BarcodeUC euc in this.AddBarcodesWrapPanel.Children)
            {
                if (euc.UnitName == this.BaseUnitName)
                {
                    nomen.Barcode = euc.Barcode;
                    nomen.BarcodeType = euc.BarcodeType;
                }
            }
            if ((DataBaseRequest.CreateNewNomenclature(nomen) && this.CreateAddUnitsDB()) && (this.CreatePropertiesDB() && this.CreateNomenImages()))
            {
                AfterCreationDelegate(nomen);
                ClearAll();
            }
        }

        // Метод: Добавляет Изображения в Базу данных ----------------------------------------------------------------------------------------
        private bool CreateNomenImages()
        {
            bool flag = false;
            if (AddImagesWrapPanel.Children.Count > 0)
            {
                foreach (NomenImageUC euc in AddImagesWrapPanel.Children)
                {
                    flag = DataBaseRequest.CreateNomenImages(ID, ImageByte.ImageToByteArray(euc.ImagePath), euc.Description, euc.MainImage);
                }
                return flag;
            }
            return true;
        }

        // Метод: Добавляет Свойства и значения в Базу данных --------------------------------------------------------------------------------
        private bool CreatePropertiesDB()
        {
            bool flag = false;
            if (AddPropertiesStackPanel.Children.Count > 0)
            {
                foreach (NomenPropertyUC yuc in AddPropertiesStackPanel.Children)
                {
                    if (yuc.Readiness)
                    {
                        NomenPropertyClass nPC = new NomenPropertyClass
                        {
                            ID = this.ID,
                            PropertyName = (yuc.PropertyComboBox.SelectedItem as ComboBoxItem).Content.ToString(),
                            ValueName = (yuc.ValueComboBox.SelectedItem as ComboBoxItem).Content.ToString()
                        };
                        flag = DataBaseRequest.CreateNomenProperties(nPC);
                    }
                    else
                    {
                        flag = true;
                    }
                }
                return flag;
            }
            return true;
        }

        // Метод: Добавляет Доп. Единицы редактируемой номенклатуры из базы данных -----------------------------------------------------------
        private void LoadAdditionalUnits()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                foreach (AdditionalUnitsClass AUC in DataBaseRequest.GetEditableNomenclatureAddedUnits(EditableNomenclature.ID))
                {
                    AdditionalUnitsUC AUUC = new AdditionalUnitsUC
                    {
                        Readiness = true,
                        BaseUnitWeight = BaseUnitWeight,
                        BaseUnitName = BaseUnitName,
                        ID = AUC.ID,
                        NomenclatureID = AUC.NomenclatureID,
                        AddUnitName = AUC.AddUnitName,
                        Quantity = AUC.Quantity,
                        Weight = AUC.Weight
                    };
                    AUUC.UnitsComboBox.SelectionChanged += new SelectionChangedEventHandler(AdditionalUnitsComboBox_SelectionChanged);
                    AUUC.DeleteButton.Click += new RoutedEventHandler(AdditionalUnitsDeleteButton_Click);

                    if ((AUC.BarcodeType != "") && (AUC.Barcode != ""))
                    {
                        AddBarcode(AUC.AddUnitName, AUC.BarcodeType, AUC.Barcode);
                    }
                    LoadAddUnitsToComboBox(ref AUUC);
                    foreach (ComboBoxItem item in AUUC.UnitsComboBox.Items)
                    {
                        if (item.Content.ToString() == AUC.AddUnitName)
                        {
                            AUUC.UnitsComboBox.SelectedItem = item;
                        }
                    }
                    AddUnitsStackPanel.Children.Add(AUUC);
                    (AddUnitsStackPanel.Parent as ScrollViewer).ScrollToEnd();
                }
            }));
        }

        // Метод: Загружает Единицы измерения в ComboBox элемента "Доп. Един." из базы данных ------------------------------------------------
        private void LoadAddUnitsToComboBox(ref AdditionalUnitsUC AUUC)
        {
            foreach (string str in DataBaseRequest.GetUnits())
            {
                ComboBoxItem newItem = new ComboBoxItem
                {
                    Content = str,
                    Height = 25.0,
                    Padding = new Thickness(10.0, 1.0, 1.0, 1.0),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3B5BA6"))
                };
                AUUC.UnitsComboBox.Items.Add(newItem);
            }
        }

        // Метод: Загружает Типы штрих-кодов в ComboBox из базы данных -----------------------------------------------------------------------
        private void LoadBarcodeTypes()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                ArrayList barcodeTypes = DataBaseRequest.GetBarcodeTypes();
                BarcodeTypesComboBox.Items.Clear();
                foreach (object obj2 in barcodeTypes)
                {
                    ComboBoxItem newItem = new ComboBoxItem
                    {
                        Content = obj2.ToString(),
                        Height = 25.0,
                        Padding = new Thickness(10.0, 1.0, 1.0, 1.0),
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3B5BA6"))
                    };
                    BarcodeTypesComboBox.Items.Add(newItem);
                }
            }));
        }

        // Метод: Загружает Штрих-код Базовой единицы редактируемой номенклатуры из базы данных ----------------------------------------------
        private void LoadBaseBarcode()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                NomenclatureClass baseBarcode = DataBaseRequest.GetBaseBarcode(EditableNomenclature.ID);
                if (baseBarcode.BaseUnit != "" && baseBarcode.BarcodeType != "" && baseBarcode.Barcode != "")
                {
                    AddBarcode(baseBarcode.BaseUnit, baseBarcode.BarcodeType, baseBarcode.Barcode);
                }
            }));
        }

        // Метод: Загружает в CommboBox Страны из базы данных --------------------------------------------------------------------------------
        private void LoadCountry()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                ArrayList country = DataBaseRequest.GetCountry();
                ComboBoxCountry.Items.Clear();
                foreach (object obj2 in country)
                {
                    ComboBoxItem newItem = new ComboBoxItem
                    {
                        Height = 25.0,
                        Foreground = GetColors.Get("DarkBlue-001"),
                        Padding = new Thickness(10.0, 0.0, 0.0, 0.0)
                    };
                    newItem.Content = obj2.ToString();
                    ComboBoxCountry.Items.Add(newItem);
                }
            }));
        }

        // Метод: Загружает Свойства в ComboBox из базы данных -------------------------------------------------------------------------------
        private void LoadPropertiesInComboBox(ref ComboBox CBX)
        {
            ArrayList allNomenProperties = DataBaseRequest.GetAllNomenProperties();
            CBX.Items.Clear();
            foreach (object obj2 in allNomenProperties)
            {
                ComboBoxItem newItem = new ComboBoxItem
                {
                    Content = obj2.ToString(),
                    Height = 25.0,
                    Padding = new Thickness(10.0, 1.0, 1.0, 1.0),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3B5BA6"))
                };
                CBX.Items.Add(newItem);
            }
        }

        // Метод: Загружает Свойства и значения редактируемой номенклатуры из базы данных ----------------------------------------------------
        private void LoadPropertiesValues()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                foreach (NomenPropertyClass NPC in DataBaseRequest.GetEditableNomenclaturePropertiesAndValues(EditableNomenclature.ID))
                {
                    AddPropertiesValues(NPC.PropertyName, NPC.ValueName);
                }
            }));
        }

        // Метод: Загружает Единицы измерения в ComboBox Базовой единицы из базы данных ------------------------------------------------------
        private void LoadUnits()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                ArrayList units = DataBaseRequest.GetUnits();
                BaseUnitComboBox.Items.Clear();
                foreach (object obj2 in units)
                {
                    ComboBoxItem newItem = new ComboBoxItem
                    {
                        Height = 25.0,
                        Foreground = GetColors.Get("DarkBlue-001"),
                        Padding = new Thickness(10.0, 0.0, 0.0, 0.0)
                    };
                    newItem.Content = obj2.ToString();
                    BaseUnitComboBox.Items.Add(newItem);
                }
            }));
        }

        // Метод: Загружает Единицы измерения в ComboBox для создания Штрих-кода из базы данных ----------------------------------------------
        private void LoadUnitsForBacode()
        {
            BarcodeUnitsComboBox.Items.Clear();
            ArrayList list = new ArrayList();
            list.Add(BaseUnitName);
            foreach (AdditionalUnitsUC suc in AddUnitsStackPanel.Children)
            {
                if (((suc.AddUnitName != "") && (suc.AddUnitName != null)) && (suc.AddUnitName != string.Empty))
                {
                    list.Add(suc.AddUnitName);
                }
            }
            foreach (object obj2 in list)
            {
                ComboBoxItem newItem = new ComboBoxItem
                {
                    Content = obj2.ToString(),
                    Height = 25.0,
                    Padding = new Thickness(10.0, 1.0, 1.0, 1.0),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3B5BA6"))
                };
                BarcodeUnitsComboBox.Items.Add(newItem);
            }
        }

        // Метод: Загружает Значения в ComboBox элемента "Свойства и значения" из базы данных ------------------------------------------------
        private void LoadValuesInComboBox(ref ComboBox CBX, string Property)
        {
            ArrayList allNomenPropertyValues = DataBaseRequest.GetAllNomenPropertyValues(Property);
            CBX.Items.Clear();
            foreach (object obj2 in allNomenPropertyValues)
            {
                ComboBoxItem newItem = new ComboBoxItem
                {
                    Content = obj2.ToString(),
                    Height = 25.0,
                    Padding = new Thickness(10.0, 1.0, 1.0, 1.0),
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3B5BA6"))
                };
                CBX.Items.Add(newItem);
            }
        }

        // Метод: Проверяет "Готовность"(То-есть заполненность значений) для создания штрих-кода ---------------------------------------------
        private bool ReadyForCreateBarcode()
        {
            bool flag = false;
            if (BarcodeUnitsComboBox.SelectedItem != null && BarcodeTypesComboBox.SelectedItem != null && BarcodeTextBox.Text != "")
            {
                 flag = true;
            }
            return flag;
        }

        // Метод: Удаляет элемент Доп. единицы из списка, по названию ------------------------------------------------------------------------
        private void RemoveAddedUnit(string UnitName)
        {
            foreach (AdditionalUnitsUC suc in AddUnitsStackPanel.Children)
            {
                if (suc.AddUnitName == UnitName)
                {
                    AddUnitsStackPanel.Children.Remove(suc);
                    break;
                }
            }
        }

        // Метод: Выберает базовую единицу из элеметов ComboBox ------------------------------------------------------------------------------
        private void SelectBaseUnitItem()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                if (!CreateMode)
                {
                    foreach (ComboBoxItem item in BaseUnitComboBox.Items)
                    {
                        if (item.Content.ToString() == BaseUnitName)
                        {
                            BaseUnitComboBox.SelectedItem = item;
                        }
                    }
                }
            }));
        }

        // Метод: Выберает страну из элеметов ComboBox ---------------------------------------------------------------------------------------
        private void SelectCountryItem()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                if (!CreateMode)
                {
                    foreach (ComboBoxItem item in ComboBoxCountry.Items)
                    {
                        if (item.Content.ToString() == Country)
                        {
                            ComboBoxCountry.SelectedItem = item;
                        }
                    }
                }
            }));
        }

        // Метод: Переключает страницы -------------------------------------------------------------------------------------------------------
        private void ShowPage(string RadioButtonName)
        {
            string str = RadioButtonName;
            if (!(str == "MainRB"))
            {
                if (str == "AddUnitsRB")
                {
                    foreach (Grid grid2 in WorkGrid.Children)
                    {
                        if (grid2.Name == "AddUnitsGrid")
                        {
                            grid2.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            grid2.Visibility = Visibility.Hidden;
                        }
                    }
                }
                else if (str == "PropertiesRB")
                {
                    foreach (Grid grid3 in WorkGrid.Children)
                    {
                        if (grid3.Name == "PropertiesGrid")
                        {
                            grid3.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            grid3.Visibility = Visibility.Hidden;
                        }
                    }
                }
                else if (str == "BarcodeRB")
                {
                    foreach (Grid grid4 in WorkGrid.Children)
                    {
                        if (grid4.Name == "BarcodesGrid")
                        {
                            grid4.Visibility = Visibility.Visible;
                            LoadUnitsForBacode();
                            LoadBarcodeTypes();
                        }
                        else
                        {
                            grid4.Visibility = Visibility.Hidden;
                        }
                    }
                }
                else if (str == "ImagesRB")
                {
                    foreach (Grid grid5 in WorkGrid.Children)
                    {
                        if (grid5.Name == "ImagesGrid")
                        {
                            grid5.Visibility = Visibility.Visible;
                            LoadUnitsForBacode();
                            LoadBarcodeTypes();
                        }
                        else
                        {
                            grid5.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
            else
            {
                foreach (Grid grid in WorkGrid.Children)
                {
                    if (grid.Name == "MainGrid")
                    {
                        grid.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        grid.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        // Метод: Загружает изображения редактируемой номенклатуры из базы данных ------------------------------------------------------------
        private void LoadNomenclatureImages()
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                foreach (NomenclatureImageClass NIC in DataBaseRequest.GetNomenclatureImages(EditableNomenclature.ID))
                {
                    AddNomenclatureImages(NIC);
                }
            }));
        }

        // Метод: добавляет элемент "Изображение" в "AddImagesWrapPanel" ---------------------------------------------------------------------
        private void AddNomenclatureImages(NomenclatureImageClass NIC)
        {
            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                NomenImageUC NIUC = new NomenImageUC();
                NIUC.NomenImage = ImageByte.ByteArrayToImage(NIC.ImageData);
                NIUC.Description = NIC.Description;
                NIUC.MainImage = NIC.MainImage;
                NIUC.Width = 294.5;
                NIUC.Height = 200.0;
                AddImagesWrapPanel.Children.Add(NIUC);
            }));
        }

        #endregion :::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

    }
}
