using System.Collections.Generic;
using System.Threading.Tasks;
using EnvDTE;
using Slalom.Boost.VisualStudio.RuntimeBinding;

namespace Slalom.Boost.VisualStudio
{
    [RuntimeBinding(BindingType.Multiple)]
    public interface IVisualStudioCommand
    {
        int Id { get; }

        SelectedItem SelectedItem { get; }

        List<string> Markers { get; }

        void HandleCallback(ProjectItem projectItem);

        bool ShouldDisplay();

        void HandleMarkerClicked();

        void Execute();
    }
}