using System.Windows.Controls;

namespace OpenControls.Wpf.DockManager.Events
{
    internal class DocumentExtractedEventArgs : System.EventArgs
    {
        public IViewContainer sourceViewContainer { get; set; }
    }
}
