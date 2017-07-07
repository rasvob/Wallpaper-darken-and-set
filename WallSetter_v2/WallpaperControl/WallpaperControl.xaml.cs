using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WallSetter_v2.ViewModels;

namespace WallSetter_v2.WallpaperControl
{
    /// <summary>
    /// Interaction logic for WallpaperControl.xaml
    /// </summary>
    public partial class WallpaperControl : UserControl
    {
        private WallpaperViewModel _viewModel;

        public WallpaperViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                DataContext = _viewModel;
            }
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
                double change = e.VerticalChange;
                switch (thumb.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        switch (thumb.HorizontalAlignment)
                        {
                            case HorizontalAlignment.Left:
                                ViewModel.Top += change;
                                ViewModel.Left += change;
                                break;
                            case HorizontalAlignment.Right:
                                ViewModel.Width += change;
                                ViewModel.Top += change;
                                break;
                        }
                        break;
                    case VerticalAlignment.Bottom:
                        switch (thumb.HorizontalAlignment)
                        {
                            case HorizontalAlignment.Left:
                                ViewModel.Height += change;
                                ViewModel.Left -= change;
                                break;
                            case HorizontalAlignment.Right:
                                ViewModel.Width += change;
                                ViewModel.Height += change;
                                break;
                        }
                        break;
                }
            }
        }
    }
}
