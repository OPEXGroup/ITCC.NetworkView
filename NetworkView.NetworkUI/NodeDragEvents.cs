﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections;
using System.Windows;

namespace NetworkView.NetworkUI
{
    /// <summary>
    /// Base class for node dragging event args.
    /// </summary>
    public class NodeDragEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The NodeItem's or their DataContext (when non-NULL).
        /// </summary>
        public ICollection nodes;

        protected NodeDragEventArgs(RoutedEvent routedEvent, object source, ICollection nodes) :
            base(routedEvent, source)
        {
            this.nodes = nodes;
        }

        /// <summary>
        /// The NodeItem's or their DataContext (when non-NULL).
        /// </summary>
        public ICollection Nodes => nodes;
    }

    /// <summary>
    /// Defines the event handler for NodeDragStarted events.
    /// </summary>
    public delegate void NodeDragEventHandler(object sender, NodeDragEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user starts to drag a node in the network.
    /// </summary>
    public class NodeDragStartedEventArgs : NodeDragEventArgs
    {
        internal NodeDragStartedEventArgs(RoutedEvent routedEvent, object source, ICollection nodes) :
            base(routedEvent, source, nodes)
        {
        }

        /// <summary>
        /// Set to 'false' to disallow dragging.
        /// </summary>
        public bool Cancel { get; set; }
    }

    /// <summary>
    /// Defines the event handler for NodeDragStarted events.
    /// </summary>
    public delegate void NodeDragStartedEventHandler(object sender, NodeDragStartedEventArgs e);

    /// <summary>
    /// Arguments for event raised while user is dragging a node in the network.
    /// </summary>
    public class NodeDraggingEventArgs : NodeDragEventArgs
    {
        /// <summary>
        /// The amount the node has been dragged horizontally.
        /// </summary>
        public readonly double horizontalChange;

        /// <summary>
        /// The amount the node has been dragged vertically.
        /// </summary>
        public readonly double verticalChange;

        internal NodeDraggingEventArgs(RoutedEvent routedEvent, object source, ICollection nodes, double horizontalChange, double verticalChange) :
            base(routedEvent, source, nodes)
        {
            this.horizontalChange = horizontalChange;
            this.verticalChange = verticalChange;
        }

        /// <summary>
        /// The amount the node has been dragged horizontally.
        /// </summary>
        public double HorizontalChange => horizontalChange;

        /// <summary>
        /// The amount the node has been dragged vertically.
        /// </summary>
        public double VerticalChange => verticalChange;
    }

    /// <summary>
    /// Defines the event handler for NodeDragStarted events.
    /// </summary>
    public delegate void NodeDraggingEventHandler(object sender, NodeDraggingEventArgs e);

    /// <summary>
    /// Arguments for event raised when the user has completed dragging a node in the network.
    /// </summary>
    public class NodeDragCompletedEventArgs : NodeDragEventArgs
    {
        public NodeDragCompletedEventArgs(RoutedEvent routedEvent, object source, ICollection nodes) :
            base(routedEvent, source, nodes)
        {
        }
    }

    /// <summary>
    /// Defines the event handler for NodeDragCompleted events.
    /// </summary>
    public delegate void NodeDragCompletedEventHandler(object sender, NodeDragCompletedEventArgs e);
}
