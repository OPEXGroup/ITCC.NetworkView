using System.ComponentModel;

namespace NetworkView.Utils
{
    /// <summary>
    /// Abstract base for view-model classes that need to implement INotifyPropertyChanged.
    /// </summary>
    public abstract class AbstractModelBase : INotifyPropertyChanged
    {
#if DEBUG
        private static int _nextObjectId;
        private readonly int _objectDebugId = _nextObjectId++;

        public int ObjectDebugId => _objectDebugId;
#endif //  DEBUG

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Event raised to indicate that a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
