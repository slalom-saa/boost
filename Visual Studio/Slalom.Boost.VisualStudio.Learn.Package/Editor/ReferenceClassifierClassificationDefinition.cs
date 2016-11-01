//------------------------------------------------------------------------------
// <copyright file="ReferenceClassifierClassificationDefinition.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Slalom.Boost.Learn.Package.Editor
{
    /// <summary>
    /// Classification type definition export for ReferenceClassifier
    /// </summary>
    internal static class ReferenceClassifierClassificationDefinition
    {
        // This disables "The field is never used" compiler's warning. Justification: the field is used by MEF.
#pragma warning disable 169

        /// <summary>
        /// Defines the "ReferenceClassifier" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("ReferenceClassifier")]
        [ContentType("code")]
        private static ClassificationTypeDefinition typeDefinition;

#pragma warning restore 169
    }
}
