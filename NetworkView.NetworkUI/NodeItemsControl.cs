// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Windows;
using System.Windows.Controls;

namespace NetworkView.NetworkUI
{
    /// <summary>
    /// Implements an ListBox for displaying nodes in the NetworkView UI.
    /// </summary>
    internal class NodeItemsControl : ListBox
    {
        public NodeItemsControl()
        {
            //
            // By default, we don't want this UI element to be focusable.
            //
            Focusable = false;
        }

        #region Private Methods

        /// <summary>
        /// Find the NodeItem UI element that has the specified data context.
        /// Return null if no such NodeItem exists.
        /// </summary>
        internal NodeItem FindAssociatedNodeItem(object nodeDataContext) => (NodeItem) ItemContainerGenerator.ContainerFromItem(nodeDataContext);

        /// <summary>
        /// Creates or identifies the element that is used to display the given item. 
        /// </summary>
        protected override DependencyObject GetContainerForItemOverride() => new NodeItem();

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own container. 
        /// </summary>
        protected override bool IsItemItsOwnContainerOverride(object item) => item is NodeItem;

        #endregion Private Methods
    }
}
