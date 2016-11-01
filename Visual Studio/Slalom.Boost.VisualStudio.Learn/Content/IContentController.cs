using System.Collections.Generic;

namespace Slalom.Boost.Learn.Content
{
    public interface IContentController
    {
        string GetContentForToken(string name);
        IEnumerable<string> GetContentTokens();
    }
}