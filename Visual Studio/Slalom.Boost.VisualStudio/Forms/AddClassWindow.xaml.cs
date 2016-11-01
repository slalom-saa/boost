using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Slalom.Boost.VisualStudio.Forms
{
    public partial class AddClassWindow : Window
    {
        [DllImport("user32.dll")]
        static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        private const int GWL_STYLE = -16;

        private const uint WS_SYSMENU = 0x80000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE,
                GetWindowLong(hwnd, GWL_STYLE) & (0xFFFFFFFF ^ WS_SYSMENU));

            base.OnSourceInitialized(e);
        }

        public bool TemplateSelected => SelectionTabControl.SelectedIndex == 1;

        public string SelectedTemplate => this.TemplateSelected ? TemplatesListBox.SelectedItem as String : null;

        public static readonly DependencyProperty PropertyTypesProperty =
            DependencyProperty.Register("PropertyTypes", typeof(List<Type>), typeof(AddClassWindow));

        public AddClassWindow(string title, params string[] templates)
        {
            this.InitializeComponent();

            this.Title = title;

            PropertyItems.ItemsSource = this.Properties;
            this.PropertyTypes = new List<Type>
            {
                typeof(string),
                typeof(int),
                typeof(Guid),
                typeof(double),
                typeof(object)
            };
            cbPropertyType.SelectedValue = typeof(string);
            TemplatesListBox.ItemsSource = templates;

            txtName.Focus();
            txtName.SelectAll();
            
           
        }

        public string ItemName => txtName.Text;

        public ObservableCollection<BoostCodeProperty> Properties { get; set; } = new ObservableCollection<BoostCodeProperty>();

        public List<Type> PropertyTypes
        {
            get { return (List<Type>)this.GetValue(PropertyTypesProperty); }
            set { this.SetValue(PropertyTypesProperty, value); }
        }

        private void HandleAddClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPropertyName.Text))
            {
                this.Properties.Add(new BoostCodeProperty(txtPropertyName.Text, cbPropertyType.SelectedValue as Type));
                txtPropertyName.Text = "";
            }
        }

        private void HandleCancelClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void HandleOkClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void HandlePropertyKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                this.Properties.Remove(PropertyItems.SelectedValue as BoostCodeProperty);
            }
        }

        private void HandlePropertyNamePreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                if (string.IsNullOrWhiteSpace(txtPropertyName.Text))
                {
                    this.HandleOkClicked(this, e);
                }
                else
                {
                    this.HandleAddClick(this, e);
                }
            }
        }

        private void HandleNameKeyDown(object sender, KeyEventArgs e)
        {
            this.NameChanged = true;
        }

        public bool NameChanged { get; set; }

        private void HandleTemplateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!this.NameChanged)
            {
                this.txtName.Text = this.TemplatesListBox.SelectedItem as string;
            }
        }
    }
}