using System;
using System.Windows;

namespace OpenControls.Wpf.DockManager
{
    internal class FloatingDocumentPaneGroup : FloatingPane, IActiveDocument
    {
        internal FloatingDocumentPaneGroup() : base(new InternalViewContainer())
        {
            IViewContainer.SelectionChanged += IViewContainer_SelectionChanged;
            if (IViewContainer is DocumentContainer)
            {
                (IViewContainer as DocumentContainer).HideCommandsButton();
            }
        }

        private void IViewContainer_SelectionChanged(object sender, EventArgs e)
        {
            FloatingViewModel floatingViewModel = DataContext as FloatingViewModel;
            System.Diagnostics.Trace.Assert(floatingViewModel != null);

            floatingViewModel.Title = Application.Current.MainWindow.Title + " - " + IViewContainer.URL;
        }

        bool IActiveDocument.IsActive
        {
            get
            {
                return IViewContainer.IsActive;
            }
            set
            {
                IViewContainer.IsActive = value;
            }
        }
    }
}
