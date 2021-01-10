using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ControlSystemsLibrary.Classes
{
    class NomenclatureImageClass : INotifyPropertyChanged
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

        // ID 
        public Guid ID { get; set; }

        // ID Номенклатуры
        public Guid NomenclatureID { get; set; }

        private byte[] imageData;
        public byte[] ImageData
        {
            get => imageData;
            set
            {
                imageData = value;
                OnPropertyChanged();
            }
        }

        private string description = "";
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        public bool mainImage;
        public bool MainImage
        {
            get => mainImage;
            set
            {
                mainImage = value;
                OnPropertyChanged();
            }
        }

    }
}
