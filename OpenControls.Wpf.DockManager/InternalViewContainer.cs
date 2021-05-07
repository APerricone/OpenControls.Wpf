using OpenControls.Wpf.DockManager.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public event EventHandler SelectionChanged;
        public event TabClosedEventHandler TabClosed;
        public event ElementExtractedEventHandler ElementExtracted;
        public event EventHandler FloatTabRequest;
        public event EventHandler TabMouseDown;

        protected ObservableCollection<KeyValuePair<UserControl, IViewModel>> _items;
        public InternalViewContainer()
        {
            _items = new ObservableCollection<KeyValuePair<UserControl, IViewModel>>();
        }
        public void AddUserControl(UserControl userControl)
        {
            System.Diagnostics.Trace.Assert(userControl != null);
            System.Diagnostics.Trace.Assert(userControl.DataContext is IViewModel);
            _items.Add(new KeyValuePair<UserControl, IViewModel>(userControl, userControl.DataContext as IViewModel));
        }

        public void ExtractDocuments(IViewContainer sourceViewContainer)
        {
            for (int index = 0; ; ++index)
            {
                UserControl userControl = sourceViewContainer.ExtractUserControl(index);
                if (userControl == null)
                {
                    break;
                }

                AddUserControl(userControl);
            }
        }

        public UserControl ExtractUserControl(int index)
        {
            if ((index < 0) || (index >= _items.Count))
            {
                return null;
            }
            UserControl userControl = _items[index].Key;
            _items.RemoveAt(index);
            //userControl.Parent.Remove( userControl );
            UserControl test = ((IViewContainer)userControl.Parent).GetUserControl(index);
            IViewContainer parent = ((IViewContainer)userControl.Parent);
            parent.ExtractUserControl(parent.GetUserControlIndex(userControl));
            ElementExtracted?.Invoke(this, new Events.ElementExtractedEventArgs() { UserControl = userControl });
            return userControl;
        }

        public IViewModel GetIViewModel(int index)
        {
            if ((index < 0) || (index >= _items.Count))
            {
                return null;
            }
            return _items[index].Value;
        }

        public UserControl GetUserControl(int index)
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

        public void InsertUserControl(int index, UserControl userControl)
        {
            System.Diagnostics.Trace.Assert(index > -1);
            System.Diagnostics.Trace.Assert(index <= _items.Count);
            System.Diagnostics.Trace.Assert(userControl != null);
            System.Diagnostics.Trace.Assert(userControl.DataContext is IViewModel);

            _items.Insert(index, new System.Collections.Generic.KeyValuePair<UserControl, IViewModel>(userControl, userControl.DataContext as IViewModel));
        }

        public int GetUserControlIndex(UserControl userControl)
        {
            for(int i=0;i<_items.Count;++i)
            {
                if (_items[i].Key == userControl)
                    return i;
            }
            return -1;
        }
    }
}
