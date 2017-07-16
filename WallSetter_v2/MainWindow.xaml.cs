using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WallSetter_v2.EventArgsCustom;
using WallSetter_v2.Services;
using WallSetter_v2.ViewModels;

namespace WallSetter_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel(new OpenFileFromDialog());
            ViewModel.ScaleChanged += ViewModelOnScaleChanged;
            WallpaperControl.ViewModel = ViewModel.WallpaperViewModel;
            DataContext = ViewModel;
        }

        private void ViewModelOnScaleChanged(object sender, ScaleEventArgs args)
        {
            double contentHorizontalMiddle = (args.OldHorizontalOffset + args.OldViewportWidth / 2) / args.OldScale;
            double contentVerticalMiddle = (args.OldVerticalOffset + args.OldViewportHeight / 2) / args.OldScale;

            if ((int)ScrollViewer.ScrollableWidth == 0)
            {
                contentHorizontalMiddle = ViewModel.CanvasWidth / 2;
            }

            if ((int)ScrollViewer.ScrollableHeight == 0)
            {
                contentVerticalMiddle = ViewModel.CanvasHeight / 2;
            }

            double newContentHorizontalOffset = contentHorizontalMiddle - (ViewModel.ViewportWidth / 2) / ViewModel.Scale;
            double newContentVerticalOffset = contentVerticalMiddle - (ViewModel.ViewportHeight / 2) / ViewModel.Scale;

            double newHorizontalOffset = newContentHorizontalOffset * ViewModel.Scale;
            double newVerticalOffset = newContentVerticalOffset * ViewModel.Scale;

            ScrollViewer.ScrollToHorizontalOffset(newHorizontalOffset);
            ScrollViewer.ScrollToVerticalOffset(newVerticalOffset);
        }

        private void ScrollViewer_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset + e.Delta);
                e.Handled = true;
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                double ns = (e.Delta > 0 ? 0.05 : -0.05) + ViewModel.Scale;
                if (ns <= MainWindowViewModel.MinScale || ns >= MainWindowViewModel.MaxScale)
                {
                    ns = ViewModel.Scale;
                }
                ViewModel.Scale = ns;
                e.Handled = true;
            }
        }

        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ViewModel.ViewportHeight = e.ViewportHeight;
            ViewModel.ViewportWidth = e.ViewportWidth;
            ViewModel.HorizontalScrollOffset = e.HorizontalOffset;
            ViewModel.VerticalScrollOffset = e.VerticalOffset;
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => ViewModel.GetDefaultWallpaperPath());
        }
    }
}
