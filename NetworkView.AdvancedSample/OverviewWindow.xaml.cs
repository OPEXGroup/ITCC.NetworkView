using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace NetworkView.AdvancedSample
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
        public MainWindowViewModel ViewModel
        {
            get
            {
                return (MainWindowViewModel)DataContext;
            }
        }

        /// <summary>
        /// Event raised when the ZoomAndPanControl is loaded.
        /// </summary>
        private void overview_Loaded(object sender, RoutedEventArgs e)
        {
            //
            // Update the scale so that the entire content fits in the window.
            //
            Overview.ScaleToFit();
        }

        /// <summary>
        /// Event raised when the size of the ZoomAndPanControl changes.
        /// </summary>
        private void overview_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //
            // Update the scale so that the entire content fits in the window.
            //
            Overview.ScaleToFit();
        }

        /// <summary>
        /// Event raised when the user drags the overview zoom rect.
        /// </summary>
        private void overviewZoomRectThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //
            // Update the position of the overview rect as the user drags it around.
            //
            double newContentOffsetX = Math.Min(Math.Max(0.0, Canvas.GetLeft(OverviewZoomRectThumb) + e.HorizontalChange), this.ViewModel.ContentWidth - this.ViewModel.ContentViewportWidth);
            Canvas.SetLeft(OverviewZoomRectThumb, newContentOffsetX);

            double newContentOffsetY = Math.Min(Math.Max(0.0, Canvas.GetTop(OverviewZoomRectThumb) + e.VerticalChange), this.ViewModel.ContentHeight - this.ViewModel.ContentViewportHeight);
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
            Point clickedPoint = e.GetPosition(NetworkControl);
            double newX = clickedPoint.X - (OverviewZoomRectThumb.Width / 2);
            double newY = clickedPoint.Y - (OverviewZoomRectThumb.Height / 2);
            Canvas.SetLeft(OverviewZoomRectThumb, newX);
            Canvas.SetTop(OverviewZoomRectThumb, newY);
        }

    }
}
