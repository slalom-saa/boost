using System;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Slalom.Boost.VisualStudio.Forms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CreateSolutionWindow : Window
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

        public CreateSolutionWindow()
        {
            this.InitializeComponent();

            txtLocation.Text = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "Source");

            txtModule.GotFocus += (a, b) =>
            {
                txtModule.SelectAll();
            };

            txtProduct.GotFocus += (a, b) =>
            {
                txtProduct.SelectAll();
            };

            txtProject.GotFocus += (a, b) =>
            {
                txtProject.SelectAll();
            };

            txtProduct.Focus();


            this.UpdateModuleName();
        }

        public string SolutionPath =>
            Path.Combine(txtLocation.Text, $"{txtProduct.Text}.{txtProject.Text}.{txtModule.Text}", $"{txtProduct.Text}.{txtProject.Text}.{txtModule.Text}.sln");

        private void HandleBrowse(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                this.Topmost = false;
                dialog.RestoreDirectory = true;
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    txtLocation.Text = dialog.FileName;
                    this.UpdateModuleName();
                }
                this.Topmost = true;
                this.Activate();
            }
        }

        private void HandleCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HandleOk(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void UpdateModuleName()
        {
            if (Directory.Exists(Path.Combine(txtLocation.Text, txtProduct.Text + "." + txtProject.Text + "." + txtModule.Text)))
            {
                var i = 1;
                while (Directory.Exists(Path.Combine(txtLocation.Text, txtProduct.Text + "." + txtProject.Text + "." + txtModule.Text + i)))
                {
                    i++;
                }
                txtModule.Text = "Module" + i;
            }
        }
    }
}