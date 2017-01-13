using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.Diagnostics;
using System.Windows;

namespace NetworkModel
{
    /// <summary>
    /// Defines a connector (aka connection point) that can be attached to a node and is used to connect the node to another node.
    /// </summary>
    public sealed class ConnectorViewModel : AbstractModelBase
    {
        #region Internal Data Members

        /// <summary>
        /// The hotspot (or center) of the connector.
        /// This is pushed through from ConnectorItem in the UI.
        /// </summary>
        private Point hotspot;

        #endregion Internal Data Members
        
        /// <summary>
        /// The connection that is attached to this connector, or null if no connection is attached.
        /// </summary>
        public ConnectionViewModel AttachedConnection
        {
            get;
            internal set;
        }

        /// <summary>
        /// The parent node that the connector is attached to, or null if the connector is not attached to any node.
        /// </summary>
        public NodeViewModel ParentNode
        {
            get;
            internal set;
        }

        /// <summary>
        /// The hotspot (or center) of the connector.
        /// This is pushed through from ConnectorItem in the UI.
        /// </summary>
        public Point Hotspot
        {
            get
            {
                return hotspot;
            }
            set
            {
                if (hotspot == value)
                {
                    return;
                }

                hotspot = value;

                OnHotspotUpdated();
            }
        }

        /// <summary>
        /// Event raised when the connector hotspot has been updated.
        /// </summary>
        public event EventHandler<EventArgs> HotspotUpdated;

        #region Private Methods

        /// <summary>
        /// Called when the connector hotspot has been updated.
        /// </summary>
        private void OnHotspotUpdated()
        {
            OnPropertyChanged("Hotspot");

            if (HotspotUpdated != null)
            {
                HotspotUpdated(this, EventArgs.Empty);
            }
        }

        #endregion Private Methods
    }
}
