﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace OpenControls.Wpf.DockManager
{
    internal abstract class ViewContainer : Grid, IViewContainer
    {
        protected System.Collections.ObjectModel.ObservableCollection<System.Collections.Generic.KeyValuePair<FrameworkElement, IViewModel>> _items;
        public OpenControls.Wpf.TabHeaderControl.TabHeaderControl TabHeaderControl;
        protected FrameworkElement _selectedUserControl;
        protected Border _gap;
        protected Button _listButton;

        protected void CreateTabControl(int row, int column)
        {
            _items = new System.Collections.ObjectModel.ObservableCollection<System.Collections.Generic.KeyValuePair<FrameworkElement, IViewModel>>();

            TabHeaderControl = new OpenControls.Wpf.TabHeaderControl.TabHeaderControl();
            TabHeaderControl.SelectionChanged += _tabHeaderControl_SelectionChanged;
            TabHeaderControl.CloseTabRequest += _tabHeaderControl_CloseTabRequest;
            TabHeaderControl.FloatTabRequest += _tabHeaderControl_FloatTabRequest;
            TabHeaderControl.ItemsChanged += _tabHeaderControl_ItemsChanged;
            TabHeaderControl.TabMouseDown += TabHeaderControl_TabMouseDown;
            TabHeaderControl.ItemsSource = _items;
            TabHeaderControl.DisplayMemberPath = "Value.Title";
            Children.Add(TabHeaderControl);
            Grid.SetRow(TabHeaderControl, row);
            Grid.SetColumn(TabHeaderControl, column);
        }

        private void TabHeaderControl_TabMouseDown(object sender, EventArgs e)
        {
            TabMouseDown?.Invoke(this, null);
        }

        protected abstract void SetSelectedUserControlGridPosition();

        protected void _tabHeaderControl_SelectionChanged(object sender, System.EventArgs e)
        {
            if ((_selectedUserControl != null) && (Children.Contains(_selectedUserControl)))
            {
                Children.Remove(_selectedUserControl);
            }
            _selectedUserControl = null;

            if ((TabHeaderControl.SelectedIndex > -1) && (TabHeaderControl.SelectedIndex < _items.Count))
            {
                _selectedUserControl = _items[TabHeaderControl.SelectedIndex].Key;
                Children.Add(_selectedUserControl);
                SetSelectedUserControlGridPosition();
            }
            CheckTabCount();

            SelectionChanged?.Invoke(sender, e);
            TabMouseDown?.Invoke(this, null);
        }

        protected void _tabHeaderControl_ItemsChanged(object sender, System.EventArgs e)
        {
            var items = new System.Collections.ObjectModel.ObservableCollection<System.Collections.Generic.KeyValuePair<FrameworkElement, IViewModel>>();

            foreach (var item in TabHeaderControl.Items)
            {
                items.Add((System.Collections.Generic.KeyValuePair<FrameworkElement, IViewModel>)item);
            }
            int selectedIndex = (TabHeaderControl.SelectedIndex == -1) ? 0 : TabHeaderControl.SelectedIndex;

            _items = items;
            TabHeaderControl.SelectedIndex = selectedIndex;

            _tabHeaderControl_SelectionChanged(this, null);
        }

        protected abstract System.Windows.Forms.DialogResult UserConfirmClose(string documentTitle);

        protected void _tabHeaderControl_CloseTabRequest(object sender, EventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            System.Collections.Generic.KeyValuePair<FrameworkElement, IViewModel> item = (System.Collections.Generic.KeyValuePair<FrameworkElement, IViewModel>)sender;
            if (item.Value.CanClose)
            {
                if (item.Value.HasChanged)
                {
                    System.Windows.Forms.DialogResult dialogResult = UserConfirmClose(item.Value.Title);

                    if (dialogResult == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }

                    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        item.Value.Save();
                    }
                }

                int index = _items.IndexOf(item);

                _items.RemoveAt(index);
                TabHeaderControl.ItemsSource = _items;

                if (item.Key == _selectedUserControl)
                {
                    Children.Remove(_selectedUserControl);
                    _selectedUserControl = null;

                    if (_items.Count > 0)
                    {
                        if (index >= _items.Count)
                        {
                            --index;
                        }
                        _selectedUserControl = _items[index].Key;
                        Children.Add(_selectedUserControl);
                    }
                }

                CheckTabCount();

                TabClosed?.Invoke(sender, new Events.TabClosedEventArgs() { UserControl = item.Key });
            }
        }

        protected void _tabHeaderControl_FloatTabRequest(object sender, EventArgs e)
        {
            FloatTabRequest?.Invoke(this, e);
        }

        #region IViewContainer

        public event EventHandler SelectionChanged;
        public event Events.TabClosedEventHandler TabClosed;
        public event Events.DocumentExtractedEventHandler DocumentExtracted;
        public event EventHandler FloatTabRequest;
        public event EventHandler TabMouseDown;

        public string Title
        {
            get
            {
                if ((_items.Count == 0) || (TabHeaderControl.SelectedIndex == -1))
                {
                    return null;
                }

                return _items[TabHeaderControl.SelectedIndex].Value.Title;
            }
        }

        public string URL
        {
            get
            {
                if ((_items.Count == 0) || (TabHeaderControl.SelectedIndex == -1))
                {
                    return null;
                }

                return _items[TabHeaderControl.SelectedIndex].Value.URL;
            }
        }

        public bool IsActive
        {
            get
            {
                return TabHeaderControl.IsActive;
            }
            set
            {
                TabHeaderControl.IsActive = value;
            }
        }

        protected abstract void CheckTabCount();

        public void AddUserControl(FrameworkElement userControl)
        {
            System.Diagnostics.Trace.Assert(userControl != null);
            System.Diagnostics.Trace.Assert(userControl.DataContext is IViewModel);

            _items.Add(new System.Collections.Generic.KeyValuePair<FrameworkElement, IViewModel>(userControl, userControl.DataContext as IViewModel));
            TabHeaderControl.SelectedItem = _items[_items.Count - 1];
        }

        public void InsertUserControl(int index, FrameworkElement userControl)
        {
            System.Diagnostics.Trace.Assert(index > -1);
            System.Diagnostics.Trace.Assert(index <= _items.Count);
            System.Diagnostics.Trace.Assert(userControl != null);
            System.Diagnostics.Trace.Assert(userControl.DataContext is IViewModel);

            _items.Insert(index, new System.Collections.Generic.KeyValuePair<FrameworkElement, IViewModel>(userControl, userControl.DataContext as IViewModel));
            CheckTabCount();
        }

        public FrameworkElement ExtractUserControl(int index)
        {
            if ((index < 0) || (index >= _items.Count))
            {
                return null;
            }

            FrameworkElement userControl = _items[index].Key;
            _items.RemoveAt(index);
            TabHeaderControl.ItemsSource = _items;
            if (Children.Contains(userControl))
            {
                Children.Remove(userControl);
            }
            CheckTabCount();
            return userControl;
        }

        public int GetUserControlCount()
        {
            return _items.Count;
        }

        public int SelectedIndex
        {
            get
            {
                return TabHeaderControl.SelectedIndex;
            }
            set
            {
                TabHeaderControl.SelectedIndex = value;
            }
        }

        private DockPane _Pane;
        public DockPane Pane { get => _Pane; set => _Pane = value; }


        public FrameworkElement GetUserControl(int index)
        {
            if ((index < 0) || (index >= _items.Count))
            {
                return null;
            }

            return _items[index].Key;
        }

        public IViewModel GetIViewModel(int index)
        {
            FrameworkElement userControl = GetUserControl(index);
            if (userControl == null)
            {
                return null;
            }

            return userControl.DataContext as IViewModel;
        }

        public void ExtractDocuments(IViewContainer sourceViewContainer)
        {
            System.Diagnostics.Trace.Assert(sourceViewContainer != null);
            if(sourceViewContainer is InternalViewContainer && ((InternalViewContainer)sourceViewContainer).CheckCyclic(this))
            {
                return;
            }
            while (true)
            {
                FrameworkElement userControl = sourceViewContainer.ExtractUserControl(0);
                if (userControl == null)
                {
                    break;
                }

                AddUserControl(userControl);
            }
            DocumentExtracted?.Invoke(this, new Events.DocumentExtractedEventArgs() { sourceViewContainer = sourceViewContainer });

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

        #endregion IViewContainer
    }
}
