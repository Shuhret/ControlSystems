using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ControlSystemsLibrary.Classes
{
    public class NomenclatureClass : INotifyPropertyChanged
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
        



        public NomenclatureClass()
        {
            Article = "";
            CountryOfOrigin = "";
            Barcode = "";
            BarcodeType = "";
            Description = "";
            Nomenclatures = new BindingList<NomenclatureClass>();
        }

        // ID Номенклатуры
        public Guid ID { get; set; }

        // ID Группы
        private Guid groupID;
        public Guid GroupID
        {
            get => groupID;
            set
            {
                groupID = value;
                OnPropertyChanged();
            }
        }

        // Группа или Номенклатура
        private bool groupNomen;
        public bool GroupNomen
        {
            get  => groupNomen;
            set
            {
                groupNomen = value;
                if (value == true)
                {
                    TagVisibility = Visibility.Visible;
                    Icon = (Viewbox)Icons.GetIcon("Номенклатура");
                }
                else
                {
                    TagVisibility = Visibility.Collapsed;
                    Icon = (Viewbox)Icons.GetIcon("Папка");
                }
            }
        }

        // Иконка Группа или Номенклатура
        public Viewbox Icon { get; set; }

        

        // Выбрано Номенклатура
        private bool? selected = false;
        public bool? Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged();
            }
        }


        // Артикул Номенклатуры
        public string Article { get; set; }

        // Название Номенклатуры
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }


        // Название Базовой Единицы Измерения
        public string BaseUnit { get; set; }

        // Вес Базовой Единицы 
        private double weightBaseUnit;
        public double WeightBaseUnit
        {
            get => weightBaseUnit;
            set
            {
                weightBaseUnit = value;
                if (value > 0)
                {
                    if(value < 1)
                        WeightBaseUnitShow = (weightBaseUnit * 1000).ToString() + " гр";
                    else
                        WeightBaseUnitShow = value.ToString() + " кг";
                }
            }
        }

        // Вес Базовой Единицы для показа в DataGrid
        public string WeightBaseUnitShow { get; set; }

        // Штрих-код
        public string Barcode { get; set; }

        // Тип Штрих-кода
        public string BarcodeType { get; set; }

        // Страна Происхождения
        public string CountryOfOrigin { get; set; }

        // Описание
        public string Description { get; set; }

        // Метка: "Акция"
        private bool aksia;
        public bool Aksia
        {
            get => aksia;
            set
            {
                aksia = value;
                //DataBaseRequest.UpdateTagAksia(this);
                OnPropertyChanged();
            }
        }

        // Метка: "Фокус"
        private bool focus;
        public bool Focus
        {
            get => focus;
            set
            {
                focus = value;
                //DataBaseRequest.UpdateTagFocus(this);
                OnPropertyChanged();
            }
        }

        // Метка: "Новинка"
        private bool _new;
        public bool New
        {
            get => _new;
            set
            {
                _new = value;
                //DataBaseRequest.UpdateTagNew(this);
                OnPropertyChanged();
            }
        }

        // Видимость меток
        public Visibility TagVisibility { get; set; }

        public BindingList<NomenclatureClass> Nomenclatures { get; set; }


        // Вырезано
        private bool cutOut = false;
        public bool CutOut
        {
            get => cutOut;
            set
            {
                cutOut = value;
                if (value == true)
                    CutIconVisibility = Visibility.Visible;
                else
                    CutIconVisibility = Visibility.Collapsed;
                OnPropertyChanged();
            }
        }

        // Основная вырезанная группа
        private bool cutOutParent = false;
        public bool CutOutParent
        {
            get => cutOutParent;
            set
            {
                cutOutParent = value;
                OnPropertyChanged();
            }
        }


        private byte[] imageData;
        public byte[] ImageData
        {
            get => imageData;
            set
            {
                imageData = value;
                if(imageData.Length > 0)
                {
                    MainImage = ImageByte.ByteArrayToImage(imageData);
                }

                OnPropertyChanged();
            }
        }




        private BitmapImage mainImage;

        public BitmapImage MainImage
        {
            get => mainImage;
            set
            {
                mainImage = value;
                OnPropertyChanged();
            }
        }


        private Visibility cutIconVisibility = Visibility.Collapsed;
        public Visibility CutIconVisibility
        {
            get => cutIconVisibility;
            set
            {
                cutIconVisibility = value;
                OnPropertyChanged();
            }
        }


    }
}
