// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using NetworkView.Utils;

namespace NetworkView.SimpleNetworkModel
{
    /// <summary>
    /// Defines a node in the view-model.
    /// Nodes are connected to other nodes through attached connectors (aka connection points).
    /// </summary>
    public sealed class NodeViewModel : AbstractModelBase
    {
        #region Internal Data Members

        /// <summary>
        ///     Coordinates comparasion precision
        /// </summary>
        private const double Epsilon = 1e-6;

        /// <summary>
        /// The name of the node.
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// The X coordinate for the position of the node.
        /// </summary>
        private double _x;

        /// <summary>
        /// The Y coordinate for the position of the node.
        /// </summary>
        private double _y;

        /// <summary>
        /// Set to 'true' when the node is selected.
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// List of input connectors (connections points) attached to the node.
        /// </summary>
        private ImpObservableCollection<ConnectorViewModel> _connectors;

        #endregion Internal Data Members

        public NodeViewModel()
        {
        }

        public NodeViewModel(string name)
        {
            _name = name;
        }

        /// <summary>
        /// The name of the node.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;

                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// The X coordinate for the position of the node.
        /// </summary>
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                if (Math.Abs(_x - value) < Epsilon)
                {
                    return;
                }

                _x = value;

                OnPropertyChanged("X");
            }
        }

        /// <summary>
        /// The Y coordinate for the position of the node.
        /// </summary>
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (Math.Abs(_y - value) < Epsilon)
                {
                    return;
                }

                _y = value;

                OnPropertyChanged("Y");
            }
        }

        /// <summary>
        /// List of connectors (connection anchor points) attached to the node.
        /// </summary>
        public ImpObservableCollection<ConnectorViewModel> Connectors
        {
            get
            {
                if (_connectors != null)
                    return _connectors;

                _connectors = new ImpObservableCollection<ConnectorViewModel>();
                _connectors.ItemsAdded += connectors_ItemsAdded;
                _connectors.ItemsRemoved += connectors_ItemsRemoved;

                return _connectors;
            }
        }

        /// <summary>
        /// A helper property that retrieves a list (a new list each time) of all connections attached to the node. 
        /// </summary>
        public ICollection<ConnectionViewModel> AttachedConnections
        {
            get
            {
                var attachedConnections = new List<ConnectionViewModel>();

                foreach (var connector in Connectors)
                {
                    if (connector.AttachedConnection != null)
                    {
                        attachedConnections.Add(connector.AttachedConnection);
                    }
                }

                return attachedConnections;
            }
        }

        /// <summary>
        /// Set to 'true' when the node is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected == value)
                {
                    return;
                }

                _isSelected = value;

                OnPropertyChanged("IsSelected");
            }
        }

        #region Private Methods

        /// <summary>
        /// Event raised when connectors are added to the node.
        /// </summary>
        private void connectors_ItemsAdded(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectorViewModel connector in e.Items)
            {
                connector.ParentNode = this;
            }
        }

        /// <summary>
        /// Event raised when connectors are removed from the node.
        /// </summary>
        private void connectors_ItemsRemoved(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectorViewModel connector in e.Items)
            {
                connector.ParentNode = null;
            }
        }

        #endregion Private Methods
    }
}
