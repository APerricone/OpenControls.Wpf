﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace OpenControls.Wpf.DockManager
{
    internal static class Helpers
    {
        internal static void DisplayItemsMenu(
            System.Collections.ObjectModel.ObservableCollection<System.Collections.Generic.KeyValuePair<FrameworkElement, IViewModel>> items,
            OpenControls.Wpf.TabHeaderControl.TabHeaderControl tabHeaderControl,
            FrameworkElement selectedUserControl)
        {
            ContextMenu contextMenu = new ContextMenu();
            int i = 0;
            foreach (var item in items)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = item.Value.Title;
                menuItem.IsChecked = item.Key == selectedUserControl;
                menuItem.CommandParameter = i;
                ++i;
                menuItem.Command = new OpenControls.Wpf.Utilities.Command(delegate { tabHeaderControl.SelectedIndex = (int)menuItem.CommandParameter; }, delegate { return true; });
                contextMenu.Items.Add(menuItem);
            }

            contextMenu.IsOpen = true;
        }
    }
}
