using System.Windows;

namespace OpenControls.Wpf.DockManager
{
    /// <summary>
    /// Interaction logic for IndicatorPane.xaml
    /// </summary>
    public partial class WindowLocationPane : Window
    {
        public WindowLocationPane()
        {
            InitializeComponent();
        }
      const float _percentSelection = 0.20f;
      public WindowLocation TrySelectIndicator(Point cursorPositionOnScreen) {
         Point localPoint = PointFromScreen(cursorPositionOnScreen);
         if (localPoint.X < Width * _percentSelection) return WindowLocation.Left;
         if (localPoint.X > Width * (1- _percentSelection)) return WindowLocation.Right;
         if (localPoint.Y < Height * _percentSelection) return WindowLocation.Top;
         if (localPoint.Y > Height * (1 - _percentSelection)) return WindowLocation.Bottom;
                return WindowLocation.Middle;
            }

      public void ShowIcons(WindowLocation windowLocations) { }
    }
}
