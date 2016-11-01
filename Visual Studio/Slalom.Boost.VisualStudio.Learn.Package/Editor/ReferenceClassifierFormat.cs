//------------------------------------------------------------------------------
// <copyright file="ReferenceClassifierFormat.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Slalom.Boost.Learn.Package.Editor
{
    /// <summary>
    /// Defines an editor format for the ReferenceClassifier type that has a purple background
    /// and is underlined.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "ReferenceClassifier")]
    [Name("ReferenceClassifier")]
    [ContentType("code")]
    [UserVisible(true)] // This should be visible to the end user
    [Order(Before = Priority.Default)] // Set the priority to be after the default classifiers
    internal sealed class ReferenceClassifierFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceClassifierFormat"/> class.
        /// </summary>
        public ReferenceClassifierFormat()
        {
            this.DisplayName = "ReferenceClassifier"; // Human readable version of the name
            this.ForegroundColor = Colors.Blue;
            this.TextDecorations = System.Windows.TextDecorations.Underline;
        }
    }
}
