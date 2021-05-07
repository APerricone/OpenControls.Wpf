using System.Windows;

namespace OpenControls.Wpf.DockManager
{
    /// <summary>
    /// Interaction logic for EdgeLocationPane.xaml
    /// </summary>
    public partial class SideLocationPane : Window
    {
        public SideLocationPane()
        {
            InitializeComponent();
        }
        const float _percentSelection = 0.1f;
        public WindowLocation TrySelectIndicator(Point cursorPositionOnScreen)
        {
            Point localPoint = PointFromScreen(cursorPositionOnScreen);
            if (localPoint.X < Width * _percentSelection)
                return WindowLocation.LeftSide;
            if (localPoint.X > Width * (1 - _percentSelection))
                return WindowLocation.RightSide;
            if (localPoint.Y < Height * _percentSelection)
                return WindowLocation.TopSide;
            if (localPoint.Y > Height * (1 - _percentSelection))
                return WindowLocation.BottomSide;
            return WindowLocation.None;
        }
    }
}
