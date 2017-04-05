// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ITCC.NetworkView.AdvancedSample
{
    /// <summary>
    /// Interaction logic for OverviewWindow.xaml
    /// </summary>
    public partial class OverviewWindow : Window
    {
        public OverviewWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Convenient accessor for the view-model.
        /// </summary>
        public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        /// <summary>
        /// Event raised when the ZoomAndPanControl is loaded.
        /// </summary>
        private void overview_Loaded(object sender, RoutedEventArgs e) => Overview.ScaleToFit();

        /// <summary>
        /// Event raised when the size of the ZoomAndPanControl changes.
        /// </summary>
        private void overview_SizeChanged(object sender, SizeChangedEventArgs e) => Overview.ScaleToFit();

        /// <summary>
        /// Event raised when the user drags the overview zoom rect.
        /// </summary>
        private void overviewZoomRectThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //
            // Update the position of the overview rect as the user drags it around.
            //
            var newContentOffsetX = Math.Min(Math.Max(0.0, Canvas.GetLeft(OverviewZoomRectThumb) + e.HorizontalChange), ViewModel.ContentWidth - ViewModel.ContentViewportWidth);
            Canvas.SetLeft(OverviewZoomRectThumb, newContentOffsetX);

            var newContentOffsetY = Math.Min(Math.Max(0.0, Canvas.GetTop(OverviewZoomRectThumb) + e.VerticalChange), ViewModel.ContentHeight - ViewModel.ContentViewportHeight);
            Canvas.SetTop(OverviewZoomRectThumb, newContentOffsetY);
        }

        /// <summary>
        /// Event raised on mouse down.
        /// </summary>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //
            // Update the position of the overview rect to the point that was clicked.
            //
            var clickedPoint = e.GetPosition(NetworkControl);
            var newX = clickedPoint.X - (OverviewZoomRectThumb.Width / 2);
            var newY = clickedPoint.Y - (OverviewZoomRectThumb.Height / 2);
            Canvas.SetLeft(OverviewZoomRectThumb, newX);
            Canvas.SetTop(OverviewZoomRectThumb, newY);
        }

    }
}
