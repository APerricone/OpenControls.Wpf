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
        const double _percentSelection = 0.1;
        const double _percentOutside = 0.05;
        public WindowLocation TrySelectIndicator(Point cursorPositionOnScreen)
        {
            Point localPoint = PointFromScreen(cursorPositionOnScreen);
            double xMarginInside = Width * _percentSelection;
            double xMarginOutside = -Width * _percentOutside;
            double yMarginInside = Height * _percentSelection;
            double yMarginOutside = -Height * _percentOutside;
            if (localPoint.X < xMarginInside && localPoint.X > xMarginOutside)
                return WindowLocation.LeftSide;
            if (localPoint.X-Width < xMarginInside && localPoint.X-Width > xMarginOutside)
                return WindowLocation.RightSide;
            if (localPoint.Y < yMarginInside && localPoint.Y > yMarginOutside)
                return WindowLocation.TopSide;
            if (localPoint.Y-Height < yMarginInside && localPoint.Y - Height > yMarginOutside)
                return WindowLocation.BottomSide;
            return WindowLocation.None;
        }
    }
}
