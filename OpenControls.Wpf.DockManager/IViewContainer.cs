using System;
using System.Windows;

namespace OpenControls.Wpf.DockManager
{
    internal interface IViewContainer
    {
        string Title { get; }
        string URL { get; }
        bool IsActive { get; set; }
        DockPane Pane { get; set; }
        void AddUserControl(FrameworkElement userControl);
        void InsertUserControl(int index, FrameworkElement userControl);
        FrameworkElement ExtractUserControl(int index);
        int GetUserControlCount();
        int GetUserControlIndex(FrameworkElement userControl);
        int SelectedIndex { get; set; }
        FrameworkElement GetUserControl(int index);
        IViewModel GetIViewModel(int index);
        void ExtractDocuments(IViewContainer sourceViewContainer);

        event EventHandler SelectionChanged;
        event Events.TabClosedEventHandler TabClosed;
        event Events.DocumentExtractedEventHandler DocumentExtracted;
        event EventHandler FloatTabRequest;
        event EventHandler TabMouseDown;
    }
}
