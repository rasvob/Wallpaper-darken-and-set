using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WallSetter_v2.ViewModels;

namespace WallSetter_v2.WallpaperControl
{
    /// <summary>
    /// Interaction logic for WallpaperControl.xaml
    /// </summary>
    public partial class WallpaperControl : UserControl
    {
        public static readonly DependencyProperty RootCanvasProperty = DependencyProperty.Register(
            "RootCanvas", typeof(Canvas), typeof(WallpaperControl), new PropertyMetadata(default(Canvas)));

        public Canvas RootCanvas
        {
            get => (Canvas) GetValue(RootCanvasProperty);
            set => SetValue(RootCanvasProperty, value);
        }


        private WallpaperViewModel _viewModel;

        public WallpaperViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                _viewModel.CoordinateChanged += ViewModelOnCoordinateChanged;
                DataContext = _viewModel;
            }
        }

        private void ViewModelOnCoordinateChanged(object sender, EventArgs eventArgs)
        {
            Canvas.SetTop(this, ViewModel.TopCoordinate);
            Canvas.SetLeft(this, ViewModel.LeftCoordinate);
        }

        public WallpaperControl()
        {
            InitializeComponent();
        }

        private void Thumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var thumb = sender as Thumb;

            if (thumb != null)
            {
                double vertChange = e.VerticalChange;
                double horChange =
                    (ViewModel.Width + vertChange - ViewModel.WallpaperModel.Ratio * ViewModel.Height) /
                    ViewModel.WallpaperModel.Ratio;

                switch (thumb.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        if (ViewModel.Width - horChange < WallpaperViewModel.MinWidth && horChange > 0 || ViewModel.Height - vertChange < WallpaperViewModel.MinHeight && vertChange > 0)
                        {
                            return;
                        }

                        switch (thumb.HorizontalAlignment)
                        {
                            case HorizontalAlignment.Left:
                                if (ViewModel.TopCoordinate + vertChange <= 0 || ViewModel.LeftCoordinate + horChange <= 0)
                                {
                                    return;
                                }

                                ViewModel.TopCoordinate += vertChange;
                                ViewModel.LeftCoordinate += horChange;
                                ViewModel.Width -= horChange;
                                ViewModel.Height -= vertChange;
                                Canvas.SetTop(this, ViewModel.TopCoordinate);
                                Canvas.SetLeft(this, ViewModel.LeftCoordinate);
                                break;
                            case HorizontalAlignment.Right:
                                if (ViewModel.TopCoordinate + vertChange <= 0 || ViewModel.Width + ViewModel.LeftCoordinate - horChange >= RootCanvas.ActualWidth)
                                {
                                    return;
                                }

                                ViewModel.Width -= horChange;
                                ViewModel.Height -= vertChange;
                                ViewModel.TopCoordinate += vertChange;
                                Canvas.SetTop(this, ViewModel.TopCoordinate);
                                break;
                        }
                        break;
                    case VerticalAlignment.Bottom:
                        if (ViewModel.Width + vertChange <= WallpaperViewModel.MinWidth && vertChange < 0 || ViewModel.Height + horChange <= WallpaperViewModel.MinHeight && horChange < 0)
                        {
                            return;
                        }

                        switch (thumb.HorizontalAlignment)
                        {
                            case HorizontalAlignment.Left:
                                if (ViewModel.TopCoordinate + ViewModel.Height + horChange >= RootCanvas.ActualHeight || ViewModel.LeftCoordinate - vertChange <= 0)
                                {
                                    return;
                                }

                                ViewModel.Height += horChange;
                                ViewModel.Width += vertChange;
                                ViewModel.LeftCoordinate -= vertChange;
                                Canvas.SetLeft(this, ViewModel.LeftCoordinate);
                                break;
                            case HorizontalAlignment.Right:
                                if (ViewModel.TopCoordinate + ViewModel.Height + horChange >= RootCanvas.ActualHeight || ViewModel.Width + ViewModel.LeftCoordinate + vertChange >= RootCanvas.ActualWidth)
                                {
                                    return;
                                }

                                ViewModel.Width += vertChange;
                                ViewModel.Height += horChange;
                                break;
                        }
                        break;
                }
                e.Handled = true;
            }
        }

        private void MoveThumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            double horChange = e.HorizontalChange;
            double vertChange = e.VerticalChange;

            if (ViewModel.LeftCoordinate + e.HorizontalChange <= 0 && e.HorizontalChange < 0 || ViewModel.LeftCoordinate + ViewModel.Width + e.HorizontalChange >= RootCanvas.ActualWidth && e.HorizontalChange > 0)
            {
                horChange = 0;
            }

            if (ViewModel.TopCoordinate + e.VerticalChange <= 0 && e.VerticalChange < 0 || ViewModel.TopCoordinate + ViewModel.Height + e.VerticalChange >= RootCanvas.ActualHeight && e.VerticalChange > 0)
            {
                vertChange = 0;
            }

            ViewModel.LeftCoordinate += horChange;
            ViewModel.TopCoordinate += vertChange;
            Canvas.SetLeft(this, ViewModel.LeftCoordinate);
            Canvas.SetTop(this, ViewModel.TopCoordinate);
            e.Handled = true;
        }
    }
}
