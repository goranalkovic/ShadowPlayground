using System;
using System.ComponentModel;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace ShadowTesting
{

    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public double YellowX { get; set; } = -50;
        public double YellowY { get; set; } = 50;
        public double YellowZ { get; set; } = 48;
        public double YellowBgOpacity { get; set; } = 0.8;

        public double TurquoiseX { get; set; } = 50;
        public double TurquoiseY { get; set; } = -50;
        public double TurquoiseZ { get; set; } = 24;
        public double TurquoiseBgOpacity { get; set; } = 0.8;

        public double TextBoxHeight { get; set; } = 4;

        public double MaxTextBoxHeight => 240 - YellowZ > 0 ? 240 - YellowZ : 0;

        public double BgRotX { get; set; }
        public double BgRotY { get; set; }
        public double BgRotZ { get; set; }

        public double BgTraX { get; set; }
        public double BgTraY { get; set; }
        public double BgTraZ { get; set; }

        public double BgCenX { get; set; }
        public double BgCenY { get; set; }
        public double BgCenZ { get; set; }

        public double MouseInitialX { get; set; }
        public double MouseInitialY { get; set; }

        public bool MouseWheelDown { get; set; }
        public bool RightMouseDown { get; set; }

        public double BgScale { get; set; } = 1;

        public Duration AnimationDuration { get; set; } = new Duration(new TimeSpan(0, 0, 0, 0, 500));

        public ExponentialEase EasingFunc { get; set; } = new ExponentialEase();

        public MainPage()
        {
            InitializeComponent();

            BgShadow.Receivers.Add(TurGrid);
            Shadow2.Receivers.Add(BgGrid);
            YellowBoxInnerShadow.Receivers.Add(YellowGridShadowCatcher);

            BgCenX = CoreWindow.GetForCurrentThread().Bounds.Width / 2;
            BgCenY = CoreWindow.GetForCurrentThread().Bounds.Height / 2;
        }

        public double YellowGridContentHeight => YellowZ + 4;

        public event PropertyChangedEventHandler PropertyChanged;

        private void ResetGrid(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            ResetView.Begin();
        }

        private void MouseClicked(object sender, PointerRoutedEventArgs e)
        {
            MouseInitialX = (int)e.GetCurrentPoint(null).Position.X;
            MouseInitialY = (int)e.GetCurrentPoint(null).Position.Y;

            if (e.GetCurrentPoint(null).Properties.IsRightButtonPressed) RightMouseDown = true;
            if (e.GetCurrentPoint(null).Properties.IsMiddleButtonPressed) MouseWheelDown = true;

            if (RightMouseDown || MouseWheelDown)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 0);
            }
        }

        private void MouseMoved(object sender, PointerRoutedEventArgs e)
        {
            if (MouseWheelDown)
            {
                BgTraX = e.GetCurrentPoint(null).Position.X - MouseInitialX;
                BgTraY = e.GetCurrentPoint(null).Position.Y - MouseInitialY;
            }

            if (RightMouseDown)
            {
                BgRotY = Map(e.GetCurrentPoint(null).Position.X, 0, CoreWindow.GetForCurrentThread().Bounds.Width, -90, 90);
                BgRotX = Map(e.GetCurrentPoint(null).Position.Y, 0, CoreWindow.GetForCurrentThread().Bounds.Height, 90, -90);

            }
        }

        private void MouseReleased(object sender, PointerRoutedEventArgs e)
        {
            MouseWheelDown = false;
            RightMouseDown = false;

            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
        }



        private void KbKeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

        private void KbKeyUp(object sender, KeyRoutedEventArgs e)
        {

        }

        public static double Map(double value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        private void ResetCenterPoint(object sender, SizeChangedEventArgs e)
        {
            BgCenX = CoreWindow.GetForCurrentThread().Bounds.Width / 2;
            BgCenY = CoreWindow.GetForCurrentThread().Bounds.Height / 2;
        }

        private void WheelZoom(object sender, PointerRoutedEventArgs e)
        {
            if (e.GetCurrentPoint(null).Properties.MouseWheelDelta > 0)
            {
                if (BgScale + 0.05 <= 2.5)
                    BgScale += 0.05;
            }
            else
            {
                if (BgScale - 0.05 > 0)
                    BgScale -= 0.05;
            }
        }

        private void ShowRightPanel(object sender, PointerRoutedEventArgs e)
        {
            RightPanelOpen.Begin();
        }

        private void HideRightPanel(object sender, PointerRoutedEventArgs e)
        {
            RightPanelClose.Begin();
        }
    }
}