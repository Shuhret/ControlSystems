using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ControlSystemsLibrary.Classes
{
    class AdditionalUnitsClass : INotifyPropertyChanged
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

        public AdditionalUnitsClass()
        {
            BaseUnitName = "";
            AddUnitName = "";
            BarcodeType = "";
            Barcode = "";
        }

        public Guid ID { get; set; }

        public Guid NomenclatureID { get; set; }
        
        public string BaseUnitName { get; set; }

        public string AddUnitName { get; set; }
        
        public double Quantity { get; set; }

        public double Weight { get; set; }

        public string BarcodeType { get; set; }

        public string Barcode { get; set; }

    }
}
