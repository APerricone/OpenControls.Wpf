using OpenControls.Wpf.DockManager.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace OpenControls.Wpf.DockManager
{
    class InternalViewContainer : IViewContainer
    {
        public string Title => "Internal";

        public string URL => "";


        public bool IsActive { get => false; set { } }
        private int _selIndex = 0;
        public int SelectedIndex { get => _selIndex; set => _selIndex = value; }
        private DockPane _Pane;
        public DockPane Pane { get => _Pane; set => _Pane=value; }

        public event EventHandler SelectionChanged;
        public event TabClosedEventHandler TabClosed;
        public event DocumentExtractedEventHandler DocumentExtracted;
        public event EventHandler FloatTabRequest;
        public event EventHandler TabMouseDown;

        protected ObservableCollection<KeyValuePair<FrameworkElement, IViewModel>> _items;
        public InternalViewContainer()
        {
            _items = new ObservableCollection<KeyValuePair<FrameworkElement, IViewModel>>();
        }
        public void AddUserControl(FrameworkElement userControl)
        {
            System.Diagnostics.Trace.Assert(userControl != null);
            System.Diagnostics.Trace.Assert(userControl.DataContext is IViewModel);
            _items.Add(new KeyValuePair<FrameworkElement, IViewModel>(userControl, userControl.DataContext as IViewModel));
            if(Pane==null)
            {
                IViewContainer parent = ((IViewContainer)userControl.Parent);
                Pane = parent.Pane;
            }
        }

        public void ExtractDocuments(IViewContainer sourceViewContainer)
        {
            for (int index = 0; ; ++index)
            {
                FrameworkElement userControl = sourceViewContainer.ExtractUserControl(index);
                if (userControl == null)
                {
                    break;
                }

                AddUserControl(userControl);
            }
            DocumentExtracted?.Invoke(this, new Events.DocumentExtractedEventArgs() { sourceViewContainer = sourceViewContainer });
        }

        public void Clear()
        {
            _items.Clear();
        }

        public FrameworkElement ExtractUserControl(int index)
        {
            if ((index < 0) || (index >= _items.Count))
            {
                return null;
            }
            FrameworkElement userControl = _items[index].Key;
            _items.RemoveAt(index);
            //userControl.Parent.Remove( userControl );
            IViewContainer parent = ((IViewContainer)userControl.Parent);
            if (parent!=null)
                parent.ExtractUserControl(parent.GetUserControlIndex(userControl));
            return userControl;
        }
        internal bool CheckCyclic(ViewContainer viewContainer)
        {
            if (_items.Count==0)
            {
                return false;
            }
            FrameworkElement userControl = _items[0].Key;
            IViewContainer parent = ((IViewContainer)userControl.Parent);
            return parent == viewContainer;
        }

        public IViewModel GetIViewModel(int index)
        {
            if ((index < 0) || (index >= _items.Count))
            {
                return null;
            }
            return _items[index].Value;
        }

        public FrameworkElement GetUserControl(int index)
        {
            if ((index < 0) || (index >= _items.Count))
            {
                return null;
            }
            return _items[index].Key;
        }

        public int GetUserControlCount()
        {
            return _items.Count;
        }

        public void InsertUserControl(int index, FrameworkElement userControl)
        {
            System.Diagnostics.Trace.Assert(index > -1);
            System.Diagnostics.Trace.Assert(index <= _items.Count);
            System.Diagnostics.Trace.Assert(userControl != null);
            System.Diagnostics.Trace.Assert(userControl.DataContext is IViewModel);

            _items.Insert(index, new System.Collections.Generic.KeyValuePair<FrameworkElement, IViewModel>(userControl, userControl.DataContext as IViewModel));
        }

        public int GetUserControlIndex(FrameworkElement userControl)
        {
            for (int i = 0; i < _items.Count; ++i)
            {
                if (_items[i].Key == userControl)
                    return i;
            }
            return -1;
        }
    }
}
