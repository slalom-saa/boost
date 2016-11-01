using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Slalom.Boost.Learn.Content;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.Events;
using Application = Slalom.Boost.VisualStudio.Application;

namespace Slalom.Boost.Learn
{
    /// <summary>
    /// Interaction logic for Reference.xaml
    /// </summary>
    public partial class ReferenceControl : UserControl
    {
        private readonly IContentController _controller;
        private readonly IClickTrackService _service;

        private ScrollViewer scrollViewer;
        private string _previous;

        public ReferenceControl() : this(new ContentController(), new ClickTrackService())
        {
        }

        public ReferenceControl(IContentController controller, IClickTrackService service)
        {
            InitializeComponent();

            _controller = controller;
            _service = service;

            this.LoadContent("Home");

            DomainEvents.Register<TemplateChanged>(e =>
            {
                this.LoadContent(e.Name);
            });
        }

        public ScrollViewer ScrollViewer
        {
            get
            {
                if (scrollViewer == null)
                {
                    DependencyObject obj = Viewer;

                    do
                    {
                        if (VisualTreeHelper.GetChildrenCount(obj) > 0)
                        {
                            obj = VisualTreeHelper.GetChild(obj as Visual, 0);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    while (!(obj is ScrollViewer));

                    scrollViewer = obj as ScrollViewer;
                }

                return scrollViewer;
            }
        }

        public void LoadContent(string content)
        {
            if (_previous != content)
            {
                _previous = content;
                if (!content.Contains("<"))
                {
                    content = _controller.GetContentForToken(content);
                }
                var rootObject = XamlReader.Parse(content) as FlowDocument;
                Viewer.Document = rootObject;

                var hyperlinks = GetVisuals(rootObject).OfType<Hyperlink>();
                foreach (var link in hyperlinks)
                {
                    link.RequestNavigate += this.HandleLinkNavigate;
                }

                this.ScrollViewer?.ScrollToHome();
            }
        }

        private static IEnumerable<DependencyObject> GetVisuals(DependencyObject root)
        {
            foreach (var child in LogicalTreeHelper.GetChildren(root).OfType<DependencyObject>())
            {
                yield return child;
                foreach (var descendants in GetVisuals(child))
                {
                    yield return descendants;
                }
            }
        }

        private void HandleLinkNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (_previous != e.Uri.ToString())
            {
                if (e.Uri.IsAbsoluteUri)
                {
                    Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                    e.Handled = true;
                }
                else if (e.Uri.ToString().Contains("."))
                {
                    DomainEvents.Raise(new TemplateChanged(e.Uri.ToString().Split('.')[1]));
                }
                else
                {
                    this.LoadContent(e.Uri.ToString());
                    e.Handled = true;
                }
                _service.Track(WindowsIdentity.GetCurrent()?.Name, e.Uri.ToString(), GetProjectName(), null);
            }
        }

        private static string GetProjectName()
        {
            return Application.Current.ActiveProject != null ? Application.Current.ActiveProject?.DTE.Solution.GetName() + "\\" + Application.Current.ActiveProject?.Name : null;
        }

        public void OnTokenClicked(string name)
        {
            if (_previous != name)
            {
                this.LoadContent(name);
                _service.Track(WindowsIdentity.GetCurrent()?.Name, name, GetProjectName(), new
                {
                    Application.Current.ActiveDocument
                });
            }
        }

        public void OnFileNavigated(string key)
        {
            if (_previous != key)
            {
                this.LoadContent(key);
            }
        }
    }
}
