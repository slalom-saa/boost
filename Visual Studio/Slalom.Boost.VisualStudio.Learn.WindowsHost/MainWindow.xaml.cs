using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.Forms;

namespace Slalom.Boost.Learn.WindowsHost
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            var window = new AddTemplateWindow("Add Value Object", "Member", "Employee");
            window.ShowDialog();
            this.Close();
        }
    }
}