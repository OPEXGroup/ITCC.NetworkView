// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections;

namespace NetworkView.Utils
{
    /// <summary>
    /// Arguments to the ItemsAdded and ItemsRemoved events.
    /// </summary>
    public class CollectionItemsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The collection of items that changed.
        /// </summary>
        private readonly ICollection _items;

        public CollectionItemsChangedEventArgs(ICollection items)
        {
            _items = items;
        }

        /// <summary>
        /// The collection of items that changed.
        /// </summary>
        public ICollection Items => _items;
    }
}
