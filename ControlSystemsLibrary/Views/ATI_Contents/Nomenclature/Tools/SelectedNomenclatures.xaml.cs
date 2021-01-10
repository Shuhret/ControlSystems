using ControlSystemsLibrary.Classes;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ControlSystemsLibrary.Views.Tools
{

    public partial class SelectedNomenclatures : UserControl
    {

        BindingList<NomenclatureClass> AllSelectedElements = new BindingList<NomenclatureClass>();
        BindingList<NomenclatureClass> AllSelectedNomenclatures = new BindingList<NomenclatureClass>();




        public SelectedNomenclatures(ref BindingList<NomenclatureClass> AllSelectedElements)
        {
            this.AllSelectedElements = AllSelectedElements;
            InitializeComponent();
            SelectedNomenclaturesDataGrid.ItemsSource = AllSelectedNomenclatures;
            FillingAllSelectedNomenclaturesList();
        }


        private void FillingAllSelectedNomenclaturesList()
        {
            AllSelectedNomenclatures.Clear();
            foreach (NomenclatureClass NC in AllSelectedElements)
            {
                if(NC.GroupNomen == true)
                {
                    AllSelectedNomenclatures.Add(NC);
                }
            }
        }

    }
}
