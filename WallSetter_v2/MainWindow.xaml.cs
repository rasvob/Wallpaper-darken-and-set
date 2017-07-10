using System.Windows;
using System.Windows.Input;
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
            DataContext = ViewModel;
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
