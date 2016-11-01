using System.Threading.Tasks;

namespace Slalom.Boost.VisualStudio
{
    public interface IClickTrackService
    {
        Task Track(string userName, string name, string projectName, object additional);
    }
}