using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WallSetter_v2.Annotations;
using WallSetter_v2.Commands;
using WallSetter_v2.Models;

namespace WallSetter_v2.ViewModels
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        public WallpaperModel WallpaperModel { get; set; } = new WallpaperModel();

        public ObservableCollection<int> WidthItemSource
        {
            get => _widthItemSource;
            set
            {
                if (Equals(value, _widthItemSource)) return;
                _widthItemSource = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<int> HeightItemSource
        {
            get => _heightItemSource;
            set
            {
                if (Equals(value, _heightItemSource)) return;
                _heightItemSource = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> OpacityItemSource
        {
            get => _opacityItemSource;
            set
            {
                if (Equals(value, _opacityItemSource)) return;
                _opacityItemSource = value;
                OnPropertyChanged();
            }
        }

        public string Opacity
        {
            get => _opacity;
            set
            {
                if (value == _opacity) return;
                _opacity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(OpacityNumber));
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value == _height) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        private bool _useCustomSize;
        private int _height;
        private int _width;
        private string _opacity;
        private ObservableCollection<string> _opacityItemSource = new ObservableCollection<string>();
        private ObservableCollection<int> _widthItemSource = new ObservableCollection<int>();
        private ObservableCollection<int> _heightItemSource = new ObservableCollection<int>();

        private readonly int[] _widthArray = { 1280, 1366, 1440, 1600, 1680, 1920, 2560, 3840, 5760, 3840, 5120 };
        private readonly int[] _heightArray = { 768, 800, 900, 960, 1024, 1200, 1050, 1080, 1200, 1440, 1600, 1080, 2160, 2880 };
        public ICommand SetWallpaperCommand { get; private set; }
        public ICommand LoadFromFile { get; private set; }

        public bool UseCustomSize
        {
            get => _useCustomSize;
            set
            {
                if (value == _useCustomSize) return;
                _useCustomSize = value;
                OnPropertyChanged();
            }
        }

        public int OpacityNumber => int.Parse(Opacity.Substring(0, Opacity.Length - 1));

        public MainWindowViewModel()
        {
            SetWallpaperCommand = new RelayCommand<int>(i => { Debug.WriteLine("ss"); });
            FillOpacityItemSource();
            RefreshWidthSource(1920);
            RefreshHeightSource(1080);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FillOpacityItemSource()
        {
            for (int i = 0; i < 10; i++)
            {
                OpacityItemSource.Add($"{i*10}%");
            }
        }

        public void RefreshWidthSource(int maxWidth)
        {
            WidthItemSource.Clear();
            var items = _widthArray.Where(t => t <= maxWidth).ToList();
            items.Sort();
            foreach (int item in items)
            {
                WidthItemSource.Add(item);
            }
        }

        public void RefreshHeightSource(int maxHeight)
        {
            HeightItemSource.Clear();
            var items = _heightArray.Where(t => t <= maxHeight).ToList();
            items.Sort();
            foreach (int item in items)
            {
                HeightItemSource.Add(item);
            }
        }
    }
}