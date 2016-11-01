using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.Events;

namespace Slalom.Boost.Learn
{
    public class BoostLearnWindowViewModel
    {
        public bool IsHomeActive { get; set; }

        public bool IsReferenceActive { get; set; }

        public bool IsLearnActive { get; set; }

        public bool IsInsightTabActive { get; set; }

        public BoostLearnWindowViewModel()
        {
            IsHomeActive = true;
        }
    }


    /// <summary>
    /// Interaction logic for BoostLearnWindowControl.xaml
    /// </summary>
    public partial class BoostLearnWindowControl : UserControl, IComponentConnector
    {
        public BoostLearnWindowControl()
        {
            this.InitializeComponent();

            this.DataContext = new BoostLearnWindowViewModel();

            DomainEvents.Register<NavigateRequested>(e =>
            {
                if (e.Path == "Reference")
                {
                    DomainEvents.Raise(new TemplateChanged("Home"));
                    this.HandleReferenceClicked(this, null);
                }
            });
        }

        public void OnTokenClicked(string name)
        {
            this.HandleReferenceClicked(this, null);
            Reference.OnTokenClicked(name);
        }

        private void HandleHomeClicked(object sender, MouseButtonEventArgs e)
        {
            if (Home.Visibility != Visibility.Visible)
            {
                Home.Visibility = Visibility.Visible;
                Reference.Visibility = Visibility.Hidden;
                Insight.Visibility = Visibility.Hidden;
            }
        }

        private void HandleInsightClicked(object sender, MouseButtonEventArgs e)
        {
            if (Insight.Visibility != Visibility.Visible)
            {
                Insight.Visibility = Visibility.Visible;
                Home.Visibility = Visibility.Hidden;
                Reference.Visibility = Visibility.Hidden;
            }
        }

        public void HandleReferenceClicked(object sender, MouseButtonEventArgs e)
        {
            if (Reference.Visibility != Visibility.Visible)
            {
                Reference.Visibility = Visibility.Visible;
                Home.Visibility = Visibility.Hidden;
                Insight.Visibility = Visibility.Hidden;
            }
        }
    }
}