using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace NetworkModel
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
                    Trace.Assert(sourceConnector.AttachedConnection == this);

                    sourceConnector.AttachedConnection = null;
                    sourceConnector.HotspotUpdated -= new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                }

                sourceConnector = value;

                if (sourceConnector != null)
                {
                    Trace.Assert(sourceConnector.AttachedConnection == null);

                    sourceConnector.AttachedConnection = this;
                    sourceConnector.HotspotUpdated += new EventHandler<EventArgs>(sourceConnector_HotspotUpdated);
                    this.SourceConnectorHotspot = sourceConnector.Hotspot;
                }

                OnPropertyChanged("SourceConnector");
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
                    Trace.Assert(destConnector.AttachedConnection == this);

                    destConnector.AttachedConnection = null;
                    destConnector.HotspotUpdated += new EventHandler<EventArgs>(destConnector_HotspotUpdated);
                }

                destConnector = value;

                if (destConnector != null)
                {
                    Trace.Assert(destConnector.AttachedConnection == null);

                    destConnector.AttachedConnection = this;
                    destConnector.HotspotUpdated += new EventHandler<EventArgs>(destConnector_HotspotUpdated);
                    this.DestConnectorHotspot = destConnector.Hotspot;
                }

                OnPropertyChanged("DestConnector");
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

                OnPropertyChanged("DestConnectorHotspot");
            }
        }

        #region Private Methods

        /// <summary>
        /// Event raised when the hotspot of the source connector has been updated.
        /// </summary>
        private void sourceConnector_HotspotUpdated(object sender, EventArgs e)
        {
            this.SourceConnectorHotspot = this.SourceConnector.Hotspot;
        }

        /// <summary>
        /// Event raised when the hotspot of the dest connector has been updated.
        /// </summary>
        private void destConnector_HotspotUpdated(object sender, EventArgs e)
        {
            this.DestConnectorHotspot = this.DestConnector.Hotspot;
        }

        #endregion Private Methods
    }
}
