using System;
using System.Windows;
using System.Windows.Controls;

namespace ShuhratControlLibrary.Controls
{
    [TemplatePart(Name = "PART_VedroButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_YesDeleteButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NoDeleteButton", Type = typeof(Button))]
    public partial class ConnectionRB : RadioButton
    {
        public event EventHandler Deleted;
        public ConnectionRB()
        {
            InitializeComponent();
            DeleteGridVisibility = Visibility.Collapsed;
        }


        public Visibility DeleteGridVisibility
        {
            get { return (Visibility)GetValue(DeleteGridVisibilityProperty); }
            set { SetValue(DeleteGridVisibilityProperty, value); }
        }
        public static readonly DependencyProperty DeleteGridVisibilityProperty = DependencyProperty.Register("DeleteGridVisibility", typeof(Visibility), typeof(ConnectionRB), new PropertyMetadata(Visibility.Collapsed));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var VedroButton = this.GetTemplateChild("PART_VedroButton") as Button;
            if (VedroButton != null)
            {
                VedroButton.Click += VedroButton_Click;
            }

            var NoDeleteButton = this.GetTemplateChild("PART_NoDeleteButton") as Button;
            if (NoDeleteButton != null)
            {
                NoDeleteButton.Click += NoDeleteButton_Click;
            }

            var YesDeleteButton = this.GetTemplateChild("PART_YesDeleteButton") as Button;
            if (YesDeleteButton != null)
            {
                YesDeleteButton.Click += YesDeleteButton_Click;
            }
        }

        private void YesDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Deleted != null)
                Deleted(this, e);

            //ConnectionRB CRB = ((Button)sender).TemplatedParent as ConnectionRB;
            //bool isCheck = (bool)CRB.IsChecked;
            //StackPanel sp = CRB.Parent as StackPanel;
            //sp.Children.Remove(CRB);

            //if (isCheck)
            //    SelectLostElement(sp);
        }

        private void NoDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteGridVisibility = Visibility.Collapsed;
        }

        private void VedroButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteGridVisibility = Visibility.Visible;

            ConnectionRB CRB = ((Button)sender).TemplatedParent as ConnectionRB;
            foreach (ConnectionRB crb in ((StackPanel)CRB.Parent).Children)
            {
                if (crb != CRB)
                    crb.DeleteGridVisibility = Visibility.Collapsed;
            }
        }

    }
}
