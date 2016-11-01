using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Slalom.Boost.Learn.Content;
using Slalom.Boost.Learn.Package.ToolWindows;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.Commands;

namespace Slalom.Boost.Learn.Package.Editor
{
    /// <summary>
    /// LinkReferenceAdornment places red boxes behind all the "a"s in the editor window
    /// </summary>
    internal sealed class LinkReferenceAdornment
    {
        /// <summary>
        /// The layer of the adornment.
        /// </summary>
        private readonly IAdornmentLayer layer;

        /// <summary>
        /// Text view where the adornment is created.
        /// </summary>
        private readonly IWpfTextView view;

        private IEnumerable<string> _contentTokens;
        private IEnumerable<IVisualStudioCommand> commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkReferenceAdornment"/> class.
        /// </summary>
        /// <param name="view">Text view to create the adornment for</param>
        public LinkReferenceAdornment(IWpfTextView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            layer = view.GetAdornmentLayer("LinkReferenceAdornment");

            this.view = view;
            this.view.LayoutChanged += this.OnLayoutChanged;

            var controller = new ContentController();
            _contentTokens = controller.GetContentTokens();

            var commandController = new CommandController();
            commands = commandController.GetCommands();
        }

        /// <summary>
        /// Handles whenever the text displayed in the view changes by adding the adornment to any reformatted lines
        /// </summary>
        /// <remarks><para>This event is raised whenever the rendered text displayed in the <see cref="ITextView"/> changes.</para>
        /// <para>It is raised whenever the view does a layout (which happens when DisplayTextLineContainingBufferPosition is called or in response to text or classification changes).</para>
        /// <para>It is also raised whenever the view scrolls horizontally or when its size changes.</para>
        /// </remarks>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        internal void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            foreach (var line in e.NewOrReformattedLines)
            {
                this.CreateVisuals(line);
            }
        }

        private void CreateBackground(Geometry geometry, Group match, SnapshotSpan span, MouseButtonEventHandler OnClick)
        {
            var drawing = new GeometryDrawing(Brushes.Transparent, new Pen(Brushes.Transparent, 1), geometry);
            drawing.Freeze();
            var drawingImage = new DrawingImage(drawing);
            drawingImage.Freeze();

            var image = new Image
            {
                Source = drawingImage,
                Cursor = Cursors.Hand
            };

            Canvas.SetLeft(image, geometry.Bounds.Left);
            Canvas.SetTop(image, geometry.Bounds.Top);

            image.PreviewMouseDown += OnClick;
            layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, image, null);
        }

        private void CreateUnderline(Geometry geometry, Group match, SnapshotSpan span)
        {
            var left = geometry.Bounds.Left;
            var bottom = geometry.Bounds.Bottom;
            var right = geometry.Bounds.Right;

            var copy = new LineGeometry(new Point(left, bottom), new Point(right, bottom));
            var drawing = new GeometryDrawing(Brushes.Orange, new Pen(Brushes.Orange, 1), copy);
            drawing.Freeze();
            var drawingImage = new DrawingImage(drawing);
            drawingImage.Freeze();

            var image = new Image
            {
                Source = drawingImage
            };

            Canvas.SetLeft(image, geometry.Bounds.Left);
            Canvas.SetTop(image, geometry.Bounds.Bottom);

            layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, image, null);
        }

        private void CreateVisuals(ITextViewLine line)
        {
            var textViewLines = view.TextViewLines;

            var content = "";

            // Loop through each character, and place a box around any 'a'
            for (int charIndex = line.Start; charIndex < line.End; charIndex++)
            {
                content += view.TextSnapshot[charIndex];
            }

            foreach (var name in _contentTokens)
            {
                var regex = new Regex($@"<seealso.*boost=""({name})""");
                foreach (var match in regex.Match(content).Groups.OfType<Group>().Skip(1))
                {
                    var start = line.Start + match.Index;
                    var end = start + match.Length;
                    var span = new SnapshotSpan(view.TextSnapshot, Span.FromBounds(start, end));
                    var geometry = textViewLines.GetMarkerGeometry(span);

                    this.CreateUnderline(geometry, match, span);
                    this.CreateBackground(geometry, match, span, (a, b) =>
                    {
                        BoostLearnWindow.OnTokenClicked(match.Value);
                    });
                }
            }
            
            foreach (var command in commands)
            {
                foreach (var marker in command.Markers)
                {
                    var regex = new Regex(Regex.Escape(marker).Replace("Boost", "(Boost)"));
                    foreach (var match in regex.Match(content).Groups.OfType<Group>().Skip(1))
                    {
                        var start = line.Start + match.Index;
                        var end = start + match.Length;
                        var span = new SnapshotSpan(view.TextSnapshot, Span.FromBounds(start, end));
                        var geometry = textViewLines.GetMarkerGeometry(span);

                        this.CreateUnderline(geometry, match, span);
                        this.CreateBackground(geometry, match, span, (a, b) =>
                        {
                            command.Execute();
                        });
                    }
                }

            }
        }
    }
}