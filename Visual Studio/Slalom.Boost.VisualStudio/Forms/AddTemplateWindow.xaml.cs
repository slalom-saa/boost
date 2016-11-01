using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace Slalom.Boost.VisualStudio.Forms
{
    public partial class AddTemplateWindow : Window
    {
        private const int GWL_STYLE = -16;

        private const uint WS_SYSMENU = 0x80000;

        public AddTemplateWindow(string title, params string[] templates)
        {
            this.InitializeComponent();

            this.Title = title;

            TemplatesListBox.ItemsSource = templates;
            TemplatesListBox.SelectedIndex = 0;

            txtName.Focus();
            txtName.SelectAll();
        }

        public string SelectedTemplate => TemplatesListBox.SelectedItem as string;

        public string ItemName => txtName.Text;

        public bool Modified { get; set; }

        [DllImport("user32.dll")]
        private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        protected override void OnSourceInitialized(EventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE,
                GetWindowLong(hwnd, GWL_STYLE) & (0xFFFFFFFF ^ WS_SYSMENU));

            base.OnSourceInitialized(e);
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

        private void HandleNameKeyDown(object sender, KeyEventArgs e)
        {
            this.Modified = true;
        }

        private void HandleTemplateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.Modified)
            {
                txtName.Text = TemplatesListBox.SelectedItem as string;
                txtName.SelectAll();
            }
        }
    }
}