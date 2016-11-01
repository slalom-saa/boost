using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using EnvDTE;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.Events;
using Slalom.Boost.VisualStudio.IDE;
using Application = Slalom.Boost.VisualStudio.Application;

namespace Slalom.Boost.Learn
{

    /// <summary>
    /// Interaction logic for GetStarted.xaml
    /// </summary>
    public partial class HomeControl : UserControl
    {
        public HomeControl()
        {
            InitializeComponent();
        }

        private void HandleNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            DomainEvents.Raise(new NavigateRequested(e.Uri));

            this.UpdateLayout();
        }

        private void HandleCommandRequest(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            DomainEvents.Raise(new CommandRequested(e.Uri));

            this.UpdateLayout();
        }
    }
}
