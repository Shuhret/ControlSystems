using System;
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
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Bd", Type = typeof(Border))]
    public partial class AdminTabItem : TabItem
    {

        public AdminTabItem()
        {
            InitializeComponent();

            Loaded += AdminTabItem_Loaded;
        }



        #region Свойства
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }
        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string), typeof(AdminTabItem), new PropertyMetadata(string.Empty));



        public string State
        {
            get
            {
                return (string)GetValue(StateProperty);
            }
            set
            {
                SetValue(StateProperty, value);
                MessageBox.Show(value.GetType().ToString());
            }
        }

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(string), typeof(AdminTabItem), new PropertyMetadata(string.Empty));




        public SolidColorBrush StateColor
        {
            get { return (SolidColorBrush)GetValue(StateColorProperty); }
            set { SetValue(StateColorProperty, value); }
        }
        public static readonly DependencyProperty StateColorProperty = DependencyProperty.Register("StateColor", typeof(SolidColorBrush), typeof(AdminTabItem), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));





        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(AdminTabItem), new PropertyMetadata(null));

        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register("IconSize", typeof(double), typeof(AdminTabItem), new PropertyMetadata((double)20));

        #endregion


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var button = this.GetTemplateChild("PART_CloseButton") as Button; // Найти в шаблоне объект с названием "PART_CloseButton" и привести в тип Button
            if (button != null)
            {
                button.Click += ClickCloseButton;
            }


            var bd = this.GetTemplateChild("PART_Bd") as Border; // Найти в шаблоне объект с названием "PART_Bd" и привести в тип Border
            if (bd != null)
            {
                bd.MouseRightButtonUp += Bd_MouseRightButtonUp;
            }

        }



        private void Bd_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Border bd = sender as Border;
            AdminTabItem ti = bd.TemplatedParent as AdminTabItem;

            TabControl tc = ti.Parent as TabControl;
            tc.Items.Remove(ti);
            ВыровнитьШиринуВкладок();

        }

        private void ClickCloseButton(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            AdminTabItem ti = btn.TemplatedParent as AdminTabItem;
            TabControl tc = ti.Parent as TabControl;
            tc.Items.Remove(ti);
            ВыровнитьШиринуВкладок();
        }


        TabControl TC;
        private void AdminTabItem_Loaded(object sender, RoutedEventArgs e)
        {
            TC = Parent as TabControl;
            TC.SelectedItem = this;
            TC.SizeChanged += TC_SizeChanged;
            ВыровнитьШиринуВкладок();
        }

        private void TC_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ВыровнитьШиринуВкладок();
        }

        void ВыровнитьШиринуВкладок()
        {
            try
            {
                if (TC.Items.Count == 0)
                    TC.BorderThickness = new Thickness(0, 0, 0, 0);


                double ШиринаВкладки = 0;


                if (TC.Items.Count == 1)
                {
                    ШиринаВкладки = TC.ActualWidth;
                    TC.BorderThickness = new Thickness(0, 1, 0, 0);

                }
                else
                {
                    ШиринаВкладки = TC.ActualWidth / TC.Items.Count;
                }
                ШиринаВкладки = Math.Round(ШиринаВкладки);

                int count = 0;
                foreach (TabItem ti in TC.Items)
                {
                    if (count != TC.Items.Count - 1)
                    {
                        ti.Width = ШиринаВкладки;
                        ti.BorderThickness = new Thickness(0, 0, 1, 0);
                    }
                    else
                    {
                        ti.Width = (TC.ActualWidth - (ШиринаВкладки * count));
                        ti.BorderThickness = new Thickness(0, 0, 0, 0);
                    }
                    count++;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Вы открыли дохуя окон!");
            }
        }




    }
}
