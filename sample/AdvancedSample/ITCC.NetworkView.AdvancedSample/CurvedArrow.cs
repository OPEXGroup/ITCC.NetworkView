﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ITCC.NetworkView.AdvancedSample
{
    /// <summary>
    /// Defines an arrow that has multiple points.
    /// </summary>
    public class CurvedArrow : Shape
    {
        #region Dependency Property/Event Definitions

        public static readonly DependencyProperty ArrowHeadLengthProperty =
            DependencyProperty.Register("ArrowHeadLength", typeof(double), typeof(CurvedArrow),
                new FrameworkPropertyMetadata(20.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowHeadWidthProperty =
            DependencyProperty.Register("ArrowHeadWidth", typeof(double), typeof(CurvedArrow),
                new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(PointCollection), typeof(CurvedArrow),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion Dependency Property/Event Definitions

        /// <summary>
        /// The angle (in degrees) of the arrow head.
        /// </summary>
        public double ArrowHeadLength
        {
            get
            {
                return (double)GetValue(ArrowHeadLengthProperty);
            }
            set
            {
                SetValue(ArrowHeadLengthProperty, value);
            }
        }

        /// <summary>
        /// The width of the arrow head.
        /// </summary>
        public double ArrowHeadWidth
        {
            get
            {
                return (double)GetValue(ArrowHeadWidthProperty);
            }
            set
            {
                SetValue(ArrowHeadWidthProperty, value);
            }
        }

        /// <summary>
        /// The intermediate points that make up the line between the start and the end.
        /// </summary>
        public PointCollection Points
        {
            get
            {
                return (PointCollection)GetValue(PointsProperty);
            }
            set
            {
                SetValue(PointsProperty, value);
            }
        }

        #region Private Methods

        /// <summary>
        /// Return the shape's geometry.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (Points == null || Points.Count < 2)
                {
                    return new GeometryGroup();
                }

                //
                // Geometry has not yet been generated.
                // Generate geometry and cache it.
                //
                var geometry = GenerateGeometry();

                var group = new GeometryGroup();
                group.Children.Add(geometry);

                GenerateArrowHeadGeometry(group);

                //
                // Return cached geometry.
                //
                return group;
            }
        }

        /// <summary>
        /// Generate the geometry for the three optional arrow symbols at the start, middle and end of the arrow.
        /// </summary>
        private void GenerateArrowHeadGeometry(GeometryGroup geometryGroup)
        {
            var startPoint = Points[0];

            var penultimatePoint = Points[Points.Count - 2];
            var arrowHeadTip = Points[Points.Count - 1];
            var startDir = arrowHeadTip - penultimatePoint;
            startDir.Normalize();
            var basePoint = arrowHeadTip - (startDir * ArrowHeadLength);
            var crossDir = new Vector(-startDir.Y, startDir.X);

            var arrowHeadPoints = new Point[3];
            arrowHeadPoints[0] = arrowHeadTip;
            arrowHeadPoints[1] = basePoint - (crossDir * (ArrowHeadWidth / 2));
            arrowHeadPoints[2] = basePoint + (crossDir * (ArrowHeadWidth / 2));

            //
            // Build geometry for the arrow head.
            //
            var arrowHeadFig = new PathFigure();
            arrowHeadFig.IsClosed = true;
            arrowHeadFig.IsFilled = true;
            arrowHeadFig.StartPoint = arrowHeadPoints[0];
            arrowHeadFig.Segments.Add(new LineSegment(arrowHeadPoints[1], true));
            arrowHeadFig.Segments.Add(new LineSegment(arrowHeadPoints[2], true));

            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(arrowHeadFig);

            geometryGroup.Children.Add(pathGeometry);
        }

        /// <summary>
        /// Generate the shapes geometry.
        /// </summary>
        protected Geometry GenerateGeometry()
        {
            var pathGeometry = new PathGeometry();

            if (Points.Count == 2 || Points.Count == 3)
            {
                // Make a straight line.
                var fig = new PathFigure();
                fig.IsClosed = false;
                fig.IsFilled = false;
                fig.StartPoint = Points[0];

                for (var i = 1; i < Points.Count; ++i)
                {
                    fig.Segments.Add(new LineSegment(Points[i], true));
                }

                pathGeometry.Figures.Add(fig);
            }
            else
            {
                var adjustedPoints = new PointCollection();
                adjustedPoints.Add(Points[0]);
                for (var i = 1; i < Points.Count; ++i)
                {
                    adjustedPoints.Add(Points[i]);
                }

                if (adjustedPoints.Count == 4)
                {
                    // Make a curved line.
                    var fig = new PathFigure();
                    fig.IsClosed = false;
                    fig.IsFilled = false;
                    fig.StartPoint = adjustedPoints[0];
                    fig.Segments.Add(new BezierSegment(adjustedPoints[1], adjustedPoints[2], adjustedPoints[3], true));

                    pathGeometry.Figures.Add(fig);
                }
                else if (adjustedPoints.Count >= 5)
                {
                    // Make a curved line.
                    var fig = new PathFigure();
                    fig.IsClosed = false;
                    fig.IsFilled = false;
                    fig.StartPoint = adjustedPoints[0];

                    adjustedPoints.RemoveAt(0);

                    while (adjustedPoints.Count > 3)
                    {
                        var generatedPoint = adjustedPoints[1] + ((adjustedPoints[2] - adjustedPoints[1]) / 2);

                        fig.Segments.Add(new BezierSegment(adjustedPoints[0], adjustedPoints[1], generatedPoint, true));

                        adjustedPoints.RemoveAt(0);
                        adjustedPoints.RemoveAt(0);
                    }

                    if (adjustedPoints.Count == 2)
                    {
                        fig.Segments.Add(new BezierSegment(adjustedPoints[0], adjustedPoints[0], adjustedPoints[1], true));
                    }
                    else
                    {
                        Trace.Assert(adjustedPoints.Count == 2);

                        fig.Segments.Add(new BezierSegment(adjustedPoints[0], adjustedPoints[1], adjustedPoints[2], true));
                    }

                    pathGeometry.Figures.Add(fig);
                }
            }

            return pathGeometry;
        }

        #endregion Private Methods
    }
}
