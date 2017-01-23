// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Windows;
using System.Windows.Input;
using NetworkView.NetworkUI;
using NetworkView.SimpleNetworkModel;

namespace NetworkView.SimpleSample
{
    /// <summary>
    /// This is a Window that uses NetworkView to display a flow-chart.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Convenient accessor for the view-model.
        /// </summary>
        public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        /// <summary>
        /// Event raised when the Window has loaded.
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //
            // Display help text for the sample app.
            //
            var helpTextWindow = new HelpTextWindow
            {
                Left = Left + Width + 5,
                Top = Top,
                Owner = this
            };
            helpTextWindow.Show();
        }

        /// <summary>
        /// Event raised when the user has started to drag out a connection.
        /// </summary>
        private void networkControl_ConnectionDragStarted(object sender, ConnectionDragStartedEventArgs e)
        {
            var draggedOutConnector = (ConnectorViewModel)e.ConnectorDraggedOut;
            var curDragPoint = Mouse.GetPosition(NetworkControl);

            //
            // Delegate the real work to the view model.
            //
            var connection = ViewModel.ConnectionDragStarted(draggedOutConnector, curDragPoint);

            //
            // Must return the view-model object that represents the connection via the event args.
            // This is so that NetworkView can keep track of the object while it is being dragged.
            //
            e.Connection = connection;
        }

        /// <summary>
        /// Event raised while the user is dragging a connection.
        /// </summary>
        private void networkControl_ConnectionDragging(object sender, ConnectionDraggingEventArgs e)
        {
            var curDragPoint = Mouse.GetPosition(NetworkControl);
            var connection = (ConnectionViewModel)e.Connection;
            ViewModel.ConnectionDragging(connection, curDragPoint);
        }

        /// <summary>
        /// Event raised when the user has finished dragging out a connection.
        /// </summary>
        private void networkControl_ConnectionDragCompleted(object sender, ConnectionDragCompletedEventArgs e)
        {
            var connectorDraggedOut = (ConnectorViewModel)e.ConnectorDraggedOut;
            var connectorDraggedOver = (ConnectorViewModel)e.ConnectorDraggedOver;
            var newConnection = (ConnectionViewModel)e.Connection;
            ViewModel.ConnectionDragCompleted(newConnection, connectorDraggedOut, connectorDraggedOver);
        }

        /// <summary>
        /// Event raised to delete the selected node.
        /// </summary>
        private void DeleteSelectedNodes_Executed(object sender, ExecutedRoutedEventArgs e) => ViewModel.DeleteSelectedNodes();

        /// <summary>
        /// Event raised to create a new node.
        /// </summary>
        private void CreateNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var newNodeLocation = Mouse.GetPosition(NetworkControl);
            ViewModel.CreateNode("New Node!", newNodeLocation);
        }


    }
}
