// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Windows;

namespace NetworkView.NetworkUI
{
    /// <summary>
    /// Arguments for event raised when the user starts to drag a connector out from a node.
    /// </summary>
    internal class ConnectorItemDragStartedEventArgs : RoutedEventArgs
    {
        internal ConnectorItemDragStartedEventArgs(RoutedEvent routedEvent, object source) :
            base(routedEvent, source)
        {
        }

        /// <summary>
        /// Cancel dragging out of the connector.
        /// </summary>
        public bool Cancel
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Defines the event handler for ConnectorDragStarted events.
    /// </summary>
    internal delegate void ConnectorItemDragStartedEventHandler(object sender, ConnectorItemDragStartedEventArgs e);

    /// <summary>
    /// Arguments for event raised while user is dragging a node in the network.
    /// </summary>
    internal class ConnectorItemDraggingEventArgs : RoutedEventArgs
    {
        public ConnectorItemDraggingEventArgs(RoutedEvent routedEvent, object source, double horizontalChange, double verticalChange) :
            base(routedEvent, source)
        {
            HorizontalChange = horizontalChange;
            VerticalChange = verticalChange;
        }

        /// <summary>
        /// The amount the node has been dragged horizontally.
        /// </summary>
        public double HorizontalChange { get; }

        /// <summary>
        /// The amount the node has been dragged vertically.
        /// </summary>
        public double VerticalChange { get; }
    }

    /// <summary>
    /// Defines the event handler for ConnectorDragStarted events.
    /// </summary>
    internal delegate void ConnectorItemDraggingEventHandler(object sender, ConnectorItemDraggingEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user has completed dragging a connector.
    /// </summary>
    internal class ConnectorItemDragCompletedEventArgs : RoutedEventArgs
    {
        public ConnectorItemDragCompletedEventArgs(RoutedEvent routedEvent, object source) :
            base(routedEvent, source)
        {
        }
    }

    /// <summary>
    /// Defines the event handler for ConnectorDragCompleted events.
    /// </summary>
    internal delegate void ConnectorItemDragCompletedEventHandler(object sender, ConnectorItemDragCompletedEventArgs e);

}
