// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Windows;
using ITCC.NetworkView.Utils;

namespace ITCC.NetworkView.AdvancedSample.NetworkModel
{
    /// <summary>
    /// Defines a node in the view-model.
    /// Nodes are connected to other nodes through attached connectors (aka anchor/connection points).
    /// </summary>
    public sealed class NodeViewModel : AbstractModelBase
    {
        #region Private Data Members

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
        /// The Z index of the node.
        /// </summary>
        private int _zIndex;

        /// <summary>
        /// The size of the node.
        /// 
        /// Important Note: 
        ///     The size of a node in the UI is not determined by this property!!
        ///     Instead the size of a node in the UI is determined by the data-template for the Node class.
        ///     When the size is computed via the UI it is then pushed into the view-model
        ///     so that our application code has access to the size of a node.
        /// </summary>
        private Size _size = Size.Empty;

        /// <summary>
        /// List of input connectors (connections points) attached to the node.
        /// </summary>
        private ImpObservableCollection<ConnectorViewModel> _inputConnectors;

        /// <summary>
        /// List of output connectors (connections points) attached to the node.
        /// </summary>
        private ImpObservableCollection<ConnectorViewModel> _outputConnectors;

        /// <summary>
        /// Set to 'true' when the node is selected.
        /// </summary>
        private bool _isSelected;

        #endregion Private Data Members

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

                OnPropertyChanged();
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

                OnPropertyChanged();
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

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The Z index of the node.
        /// </summary>
        public int ZIndex
        {
            get
            {
                return _zIndex;
            }
            set
            {
                if (_zIndex == value)
                {
                    return;
                }

                _zIndex = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The size of the node.
        /// 
        /// Important Note: 
        ///     The size of a node in the UI is not determined by this property!!
        ///     Instead the size of a node in the UI is determined by the data-template for the Node class.
        ///     When the size is computed via the UI it is then pushed into the view-model
        ///     so that our application code has access to the size of a node.
        /// </summary>
        public Size Size
        {
            get
            {
                return _size;
            }
            set
            {
                if (_size == value)
                {
                    return;
                }

                _size = value;

                SizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Event raised when the size of the node is changed.
        /// The size will change when the UI has determined its size based on the contents
        /// of the nodes data-template.  It then pushes the size through to the view-model
        /// and this 'SizeChanged' event occurs.
        /// </summary>
        public event EventHandler<EventArgs> SizeChanged;

        /// <summary>
        /// List of input connectors (connections points) attached to the node.
        /// </summary>
        public ImpObservableCollection<ConnectorViewModel> InputConnectors
        {
            get
            {
                if (_inputConnectors == null)
                {
                    _inputConnectors = new ImpObservableCollection<ConnectorViewModel>();
                    _inputConnectors.ItemsAdded += inputConnectors_ItemsAdded;
                    _inputConnectors.ItemsRemoved += connectors_ItemsRemoved;
                }

                return _inputConnectors;
            }
        }

        /// <summary>
        /// List of output connectors (connections points) attached to the node.
        /// </summary>
        public ImpObservableCollection<ConnectorViewModel> OutputConnectors
        {
            get
            {
                if (_outputConnectors == null)
                {
                    _outputConnectors = new ImpObservableCollection<ConnectorViewModel>();
                    _outputConnectors.ItemsAdded += outputConnectors_ItemsAdded;
                    _outputConnectors.ItemsRemoved += connectors_ItemsRemoved;
                }

                return _outputConnectors;
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

                foreach (var connector in InputConnectors)
                {
                    attachedConnections.AddRange(connector.AttachedConnections);
                }

                foreach (var connector in OutputConnectors)
                {
                    attachedConnections.AddRange(connector.AttachedConnections);
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

                OnPropertyChanged();
            }
        }

        #region Private Methods

        /// <summary>
        /// Event raised when connectors are added to the node.
        /// </summary>
        private void inputConnectors_ItemsAdded(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectorViewModel connector in e.Items)
            {
                connector.ParentNode = this;
                connector.Type = ConnectorType.Input;
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
                connector.Type = ConnectorType.Undefined;
            }
        }

        /// <summary>
        /// Event raised when connectors are added to the node.
        /// </summary>
        private void outputConnectors_ItemsAdded(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectorViewModel connector in e.Items)
            {
                connector.ParentNode = this;
                connector.Type = ConnectorType.Output;
            }
        }

        #endregion Private Methods
    }
}
