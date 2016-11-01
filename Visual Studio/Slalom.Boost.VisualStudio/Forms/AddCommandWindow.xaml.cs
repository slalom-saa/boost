using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using EnvDTE;
using Window = System.Windows.Window;

namespace Slalom.Boost.VisualStudio.Forms
{
    public partial class AddCommandWindow : Window
    {
        private const int GWL_STYLE = -16;

        private const uint WS_SYSMENU = 0x80000;

        public static readonly DependencyProperty PropertyTypesProperty =
            DependencyProperty.Register("PropertyTypes", typeof(List<Type>), typeof(AddCommandWindow));

        // Using a DependencyProperty as the backing store for ReturnTypes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReturnTypesProperty =
            DependencyProperty.Register("ReturnTypes", typeof(List<string>), typeof(AddCommandWindow));

        public AddCommandWindow(string title, IEnumerable<CodeProperty> properties)
        {
            this.InitializeComponent();

            this.Title = title;

            PropertyItems.ItemsSource = this.Properties;
            foreach (var codeProperty in properties)
            {
                this.Properties.Add(codeProperty);
            }


            this.PropertyTypes = new List<Type>
            {
                typeof(string),
                typeof(int),
                typeof(Guid),
                typeof(double),
                typeof(object)
            };
            cbPropertyType.SelectedValue = typeof(string);


            txtName.Focus();
            txtName.SelectAll();
        }

        public string EventName => txtEvent.Text;

        public string ItemName => txtName.Text;

        public bool NameChanged { get; set; }

        public ObservableCollection<CodeProperty> Properties { get; set; } = new ObservableCollection<CodeProperty>();

        public IEnumerable<CodeProperty> GetProperties()
        {
            return Properties.ToArray();
        }

        public List<Type> PropertyTypes
        {
            get { return (List<Type>)this.GetValue(PropertyTypesProperty); }
            set { this.SetValue(PropertyTypesProperty, value); }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE,
                GetWindowLong(hwnd, GWL_STYLE) & (0xFFFFFFFF ^ WS_SYSMENU));

            base.OnSourceInitialized(e);
        }

        [DllImport("user32.dll")]
        static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

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

        private void HandleNameKeyDown(object sender, KeyEventArgs e)
        {
            this.NameChanged = true;
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
                this.Properties.Remove(PropertyItems.SelectedValue as CodeProperty);
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

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
    }
}