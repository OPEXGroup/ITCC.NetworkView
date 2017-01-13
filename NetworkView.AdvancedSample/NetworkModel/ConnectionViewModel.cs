using System;
using System.Windows;
using System.Windows.Media;
using NetworkView.Utils;

namespace NetworkView.AdvancedNetworkModel
{
    /// <summary>
    /// Defines a connection between two connectors (aka connection points) of two nodes.
    /// </summary>
    public sealed class ConnectionViewModel : AbstractModelBase
    {
        #region Internal Data Members

        /// <summary>
        /// The source connector the connection is attached to.
        /// </summary>
        private ConnectorViewModel sourceConnector = null;

        /// <summary>
        /// The destination connector the connection is attached to.
        /// </summary>
        private ConnectorViewModel destConnector = null;

        /// <summary>
        /// The source and dest hotspots used for generating connection points.
        /// </summary>
        private Point sourceConnectorHotspot;
        private Point destConnectorHotspot;

        /// <summary>
        /// Points that make up the connection.
        /// </summary>
        private PointCollection points = null;

        #endregion Internal Data Members

        /// <summary>
        /// The source connector the connection is attached to.
        /// </summary>
        public ConnectorViewModel SourceConnector
        {
            get
            {
                return sourceConnector;
            }
            set
            {
                if (sourceConnector == value)
                {
                    return;
                }

                if (sourceConnector != null)
                {
                    sourceConnector.AttachedConnections.Remove(this);
                    sourceConnector.HotspotUpdated -= sourceConnector_HotspotUpdated;
                }

                sourceConnector = value;

                if (sourceConnector != null)
                {
                    sourceConnector.AttachedConnections.Add(this);
                    sourceConnector.HotspotUpdated += sourceConnector_HotspotUpdated;
                    SourceConnectorHotspot = sourceConnector.Hotspot;
                }

                OnPropertyChanged("SourceConnector");
                OnConnectionChanged();
            }
        }

        /// <summary>
        /// The destination connector the connection is attached to.
        /// </summary>
        public ConnectorViewModel DestConnector
        {
            get
            {
                return destConnector;
            }
            set
            {
                if (destConnector == value)
                {
                    return;
                }

                if (destConnector != null)
                {
                    destConnector.AttachedConnections.Remove(this);
                    destConnector.HotspotUpdated -= destConnector_HotspotUpdated;
                }

                destConnector = value;

                if (destConnector != null)
                {
                    destConnector.AttachedConnections.Add(this);
                    destConnector.HotspotUpdated += destConnector_HotspotUpdated;
                    DestConnectorHotspot = destConnector.Hotspot;
                }

                OnPropertyChanged("DestConnector");
                OnConnectionChanged();
            }
        }

        /// <summary>
        /// The source and dest hotspots used for generating connection points.
        /// </summary>
        public Point SourceConnectorHotspot
        {
            get
            {
                return sourceConnectorHotspot;
            }
            set
            {
                sourceConnectorHotspot = value;

                ComputeConnectionPoints();

                OnPropertyChanged("SourceConnectorHotspot");
            }
        }

        public Point DestConnectorHotspot
        {
            get
            {
                return destConnectorHotspot;
            }
            set
            {
                destConnectorHotspot = value;

                ComputeConnectionPoints();

                OnPropertyChanged("DestConnectorHotspot");
            }
        }

        /// <summary>
        /// Points that make up the connection.
        /// </summary>
        public PointCollection Points
        {
            get
            {
                return points;
            }
            set
            {
                points = value;

                OnPropertyChanged("Points");
            }
        }

        /// <summary>
        /// Event fired when the connection has changed.
        /// </summary>
        public event EventHandler<EventArgs> ConnectionChanged;

        #region Private Methods

        /// <summary>
        /// Raises the 'ConnectionChanged' event.
        /// </summary>
        private void OnConnectionChanged()
        {
            if (ConnectionChanged != null)
            {
                ConnectionChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Event raised when the hotspot of the source connector has been updated.
        /// </summary>
        private void sourceConnector_HotspotUpdated(object sender, EventArgs e)
        {
            SourceConnectorHotspot = SourceConnector.Hotspot;
        }

        /// <summary>
        /// Event raised when the hotspot of the dest connector has been updated.
        /// </summary>
        private void destConnector_HotspotUpdated(object sender, EventArgs e)
        {
            DestConnectorHotspot = DestConnector.Hotspot;
        }

        /// <summary>
        /// Rebuild connection points.
        /// </summary>
        private void ComputeConnectionPoints()
        {
            PointCollection computedPoints = new PointCollection();
            computedPoints.Add(SourceConnectorHotspot);

            double deltaX = Math.Abs(DestConnectorHotspot.X - SourceConnectorHotspot.X);
            double deltaY = Math.Abs(DestConnectorHotspot.Y - SourceConnectorHotspot.Y);
            if (deltaX > deltaY)
            {
                double midPointX = SourceConnectorHotspot.X + ((DestConnectorHotspot.X - SourceConnectorHotspot.X) / 2);
                computedPoints.Add(new Point(midPointX, SourceConnectorHotspot.Y));
                computedPoints.Add(new Point(midPointX, DestConnectorHotspot.Y));
            }
            else
            {
                double midPointY = SourceConnectorHotspot.Y + ((DestConnectorHotspot.Y - SourceConnectorHotspot.Y) / 2);
                computedPoints.Add(new Point(SourceConnectorHotspot.X, midPointY));
                computedPoints.Add(new Point(DestConnectorHotspot.X, midPointY));
            }

            computedPoints.Add(DestConnectorHotspot);
            computedPoints.Freeze();

            Points = computedPoints;
        }

        #endregion Private Methods
    }
}
