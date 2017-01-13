using System.Windows;

namespace NetworkView.AdornedControl
{
    public class AdornerEventArgs : RoutedEventArgs
    {
        private readonly FrameworkElement adorner;

        public AdornerEventArgs(RoutedEvent routedEvent, object source, FrameworkElement adorner) :
            base(routedEvent, source)
        {
            this.adorner = adorner;
        }

        public FrameworkElement Adorner
        {
            get
            {
                return adorner;
            }
        }
    }

    public delegate void AdornerEventHandler(object sender, AdornerEventArgs e);
}
