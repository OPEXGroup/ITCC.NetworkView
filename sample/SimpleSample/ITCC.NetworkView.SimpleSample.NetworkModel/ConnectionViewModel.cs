// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Diagnostics;
using System.Windows;
using ITCC.NetworkView.Utils;

namespace ITCC.NetworkView.SimpleSample.NetworkModel
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
        private ConnectorViewModel _sourceConnector;

        /// <summary>
        /// The destination connector the connection is attached to.
        /// </summary>
        private ConnectorViewModel _destConnector;

        /// <summary>
        /// The source and dest hotspots used for generating connection points.
        /// </summary>
        private Point _sourceConnectorHotspot;
        private Point _destConnectorHotspot;

        #endregion Internal Data Members

        /// <summary>
        /// The source connector the connection is attached to.
        /// </summary>
        public ConnectorViewModel SourceConnector
        {
            get
            {
                return _sourceConnector;
            }
            set
            {
                if (_sourceConnector == value)
                {
                    return;
                }

                if (_sourceConnector != null)
                {
                    Trace.Assert(_sourceConnector.AttachedConnection == this);

                    _sourceConnector.AttachedConnection = null;
                    _sourceConnector.HotspotUpdated -= sourceConnector_HotspotUpdated;
                }

                _sourceConnector = value;

                if (_sourceConnector != null)
                {
                    Trace.Assert(_sourceConnector.AttachedConnection == null);

                    _sourceConnector.AttachedConnection = this;
                    _sourceConnector.HotspotUpdated += sourceConnector_HotspotUpdated;
                    SourceConnectorHotspot = _sourceConnector.Hotspot;
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The destination connector the connection is attached to.
        /// </summary>
        public ConnectorViewModel DestConnector
        {
            get
            {
                return _destConnector;
            }
            set
            {
                if (_destConnector == value)
                {
                    return;
                }

                if (_destConnector != null)
                {
                    Trace.Assert(_destConnector.AttachedConnection == this);

                    _destConnector.AttachedConnection = null;
                    _destConnector.HotspotUpdated += destConnector_HotspotUpdated;
                }

                _destConnector = value;

                if (_destConnector != null)
                {
                    Trace.Assert(_destConnector.AttachedConnection == null);

                    _destConnector.AttachedConnection = this;
                    _destConnector.HotspotUpdated += destConnector_HotspotUpdated;
                    DestConnectorHotspot = _destConnector.Hotspot;
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The source and dest hotspots used for generating connection points.
        /// </summary>
        public Point SourceConnectorHotspot
        {
            get
            {
                return _sourceConnectorHotspot;
            }
            set
            {
                _sourceConnectorHotspot = value;

                OnPropertyChanged();
            }
        }

        public Point DestConnectorHotspot
        {
            get
            {
                return _destConnectorHotspot;
            }
            set
            {
                _destConnectorHotspot = value;

                OnPropertyChanged();
            }
        }

        #region Private Methods

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

        #endregion Private Methods
    }
}
