using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using WallpaperDownloader;
using WallSetter_v2.Annotations;
using WallSetter_v2.Commands;
using WallSetter_v2.Dialogs;
using WallSetter_v2.Dialogs.DownloadLinkDialog;
using WallSetter_v2.EventArgsCustom;
using WallSetter_v2.Services;
using Size = System.Drawing.Size;
using Style = WallpaperManipulator.Style;

namespace WallSetter_v2.ViewModels
{
    public class MainWindowViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly string RootDialogHost = "RootDialog";
        private readonly IOpenFileService _openFileService;

        public double HorizontalScrollOffset
        {
            get => _horizontalScrollOffset;
            set
            {
                if (value.Equals(_horizontalScrollOffset)) return;
                _horizontalScrollOffset = value;
                OnPropertyChanged();
            }
        }

        public double VerticalScrollOffset
        {
            get => _verticalScrollOffset;
            set
            {
                if (value.Equals(_verticalScrollOffset)) return;
                _verticalScrollOffset = value;
                OnPropertyChanged();
            }
        }

        public double ViewportHeight
        {
            get => _viewportHeight;
            set
            {
                if (value.Equals(_viewportHeight)) return;
                _viewportHeight = value;
                OnPropertyChanged();
            }
        }

        public double ViewportWidth
        {
            get => _viewportWidth;
            set
            {
                if (value.Equals(_viewportWidth)) return;
                _viewportWidth = value;
                OnPropertyChanged();
            }
        }

        public double CanvasWidth
        {
            get => _canvasWidth;
            set
            {
                if (value.Equals(_canvasWidth)) return;
                _canvasWidth = value;
                OnPropertyChanged();
            }
        }

        public double CanvasHeight
        {
            get => _canvasHeight;
            set
            {
                if (value.Equals(_canvasHeight)) return;
                _canvasHeight = value;
                OnPropertyChanged();
            }
        }

        public double Top
        {
            get => _top;
            set
            {
                if (value.Equals(_top)) return;
                _top = value;
                OnPropertyChanged();
            }
        }

        public double Left
        {
            get => _left;
            set
            {
                if (value.Equals(_left)) return;
                _left = value;
                OnPropertyChanged();
            }
        }

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

        public double Opacity
        {
            get => _opacity;
            set
            {
                if (value == _opacity) return;
                _opacity = value;
                OnPropertyChanged();
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value == _height) return;
                _height = value;
                CenterCover();
                OnPropertyChanged();
            }
        }

        private void CenterCover()
        {
            Top = (CanvasHeight - Height) / 2;
            Left = (CanvasWidth - Width) / 2;
        }

        public int Width
        {
            get => _width;
            set
            {
                if (value == _width) return;
                _width = value;
                CenterCover();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> WallpaperStyleItemSource
        {
            get => _wallpaperStyleItemSource;
            set
            {
                if (Equals(value, _wallpaperStyleItemSource)) return;
                _wallpaperStyleItemSource = value;
                OnPropertyChanged();
            }
        }

        public string SelectedStyle
        {
            get => _selectedStyle;
            set
            {
                if (value == _selectedStyle) return;
                _selectedStyle = value;
                OnPropertyChanged();
            }
        }

        public Style WallpaperStyle
        {
            get
            {
                switch (SelectedStyle)
                {
                    case "Tiled":
                        return Style.Tiled;
                    case "Centered":
                        return Style.Centered;
                    case "Stretched":
                        return Style.Stretched;
                    default:
                        return Style.Stretched;
                }
            }
        }

        public double Scale
        {
            get => _scale;
            set
            {
                if (value.Equals(_scale)) return;

                var args = new ScaleEventArgs()
                {
                    OldScale = _scale, OldHorizontalOffset = HorizontalScrollOffset, OldVerticalOffset = VerticalScrollOffset, OldViewportHeight = ViewportHeight, OldViewportWidth =  ViewportWidth
                };

                _scale = value;
                WallpaperViewModel.Scale = 1.0 / value;
                OnPropertyChanged();
                OnScaleChanged(args);
            }
        }

        public ISnackbarMessageQueue MessageQueue
        {
            get => _messageQueue;
            set
            {
                if (Equals(value, _messageQueue)) return;
                _messageQueue = value;
                OnPropertyChanged();
            }
        }

        public string SetWallpaperTooltipError
        {
            get => _setWallpaperTooltipError;
            set
            {
                if (value == _setWallpaperTooltipError) return;
                _setWallpaperTooltipError = value;
                OnPropertyChanged();
            }
        }

        private bool _useCustomSize;
        private int _height;
        private int _width;
        private double _opacity = 0;
        private ObservableCollection<string> _opacityItemSource = new ObservableCollection<string>();
        private ObservableCollection<int> _widthItemSource = new ObservableCollection<int>();
        private ObservableCollection<int> _heightItemSource = new ObservableCollection<int>();

        private readonly int[] _widthArray = { 1280, 1366, 1440, 1600, 1680, 1920, 2560, 3840, 5760, 5120 };
        private readonly int[] _heightArray = { 768, 800, 900, 960, 1024, 1200, 1050, 1080, 1440, 1600, 2160, 2880 };
        private double _top;
        private double _left;
        private ObservableCollection<string> _wallpaperStyleItemSource = new ObservableCollection<string>() { "Tiled", "Centered", "Stretched" };
        private string _selectedStyle;
        private double _scale = 0.25;
        private ISnackbarMessageQueue _messageQueue;
        private WallpaperViewModel _wallpaperViewModel = new WallpaperViewModel();
        private double _canvasWidth;
        private double _canvasHeight;
        private double _horizontalScrollOffset;
        private double _verticalScrollOffset;
        private double _viewportHeight;
        private double _viewportWidth;
        private string _setWallpaperTooltipError = String.Empty;

        public ICommand SetWallpaperCommand { get; }
        public ICommand LoadFromFileCommand { get; }
        public ICommand DownloadFromWallhavenCommand { get; }
        public ICommand DownloadFromUnsplashCommand { get; }
        public ICommand DownloadFromLinkCommand { get; }
        public ICommand SetDefaultSizesCommand { get; set; }
        public ICommand SetCurrentScreenSizeCommand { get; set; }

        public WallpaperViewModel WallpaperViewModel
        {
            get => _wallpaperViewModel;
            set
            {
                if (Equals(value, _wallpaperViewModel)) return;
                _wallpaperViewModel = value;
                OnPropertyChanged();
            }
        }

        public bool UseCustomSize
        {
            get => _useCustomSize;
            set
            {
                if (value == _useCustomSize) return;
                _useCustomSize = value;

                if (!UseCustomSize)
                {
                    Height = WallpaperViewModel.WallpaperModel.Height;
                    Width = WallpaperViewModel.WallpaperModel.Width;
                }

                OnPropertyChanged();
            }
        }

        public MainWindowViewModel(IOpenFileService openFileService)
        {
            _openFileService = openFileService;

            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2));
            SetWallpaperCommand = new SimpleCommand(SetWallpaperExecute, SetWallpaperCanExecute);
            LoadFromFileCommand = new SimpleCommand(LoadFromFileExecute);
            DownloadFromWallhavenCommand = new SimpleCommand(DownloadFromWallhavenExecute);
            DownloadFromUnsplashCommand = new SimpleCommand(DownloadFromUnsplashExecute);
            DownloadFromLinkCommand = new SimpleCommand(DownloadFromLinkExecute);
            SetDefaultSizesCommand = new SimpleCommand(SetDefaultSizesCommandExecute, SetDefaultSizesCommandCanExecute);
            SetCurrentScreenSizeCommand = new SimpleCommand(SetCurrentScreenSizeCommandExecute, SetCurrentScreenSizeCommandCanExecute);

            FillOpacityItemSource();
            RefreshHeightSource(int.MaxValue);
            RefreshWidthSource(int.MaxValue);

            WallpaperViewModel.IsVisible = Visibility.Hidden;
        }

        private bool SetCurrentScreenSizeCommandCanExecute(object o)
        {
            return WallpaperViewModel.WallpaperModel.Path != null;
        }

        private void SetCurrentScreenSizeCommandExecute(object o)
        {
            if (CanvasWidth <= SystemParameters.VirtualScreenWidth)
            {
                CanvasWidth = SystemParameters.VirtualScreenWidth * 2;
            }

            if (CanvasHeight <= SystemParameters.VirtualScreenHeight)
            {
                CanvasHeight = SystemParameters.VirtualScreenHeight * 2;
            }

            UseCustomSize = true;
            Width = (int) SystemParameters.VirtualScreenWidth;
            Height = (int) SystemParameters.VirtualScreenHeight;
        }

        private bool SetDefaultSizesCommandCanExecute(object o)
        {
            return WallpaperViewModel.WallpaperModel.Path != null;
        }

        private void SetDefaultSizesCommandExecute(object o)
        {
            UseCustomSize = false;
            WallpaperViewModel.SetDefaultSize((CanvasHeight - WallpaperViewModel.WallpaperModel.Height)/2, (CanvasWidth - WallpaperViewModel.WallpaperModel.Width)/2);
        }

        private async void DownloadFromLinkExecute(object o)
        {
            
        }

        private async void DownloadFromUnsplashExecute(object o)
        {
            
        }

        private async void DownloadFromWallhavenExecute(object o)
        {
            await DownloadWithDialog("Download from Wallhaven.cc", DownloaderType.Wallhaven);
        }

        private async Task DownloadWithDialog(string title, DownloaderType downloaderType)
        {
            DownloadDialogViewModel dialogViewModel = new DownloadDialogViewModel()
            {
                DialogTitle = title
            };

            var dialog = new DownloadDialog()
            {
                DataContext = dialogViewModel
            };

            var res = await DialogHost.Show(dialog, RootDialogHost);

            if ((bool)res)
            {
                string err = null;
                await Task.Factory.StartNew(
                    () =>
                    {
                        
                    });

                await DialogHelper.ShowProgressDialog("Please wait", "Download in progress...",
                    () =>
                    {
                        try
                        {
                            WallpaperViewModel.WallpaperModel.DownloadWallpaper(downloaderType, dialogViewModel.Link);
                        }
                        catch (Exception e)
                        {
                            err = e.Message;
                        }
                    }, RootDialogHost);

                if (err != null)
                {
                    await DialogHelper.ShowMessage("Error occurred", err, RootDialogHost);
                    return;
                }

                LoadFile(WallpaperViewModel.WallpaperModel.Path);
            }
        }

        private void LoadFromFileExecute(object _)
        {
            var file = _openFileService.OpenFile();
            if (file == null) return;
            LoadFile(file);
            MessageQueue.Enqueue($"{file} loaded", "Hide", () => { });
        }

        private void LoadFile(string path)
        {
            WallpaperViewModel.WallpaperModel.Path = path;
            Size size = WallpaperViewModel.WallpaperModel.RefreshSize();
            WallpaperViewModel.Width = size.Width;
            WallpaperViewModel.Height = size.Height;

            MemoryStream memoryStream = new MemoryStream();
            using (FileStream fs = File.OpenRead(path))
            {
                fs.CopyTo(memoryStream);
                WallpaperViewModel.WallpaperModel.Stream = memoryStream;
                memoryStream.Seek(0, SeekOrigin.Begin);
            }

            CanvasWidth = WallpaperViewModel.WallpaperModel.Width * 2;
            CanvasHeight = WallpaperViewModel.WallpaperModel.Height * 2;
            WallpaperViewModel.TopCoordinate = (CanvasHeight - WallpaperViewModel.Height) / 2;
            WallpaperViewModel.LeftCoordinate = (CanvasWidth - WallpaperViewModel.Width) / 2;
            WallpaperViewModel.UpdateCoordinates();
            WallpaperViewModel.IsVisible = Visibility.Visible;
            Height = WallpaperViewModel.WallpaperModel.Height;
            Width = WallpaperViewModel.WallpaperModel.Width;
            OnWallpaperLoaded();
        }

        private void SetWallpaperExecute(object _)
        {
           
        }

        private bool SetWallpaperCanExecute(object _)
        {
            SetWallpaperTooltipError = string.Empty;

            if (this[nameof(Width)] != string.Empty || this[nameof(Height)] != string.Empty)
            {
                return false;
            }

            if (WallpaperViewModel.WallpaperModel.Path == null)
            {
                return false;
            }

            if (SelectedStyle == null)
            {
                return false;
            }

            if (!IsWallpaperSetupOkay())
            {
                SetWallpaperTooltipError = "Wallpaper doesn't fit the desired dimensions";
                return false;
            }

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler WallpaperLoaded;
        public event EventHandler<ScaleEventArgs> ScaleChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsWallpaperSetupOkay()
        {
            Rectangle coverSize = new Rectangle((int) Left, (int) Top, Width, Height);
            Rectangle imgSize = new Rectangle((int) WallpaperViewModel.LeftCoordinate, (int)WallpaperViewModel.TopCoordinate, (int) WallpaperViewModel.Width, (int) WallpaperViewModel.Height);
            return imgSize.Contains(coverSize);
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
            var items = _widthArray.ToList();
            items.Sort();
            foreach (int item in items)
            {
                WidthItemSource.Add(item);
            }
        }

        public void RefreshHeightSource(int maxHeight)
        {
            HeightItemSource.Clear();
            var items = _heightArray.ToList();
            items.Sort();
            foreach (int item in items)
            {
                HeightItemSource.Add(item);
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Width):
                        if (!WallpaperViewModel.WallpaperModel.ValidateWidth(Width))
                        {
                            return $"{nameof(Width)} must be greater than 0";
                        }
                        break;
                    case nameof(Height):
                        if (!WallpaperViewModel.WallpaperModel.ValidateHeight(Height))
                        {
                            return $"{nameof(Height)} must be greater than 0";
                        }
                        break;
                }
                return string.Empty;
            }
        }

        public string Error => string.Empty;

        protected virtual void OnWallpaperLoaded()
        {
            WallpaperLoaded?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnScaleChanged(ScaleEventArgs e)
        {
            ScaleChanged?.Invoke(this, e);
        }
    }
}