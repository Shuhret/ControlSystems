using System;
using System.Collections;
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

namespace ShuhratControlLibrary.Controls
{

    [TemplatePart(Name = "PART_ButtDelete", Type = typeof(Button))]
    public partial class DopEdinUC : UserControl
    {
        
        public DopEdinUC(string BaseUnitName, double BaseUnitWeight)
        {
            BaseEdin = BaseUnitName;
            this.BaseUnitWeight = BaseUnitWeight;
            InitializeComponent();
        }

        public string SelectedDopEdin
        {
            get { return (string)GetValue(SelectedDopEdinProperty); }
            set { SetValue(SelectedDopEdinProperty, value); }
        }
        public static readonly DependencyProperty SelectedDopEdinProperty = DependencyProperty.Register("SelectedDopEdin", typeof(string), typeof(DopEdinUC), new PropertyMetadata(string.Empty));

        public string BaseEdin
        {
            get { return (string)GetValue(BaseEdinProperty); }
            set { SetValue(BaseEdinProperty, value); }
        }
        public static readonly DependencyProperty BaseEdinProperty = DependencyProperty.Register("BaseEdin", typeof(string), typeof(DopEdinUC), new PropertyMetadata(string.Empty));



        public double BaseUnitWeight
        {
            get { return (double)GetValue(BaseUnitWeightProperty); }
            set { SetValue(BaseUnitWeightProperty, value); }
        }

        public static readonly DependencyProperty BaseUnitWeightProperty = DependencyProperty.Register("BaseUnitWeight", typeof(double), typeof(DopEdinUC), new PropertyMetadata((double)0));




        public Visibility WarningVisibility
        {
            get { return (Visibility)GetValue(WarningVisibilityProperty); }
            set { SetValue(WarningVisibilityProperty, value); }
        }
        public static readonly DependencyProperty WarningVisibilityProperty = DependencyProperty.Register("WarningVisibility", typeof(Visibility), typeof(DopEdinUC), new PropertyMetadata(Visibility.Visible));

        public Visibility OkVisibility
        {
            get { return (Visibility)GetValue(OkVisibilityProperty); }
            set { SetValue(OkVisibilityProperty, value); }
        }
        public static readonly DependencyProperty OkVisibilityProperty = DependencyProperty.Register("OkVisibility", typeof(Visibility), typeof(DopEdinUC), new PropertyMetadata(Visibility.Hidden));

        private void txbxDopEdinKol_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            double result;
            string str = e.Text;
            bool дробь = false;

            foreach (char ch in ((TextBox)sender).Text)
            {
                if (ch == ',')
                {
                    дробь = true;
                    break;
                }
            }

            if (((TextBox)sender).Text.Length == 0 && (str == ",") || (str == "."))
            {
                ((TextBox)sender).Text = "0,";

                ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
                ((TextBox)sender).SelectionLength = 0;

                дробь = true;
                e.Handled = true;
            }
            else if (str == "," && !дробь)
            {
                ((TextBox)sender).Text += ",";
                str = ",";
                дробь = true;
                ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
                ((TextBox)sender).SelectionLength = 0;

                e.Handled = true;
            }

            if ((str == "." || str == ",") && дробь)
            {
                e.Handled = true;
            }

            if ((!(double.TryParse(str, out result) || str == ".")))
            {
                e.Handled = true;
            }

        }

        private void DopEdinComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckValuesForWarning();
            ComboBox thisCBX = sender as ComboBox;
            bool ok = true;

            if (Parent is StackPanel && thisCBX.SelectedItem != null)
            {
                foreach (var v in ((StackPanel)Parent).Children)
                {
                    if (v is DopEdinUC)
                    {
                        DopEdinUC DE = v as DopEdinUC;
                        if (DE != this && (ComboBoxItem)this.DopEdinComboBox.SelectedItem != null && (ComboBoxItem)DE.DopEdinComboBox.SelectedItem != null)
                        {
                            if (((ComboBoxItem)DE.DopEdinComboBox.SelectedItem).Content.ToString() == ((ComboBoxItem)this.DopEdinComboBox.SelectedItem).Content.ToString())
                            {
                                MessageBox.Show("Единица: " + '"' + ((ComboBoxItem)DE.DopEdinComboBox.SelectedItem).Content.ToString() + '"' + " имеется в списке.", "Предупреждение");
                                ok = false;
                                break;
                            }
                        }
                        if (BaseEdin == ((ComboBoxItem)this.DopEdinComboBox.SelectedItem).Content.ToString() && (ComboBoxItem)this.DopEdinComboBox.SelectedItem != null && (ComboBoxItem)DE.DopEdinComboBox.SelectedItem != null)
                        {
                            MessageBox.Show("Единица: " + '"' + ((ComboBoxItem)this.DopEdinComboBox.SelectedItem).Content.ToString() + '"' + " является базовой единицой измерения.", "Предупреждение");
                            ok = false;
                            break;
                        }
                    }
                }
                if (!ok)
                {
                    thisCBX.SelectedItem = null;
                    thisCBX.IsDropDownOpen = true;
                }
                if (DopEdinComboBox.SelectedItem != null)
                    SelectedDopEdin = ((ComboBoxItem)DopEdinComboBox.SelectedItem).Content.ToString();
                else
                    SelectedDopEdin = "";
            }
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckValuesForWarning();
            if(sender is TextBox)
            {
                TextBox txbx = sender as TextBox;
                if(txbx.Name == "txbxDopEdinKol")
                    txbxDopEdinVes.Text = (Convert.ToDouble( txbxDopEdinKol.Text) * BaseUnitWeight).ToString();
            }
        }

        private void ClickDopEdinDeleteButton(object sender, RoutedEventArgs e)
        {
            ((StackPanel)Parent).Children.Remove(this);
        }

        private void CheckValuesForWarning()
        {
            if (DopEdinComboBox.SelectedItem != null && txbxDopEdinKol.Text != "" && txbxDopEdinVes.Text != "")
            {
                WarningVisibility = Visibility.Hidden;
                OkVisibility = Visibility.Visible;
            }
            else
            {
                WarningVisibility = Visibility.Visible;
                OkVisibility = Visibility.Hidden;
            }
        }
    }
}

