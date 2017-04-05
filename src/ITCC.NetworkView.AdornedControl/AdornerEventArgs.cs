// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.Windows;

namespace ITCC.NetworkView.AdornedControl
{
    public class AdornerEventArgs : RoutedEventArgs
    {
        public AdornerEventArgs(RoutedEvent routedEvent, object source, FrameworkElement adorner) :
            base(routedEvent, source)
        {
            Adorner = adorner;
        }

        public FrameworkElement Adorner { get; }
    }

    public delegate void AdornerEventHandler(object sender, AdornerEventArgs e);
}
