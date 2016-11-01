using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Slalom.Boost.Learn.Content;

namespace Slalom.Boost.Learn.Package.Editor
{
    /// <summary>
    /// Classifier that classifies all text as an instance of the "ReferenceClassifier" classification type.
    /// </summary>
    internal class ReferenceClassifier : IClassifier
    {
        /// <summary>
        /// Classification type.
        /// </summary>
        private readonly IClassificationType classificationType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceClassifier"/> class.
        /// </summary>
        /// <param name="registry">Classification registry.</param>
        internal ReferenceClassifier(IClassificationTypeRegistryService registry)
        {
            classificationType = registry.GetClassificationType("ReferenceClassifier");
            var controller = new ContentController();
            this.Expressions = controller.GetContentNames().Select(e => new Regex($@"(?<!\S)({e})(?!\w)")).ToList();
        }

        public List<Regex> Expressions { get; }

        private ClassificationSpan CreateSpan(SnapshotSpan span, Group match)
        {
            var snapshotSpan = new SnapshotSpan(span.Snapshot,
                span.Start.Position + match.Index,
                match.Length);
            return new ClassificationSpan(
                snapshotSpan, classificationType);
        }

        #region IClassifier

#pragma warning disable 67

        /// <summary>
        /// An event that occurs when the classification of a span of text has changed.
        /// </summary>
        /// <remarks>
        /// This event gets raised if a non-text change would affect the classification in some way,
        /// for example typing /* would cause the classification to change in C# without directly
        /// affecting the span.
        /// </remarks>
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

#pragma warning restore 67

        /// <summary>
        /// Gets all the <see cref="ClassificationSpan"/> objects that intersect with the given range of text.
        /// </summary>
        /// <remarks>
        /// This method scans the given SnapshotSpan for potential matches for this classification.
        /// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </remarks>
        /// <param name="span">The span currently being classified.</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var startline = span.Start.GetContainingLine();
            var endline = (span.End - 1).GetContainingLine();
            var text = span.Snapshot.GetText(new SnapshotSpan(startline.Start, endline.End));

            var matches = this.Expressions.SelectMany(e => e.Match(text).Groups.OfType<Group>().Skip(1));
            return matches.Select(e => this.CreateSpan(span, e)).ToList();
        }

        #endregion
    }
}