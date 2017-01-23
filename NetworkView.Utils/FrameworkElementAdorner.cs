// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

//
// This code based on code available here:
//
//  http://www.codeproject.com/KB/WPF/WPFJoshSmith.aspx
//
namespace NetworkView.Utils
{
    //
    // This class is an adorner that allows a FrameworkElement derived class to adorn another FrameworkElement.
    //
    public class FrameworkElementAdorner : Adorner
    {
        //
        // The framework element that is the adorner. 
        //

        //
        // Placement of the child.
        //
        private readonly AdornerPlacement _horizontalAdornerPlacement = AdornerPlacement.Inside;
        private readonly AdornerPlacement _verticalAdornerPlacement = AdornerPlacement.Inside;

        //
        // Offset of the child.
        //
        private readonly double _offsetX;
        private readonly double _offsetY;

        //
        // Position of the child (when not set to NaN).
        //

        public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement)
            : base(adornedElement)
        {
            if (adornedElement == null)
            {
                throw new ArgumentNullException(nameof(adornedElement));
            }

            if (adornerChildElement == null)
            {
                throw new ArgumentNullException(nameof(adornerChildElement));
            }

            Child = adornerChildElement;

            AddLogicalChild(adornerChildElement);
            AddVisualChild(adornerChildElement);
        }

        public FrameworkElementAdorner(FrameworkElement adornerChildElement, FrameworkElement adornedElement,
                                       AdornerPlacement horizontalAdornerPlacement, AdornerPlacement verticalAdornerPlacement,
                                       double offsetX, double offsetY)
            : base(adornedElement)
        {
            if (adornedElement == null)
            {
                throw new ArgumentNullException(nameof(adornedElement));
            }

            if (adornerChildElement == null)
            {
                throw new ArgumentNullException(nameof(adornerChildElement));
            }

            Child = adornerChildElement;
            _horizontalAdornerPlacement = horizontalAdornerPlacement;
            _verticalAdornerPlacement = verticalAdornerPlacement;
            _offsetX = offsetX;
            _offsetY = offsetY;

            adornedElement.SizeChanged += adornedElement_SizeChanged;

            AddLogicalChild(adornerChildElement);
            AddVisualChild(adornerChildElement);
        }

        /// <summary>
        /// Event raised when the adorned control's size has changed.
        /// </summary>
        private void adornedElement_SizeChanged(object sender, SizeChangedEventArgs e) => InvalidateMeasure();

        //
        // The framework element that is the adorner. 
        //
        public FrameworkElement Child { get; }

        //
        // Position of the child (when not set to NaN).
        //
        public double PositionX { get; set; } = double.NaN;

        public double PositionY { get; set; } = double.NaN;

        protected override Size MeasureOverride(Size constraint)
        {
            Child.Measure(constraint);
            return Child.DesiredSize;
        }

        /// <summary>
        /// Determine the X coordinate of the child.
        /// </summary>
        private double DetermineX()
        {
            switch (Child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                {
                    if (_horizontalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        var adornerWidth = Child.DesiredSize.Width;
                        var position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return (position.X - adornerWidth) + _offsetX;
                    }
                    if (_horizontalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        return -Child.DesiredSize.Width + _offsetX;
                    }
                    return _offsetX;
                }
                case HorizontalAlignment.Right:
                {
                    if (_horizontalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        var position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return position.X + _offsetX;
                    }
                    if (_horizontalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        var adornedWidth = AdornedElement.ActualWidth;
                        return adornedWidth + _offsetX;
                    }
                    else
                    {
                        var adornerWidth = Child.DesiredSize.Width;
                        var adornedWidth = AdornedElement.ActualWidth;
                        var x = adornedWidth - adornerWidth;
                        return x + _offsetX;
                    }
                }
                case HorizontalAlignment.Center:
                {
                    var adornerWidth = Child.DesiredSize.Width;

                    if (_horizontalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        var position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return (position.X - (adornerWidth / 2)) + _offsetX;
                    }
                    var adornedWidth = AdornedElement.ActualWidth;
                    var x = (adornedWidth / 2) - (adornerWidth / 2);
                    return x + _offsetX;
                }
                case HorizontalAlignment.Stretch:
                {
                    return 0.0;
                }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the Y coordinate of the child.
        /// </summary>
        private double DetermineY()
        {
            switch (Child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                {
                    if (_verticalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        var adornerWidth = Child.DesiredSize.Width;
                        var position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return (position.Y - adornerWidth) + _offsetY;
                    }
                    if (_verticalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        return -Child.DesiredSize.Height + _offsetY;
                    }
                    return _offsetY;
                }
                case VerticalAlignment.Bottom:
                {
                    if (_verticalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        var position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return position.Y + _offsetY;
                    }
                    if (_verticalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        var adornedHeight = AdornedElement.ActualHeight;
                        return adornedHeight + _offsetY;
                    }
                    else
                    {
                        var adornerHeight = Child.DesiredSize.Height;
                        var adornedHeight = AdornedElement.ActualHeight;
                        var x = adornedHeight - adornerHeight;
                        return x + _offsetY;
                    }
                }
                case VerticalAlignment.Center:
                {
                    var adornerHeight = Child.DesiredSize.Height;

                    if (_verticalAdornerPlacement == AdornerPlacement.Mouse)
                    {
                        var position = Mouse.GetPosition(AdornerLayer.GetAdornerLayer(AdornedElement));
                        return (position.Y - (adornerHeight/2)) + _offsetY;
                    }
                    var adornedHeight = AdornedElement.ActualHeight;
                    var y = (adornedHeight / 2) - (adornerHeight / 2);
                    return y + _offsetY;
                }
                case VerticalAlignment.Stretch:
                {
                    return 0.0;
                }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the width of the child.
        /// </summary>
        private double DetermineWidth()
        {
            if (!double.IsNaN(PositionX))
            {
                return Child.DesiredSize.Width;
            }

            switch (Child.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                {
                    return Child.DesiredSize.Width;
                }
                case HorizontalAlignment.Right:
                {
                    return Child.DesiredSize.Width;
                }
                case HorizontalAlignment.Center:
                {
                    return Child.DesiredSize.Width;
                }
                case HorizontalAlignment.Stretch:
                {
                    return AdornedElement.ActualWidth;
                }
            }

            return 0.0;
        }

        /// <summary>
        /// Determine the height of the child.
        /// </summary>
        private double DetermineHeight()
        {
            if (!double.IsNaN(PositionY))
            {
                return Child.DesiredSize.Height;
            }

            switch (Child.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                {
                    return Child.DesiredSize.Height;
                }
                case VerticalAlignment.Bottom:
                {
                    return Child.DesiredSize.Height;
                }
                case VerticalAlignment.Center:
                {
                    return Child.DesiredSize.Height; 
                }
                case VerticalAlignment.Stretch:
                {
                    return AdornedElement.ActualHeight;
                }
            }

            return 0.0;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var x = PositionX;
            if (double.IsNaN(x))
            {
                x = DetermineX();
            }
            var y = PositionY;
            if (double.IsNaN(y))
            {
                y = DetermineY();
            }
            var adornerWidth = DetermineWidth();
            var adornerHeight = DetermineHeight();
            Child.Arrange(new Rect(x, y, adornerWidth, adornerHeight));
            return finalSize;
        }

        protected override int VisualChildrenCount => 1;

        protected override Visual GetVisualChild(int index) => Child;

        protected override IEnumerator LogicalChildren
        {
            get
            {
                var list = new ArrayList {Child};
                return list.GetEnumerator();
            }
        }

        /// <summary>
        /// Disconnect the child element from the visual tree so that it may be reused later.
        /// </summary>
        public void DisconnectChild()
        {
            RemoveLogicalChild(Child);
            RemoveVisualChild(Child);
        }

        /// <summary>
        /// Override AdornedElement from base class for less type-checking.
        /// </summary>
        public new FrameworkElement AdornedElement => (FrameworkElement)base.AdornedElement;
    }
}
