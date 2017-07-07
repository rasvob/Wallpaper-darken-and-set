using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
            WallpaperControl.ViewModel = ViewModel.WallpaperViewModel;
            ViewModel.PropertyChanged += (sender, args) =>
            {
                //if (args.PropertyName.Equals("ImagePath"))
                //{
                //    WallpaperControl.WallpaperImage.Source = new BitmapImage(new Uri(ViewModel.ImagePath));
                //}
            };
            DataContext = ViewModel;
        }

        private void ThumbCover_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            double vc = e.VerticalChange;
            double hc = e.HorizontalChange;
            ViewModel.ValidateAndSetupChange(ref hc, ref vc);
            ViewModel.Top += vc;
            ViewModel.Left += hc;
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
                if (ns <= 0 || ns >= 2)
                {
                    ns = ViewModel.Scale;
                }
                ViewModel.Scale = ns;
                e.Handled = true;
            }
        }
    }
}
