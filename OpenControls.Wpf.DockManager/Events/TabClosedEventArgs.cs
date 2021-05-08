using System.Windows;

namespace OpenControls.Wpf.DockManager.Events
{
    internal class TabClosedEventArgs : System.EventArgs
    {
        public FrameworkElement UserControl { get; set; }
    }
}
