// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NetworkView.AdvancedNetworkModel;
using NetworkView.ZoomAndPan;

namespace NetworkView.AdvancedSample
{
    /// <summary>
    /// This is a partial implementation of MainWindow that just contains most of the zooming and panning functionality.
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Specifies the current state of the mouse handling logic.
        /// </summary>
        private MouseHandlingMode _mouseHandlingMode = MouseHandlingMode.None;

        /// <summary>
        /// The point that was clicked relative to the ZoomAndPanControl.
        /// </summary>
        private Point _origZoomAndPanControlMouseDownPoint;

        /// <summary>
        /// The point that was clicked relative to the content that is contained within the ZoomAndPanControl.
        /// </summary>
        private Point _origContentMouseDownPoint;

        /// <summary>
        /// Records which mouse button clicked during mouse dragging.
        /// </summary>
        private MouseButton _mouseButtonDown;

        /// <summary>
        /// Saves the previous zoom rectangle, pressing the backspace key jumps back to this zoom rectangle.
        /// </summary>
        private Rect _prevZoomRect;

        /// <summary>
        /// Save the previous content scale, pressing the backspace key jumps back to this scale.
        /// </summary>
        private double _prevZoomScale;

        /// <summary>
        /// Set to 'true' when the previous zoom rect is saved.
        /// </summary>
        private bool _prevZoomRectSet;

        /// <summary>
        /// Event raised on mouse down in the NetworkView.
        /// </summary> 
        private void networkControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NetworkControl.Focus();
            Keyboard.Focus(NetworkControl);

            _mouseButtonDown = e.ChangedButton;
            _origZoomAndPanControlMouseDownPoint = e.GetPosition(zoomAndPanControl);
            _origContentMouseDownPoint = e.GetPosition(NetworkControl);

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0 &&
                (e.ChangedButton == MouseButton.Left ||
                 e.ChangedButton == MouseButton.Right))
            {
                // Shift + left- or right-down initiates zooming mode.
                _mouseHandlingMode = MouseHandlingMode.Zooming;
            }
            else if (_mouseButtonDown == MouseButton.Left &&
                     (Keyboard.Modifiers & ModifierKeys.Control) == 0)
            {
                //
                // Initiate panning, when control is not held down.
                // When control is held down left dragging is used for drag selection.
                // After panning has been initiated the user must drag further than the threshold value to actually start drag panning.
                //
                _mouseHandlingMode = MouseHandlingMode.Panning;
            }

            if (_mouseHandlingMode != MouseHandlingMode.None)
            {
                // Capture the mouse so that we eventually receive the mouse up event.
                NetworkControl.CaptureMouse();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised on mouse up in the NetworkView.
        /// </summary>
        private void networkControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_mouseHandlingMode != MouseHandlingMode.None)
            {
                if (_mouseHandlingMode == MouseHandlingMode.Panning)
                { 
                    //
                    // Panning was initiated but dragging was abandoned before the mouse
                    // cursor was dragged further than the threshold distance.
                    // This means that this basically just a regular left mouse click.
                    // Because it was a mouse click in empty space we need to clear the current selection.
                    //
                }
                else if (_mouseHandlingMode == MouseHandlingMode.Zooming)
                {
                    if (_mouseButtonDown == MouseButton.Left)
                    {
                        // Shift + left-click zooms in on the content.
                        ZoomIn(_origContentMouseDownPoint);
                    }
                    else if (_mouseButtonDown == MouseButton.Right)
                    {
                        // Shift + left-click zooms out from the content.
                        ZoomOut(_origContentMouseDownPoint);
                    }
                }
                else if (_mouseHandlingMode == MouseHandlingMode.DragZooming)
                {
                    // When drag-zooming has finished we zoom in on the rectangle that was highlighted by the user.
                    ApplyDragZoomRect();
                }

                //
                // Reenable clearing of selection when empty space is clicked.
                // This is disabled when drag panning is in progress.
                //
                NetworkControl.IsClearSelectionOnEmptySpaceClickEnabled = true;

                //
                // Reset the override cursor.
                // This is set to a special cursor while drag panning is in progress.
                //
                Mouse.OverrideCursor = null;

                NetworkControl.ReleaseMouseCapture();
                _mouseHandlingMode = MouseHandlingMode.None;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised on mouse move in the NetworkView.
        /// </summary>
        private void networkControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseHandlingMode == MouseHandlingMode.Panning)
            {
                var curZoomAndPanControlMousePoint = e.GetPosition(zoomAndPanControl);
                var dragOffset = curZoomAndPanControlMousePoint - _origZoomAndPanControlMouseDownPoint;
                double dragThreshold = 10;
                if (Math.Abs(dragOffset.X) > dragThreshold ||
                    Math.Abs(dragOffset.Y) > dragThreshold)
                {
                    //
                    // The user has dragged the cursor further than the threshold distance, initiate
                    // drag panning.
                    //
                    _mouseHandlingMode = MouseHandlingMode.DragPanning;
                    NetworkControl.IsClearSelectionOnEmptySpaceClickEnabled = false;
                    Mouse.OverrideCursor = Cursors.ScrollAll;
                }

                e.Handled = true;
            }
            else if (_mouseHandlingMode == MouseHandlingMode.DragPanning)
            {
                //
                // The user is left-dragging the mouse.
                // Pan the viewport by the appropriate amount.
                //
                var curContentMousePoint = e.GetPosition(NetworkControl);
                var dragOffset = curContentMousePoint - _origContentMouseDownPoint;

                zoomAndPanControl.ContentOffsetX -= dragOffset.X;
                zoomAndPanControl.ContentOffsetY -= dragOffset.Y;

                e.Handled = true;
            }
            else if (_mouseHandlingMode == MouseHandlingMode.Zooming)
            {
                var curZoomAndPanControlMousePoint = e.GetPosition(zoomAndPanControl);
                var dragOffset = curZoomAndPanControlMousePoint - _origZoomAndPanControlMouseDownPoint;
                double dragThreshold = 10;
                if (_mouseButtonDown == MouseButton.Left &&
                    (Math.Abs(dragOffset.X) > dragThreshold ||
                    Math.Abs(dragOffset.Y) > dragThreshold))
                {
                    //
                    // When Shift + left-down zooming mode and the user drags beyond the drag threshold,
                    // initiate drag zooming mode where the user can drag out a rectangle to select the area
                    // to zoom in on.
                    //
                    _mouseHandlingMode = MouseHandlingMode.DragZooming;
                    var curContentMousePoint = e.GetPosition(NetworkControl);
                    InitDragZoomRect(_origContentMouseDownPoint, curContentMousePoint);
                }

                e.Handled = true;
            }
            else if (_mouseHandlingMode == MouseHandlingMode.DragZooming)
            {
                //
                // When in drag zooming mode continously update the position of the rectangle
                // that the user is dragging out.
                //
                var curContentMousePoint = e.GetPosition(NetworkControl);
                SetDragZoomRect(_origContentMouseDownPoint, curContentMousePoint);

                e.Handled = true;
            }
        }

        /// <summary>
        /// Event raised by rotating the mouse wheel.
        /// </summary>
        private void networkControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta > 0)
            {
                var curContentMousePoint = e.GetPosition(NetworkControl);
                ZoomIn(curContentMousePoint);
            }
            else if (e.Delta < 0)
            {
                var curContentMousePoint = e.GetPosition(NetworkControl);
                ZoomOut(curContentMousePoint);
            }
        }

        /// <summary>
        /// Event raised when the user has double clicked in the zoom and pan control.
        /// </summary>
        private void networkControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0)
            {
                var doubleClickPoint = e.GetPosition(NetworkControl);
                zoomAndPanControl.AnimatedSnapTo(doubleClickPoint);
            }
        }

        /// <summary>
        /// The 'ZoomIn' command (bound to the plus key) was executed.
        /// </summary>
        private void ZoomIn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var o = NetworkControl.SelectedNode;

            ZoomIn(new Point(zoomAndPanControl.ContentZoomFocusX, zoomAndPanControl.ContentZoomFocusY));
        }

        /// <summary>
        /// The 'ZoomOut' command (bound to the minus key) was executed.
        /// </summary>
        private void ZoomOut_Executed(object sender, ExecutedRoutedEventArgs e) => ZoomOut(new Point(zoomAndPanControl.ContentZoomFocusX, zoomAndPanControl.ContentZoomFocusY));

        /// <summary>
        /// The 'JumpBackToPrevZoom' command was executed.
        /// </summary>
        private void JumpBackToPrevZoom_Executed(object sender, ExecutedRoutedEventArgs e) => JumpBackToPrevZoom();

        /// <summary>
        /// Determines whether the 'JumpBackToPrevZoom' command can be executed.
        /// </summary>
        private void JumpBackToPrevZoom_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _prevZoomRectSet;
        }

        /// <summary>
        /// The 'Fill' command was executed.
        /// </summary>
        private void FitContent_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IList nodes = null;

            if (NetworkControl.SelectedNodes.Count > 0)
            {
                nodes = NetworkControl.SelectedNodes;
            }
            else
            {
                nodes = ViewModel.Network.Nodes;
                if (nodes.Count == 0)
                {
                    return;
                }
            }

            SavePrevZoomRect();

            var actualContentRect = DetermineAreaOfNodes(nodes);

            //
            // Inflate the content rect by a fraction of the actual size of the total content area.
            // This puts a nice border around the content we are fitting to the viewport.
            //
            actualContentRect.Inflate(NetworkControl.ActualWidth / 40, NetworkControl.ActualHeight / 40);

            zoomAndPanControl.AnimatedZoomTo(actualContentRect);
        }

        /// <summary>
        /// Determine the area covered by the specified list of nodes.
        /// </summary>
        private Rect DetermineAreaOfNodes(IList nodes)
        {
            var firstNode = (NodeViewModel)nodes[0];
            var actualContentRect = new Rect(firstNode.X, firstNode.Y, firstNode.Size.Width, firstNode.Size.Height);

            for (var i = 1; i < nodes.Count; ++i)
            {
                var node = (NodeViewModel)nodes[i];
                var nodeRect = new Rect(node.X, node.Y, node.Size.Width, node.Size.Height);
                actualContentRect = Rect.Union(actualContentRect, nodeRect);
            }
            return actualContentRect;
        }

        /// <summary>
        /// The 'Fill' command was executed.
        /// </summary>
        private void Fill_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SavePrevZoomRect();

            zoomAndPanControl.AnimatedScaleToFit();
        }

        /// <summary>
        /// The 'OneHundredPercent' command was executed.
        /// </summary>
        private void OneHundredPercent_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SavePrevZoomRect();

            zoomAndPanControl.AnimatedZoomTo(1.0);
        }

        /// <summary>
        /// Jump back to the previous zoom level.
        /// </summary>
        private void JumpBackToPrevZoom()
        {
            zoomAndPanControl.AnimatedZoomTo(_prevZoomScale, _prevZoomRect);

            ClearPrevZoomRect();
        }

        /// <summary>
        /// Zoom the viewport out, centering on the specified point (in content coordinates).
        /// </summary>
        private void ZoomOut(Point contentZoomCenter) => zoomAndPanControl.ZoomAboutPoint(zoomAndPanControl.ContentScale - 0.1, contentZoomCenter);

        /// <summary>
        /// Zoom the viewport in, centering on the specified point (in content coordinates).
        /// </summary>
        private void ZoomIn(Point contentZoomCenter) => zoomAndPanControl.ZoomAboutPoint(zoomAndPanControl.ContentScale + 0.1, contentZoomCenter);

        /// <summary>
        /// Initialize the rectangle that the use is dragging out.
        /// </summary>
        private void InitDragZoomRect(Point pt1, Point pt2)
        {
            SetDragZoomRect(pt1, pt2);

            dragZoomCanvas.Visibility = Visibility.Visible;
            dragZoomBorder.Opacity = 0.5;
        }

        /// <summary>
        /// Update the position and size of the rectangle that user is dragging out.
        /// </summary>
        private void SetDragZoomRect(Point pt1, Point pt2)
        {
            double x, y, width, height;

            //
            // Deterine x,y,width and height of the rect inverting the points if necessary.
            // 

            if (pt2.X < pt1.X)
            {
                x = pt2.X;
                width = pt1.X - pt2.X;
            }
            else
            {
                x = pt1.X;
                width = pt2.X - pt1.X;
            }

            if (pt2.Y < pt1.Y)
            {
                y = pt2.Y;
                height = pt1.Y - pt2.Y;
            }
            else
            {
                y = pt1.Y;
                height = pt2.Y - pt1.Y;
            }

            //
            // Update the coordinates of the rectangle that is being dragged out by the user.
            // The we offset and rescale to convert from content coordinates.
            //
            Canvas.SetLeft(dragZoomBorder, x);
            Canvas.SetTop(dragZoomBorder, y);
            dragZoomBorder.Width = width;
            dragZoomBorder.Height = height;
        }

        /// <summary>
        /// When the user has finished dragging out the rectangle the zoom operation is applied.
        /// </summary>
        private void ApplyDragZoomRect()
        {
            //
            // Record the previous zoom level, so that we can jump back to it when the backspace key is pressed.
            //
            SavePrevZoomRect();

            //
            // Retreive the rectangle that the user draggged out and zoom in on it.
            //
            var contentX = Canvas.GetLeft(dragZoomBorder);
            var contentY = Canvas.GetTop(dragZoomBorder);
            var contentWidth = dragZoomBorder.Width;
            var contentHeight = dragZoomBorder.Height;
            zoomAndPanControl.AnimatedZoomTo(new Rect(contentX, contentY, contentWidth, contentHeight));

            FadeOutDragZoomRect();
        }

        //
        // Fade out the drag zoom rectangle.
        //
        private void FadeOutDragZoomRect()
        {
            AnimationHelper.StartAnimation(dragZoomBorder, OpacityProperty, 0.0, 0.1,
                delegate {
                    dragZoomCanvas.Visibility = Visibility.Collapsed;
                });
        }

        //
        // Record the previous zoom level, so that we can jump back to it when the backspace key is pressed.
        //
        private void SavePrevZoomRect()
        {
            _prevZoomRect = new Rect(zoomAndPanControl.ContentOffsetX, zoomAndPanControl.ContentOffsetY, zoomAndPanControl.ContentViewportWidth, zoomAndPanControl.ContentViewportHeight);
            _prevZoomScale = zoomAndPanControl.ContentScale;
            _prevZoomRectSet = true;
        }

        /// <summary>
        /// Clear the memory of the previous zoom level.
        /// </summary>
        private void ClearPrevZoomRect()
        {
            _prevZoomRectSet = false;
        }
    }
}
