using System.Windows.Controls;

namespace OpenControls.Wpf.DockManager.Events
{
    internal class ElementExtractedEventArgs : System.EventArgs
    {
        public UserControl UserControl { get; set; }
    }
}
