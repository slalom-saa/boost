using System;
using Slalom.Boost.Learn.Content.Files;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Slalom.Boost.Learn.Content
{
    public class ContentMarker
    {
        public string Key { get; set; }

        public Regex FileIdentifier { get; set; }

        public string ReferenceText { get; set; }
    }

    public class ContentController : IContentController
    {
        private readonly List<ContentMarker> _contentMarkers = new List<ContentMarker>();

        private void AddMappings(string key)
        {
            _contentMarkers.Add(new ContentMarker
            {
                Key = key,
                FileIdentifier = null,
                ReferenceText = key
            });
        }

        private void AddMappings(string key, string text)
        {
            _contentMarkers.Add(new ContentMarker
            {
                Key = key,
                FileIdentifier = null,
                ReferenceText = text
            });
        }

        private void AddMappings(string key, string text, string fileIdentifier)
        {
            _contentMarkers.Add(new ContentMarker
            {
                Key = key,
                FileIdentifier = new Regex(fileIdentifier),
                ReferenceText = text
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentController"/> class.
        /// </summary>
        /// <seealso boost=""/>
        public ContentController()
        {
            this.AddMappings("Entity", "Entity", @"class\s*\w*\s*:\s*Entity");
            this.AddMappings("Aggregate");
            this.AddMappings("Concept", "Concept", @"class\s*\w*\s*:\s*ConceptAs");
            this.AddMappings("DomainEvent", "Domain Event", @"class\s*\w*\s*:\s*Event");
            this.AddMappings("ReadModel", "Read Model", @"class\s*\w*\s*:\s*IReadModelElement");
            this.AddMappings("Synchronizer", "Read Model Synchronizer");
            this.AddMappings("Command", "Command", @"class\s*\w*\s*:\s*Command");
            this.AddMappings("CommandHandler", "Command Handler", @"class\s*\w*\s*:\s*CommandHandler<");
            this.AddMappings("Repository", "Repository", @"class\s*\w*\s*:\s*Repository<");
        }

        public string GetContentForToken(string name)
        {
            try
            {
                var replacement = _contentMarkers.FirstOrDefault(e => e.ReferenceText == name);
                if (replacement?.Key != null)
                {
                    name = replacement.Key;
                }
                return typeof(ContentFiles).GetProperty(name)?.GetValue(null) as String ?? ContentFiles.NotFound;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<string> GetContentTokens()
        {
            return _contentMarkers.Where(e => e.ReferenceText != null)
                                  .Select(e => e.ReferenceText);
        }

        public string GetFileKey(string content)
        {
            return _contentMarkers.FirstOrDefault(e => e.FileIdentifier != null && e.FileIdentifier.IsMatch(content))?.Key;
        }
    }
}