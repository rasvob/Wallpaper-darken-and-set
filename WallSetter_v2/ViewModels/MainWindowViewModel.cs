﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using WallpaperDownloader;
using WallpaperManipulator;
using WallSetter_v2.Annotations;
using WallSetter_v2.Commands;
using WallSetter_v2.Models;
using WallSetter_v2.Services;

namespace WallSetter_v2.ViewModels
{
    public class MainWindowViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly IOpenFileService _openFileService;

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

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (value == _imagePath) return;
                _imagePath = value;
                OnPropertyChanged();
            }
        }

        public double Scale
        {
            get => _scale;
            set
            {
                if (value.Equals(_scale)) return;
                _scale = value;
                OnPropertyChanged();
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

        private bool _useCustomSize;
        private int _height = 768;
        private int _width = 1280;
        private double _opacity = 0;
        private ObservableCollection<string> _opacityItemSource = new ObservableCollection<string>();
        private ObservableCollection<int> _widthItemSource = new ObservableCollection<int>();
        private ObservableCollection<int> _heightItemSource = new ObservableCollection<int>();

        private readonly int[] _widthArray = { 1280, 1366, 1440, 1600, 1680, 1920, 2560, 3840, 5760, 3840, 5120 };
        private readonly int[] _heightArray = { 768, 800, 900, 960, 1024, 1200, 1050, 1080, 1440, 1600, 2160, 2880 };
        private double _top;
        private double _left;
        private ObservableCollection<string> _wallpaperStyleItemSource = new ObservableCollection<string>() { "Tiled", "Centered", "Stretched" };
        private string _selectedStyle;
        private string _imagePath;
        private double _scale = 1;
        private ISnackbarMessageQueue _messageQueue;

        public ICommand SetWallpaperCommand { get; }
        public ICommand LoadFromFileCommand { get; }
        public ICommand DownloadFromWallhavenCommand { get; }
        public ICommand DownloadFromUnsplashCommand { get; }
        public ICommand DownloadFromLinkCommand { get; }

        public WallpaperViewModel WallpaperViewModel { get; set; } = new WallpaperViewModel();

        public bool UseCustomSize
        {
            get => _useCustomSize;
            set
            {
                if (value == _useCustomSize) return;
                _useCustomSize = value;

                if (!UseCustomSize)
                {
                    Width = WallpaperViewModel.WallpaperModel.Width;
                    Height = WallpaperViewModel.WallpaperModel.Height;
                    Top = Left = 0;
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

            FillOpacityItemSource();
            //WallpaperViewModel.WallpaperModel.SizeChanged += OnWallpaperModelOnSizeChanged;
        }

        private void DownloadFromLinkExecute(object o)
        {
            
        }

        private void DownloadFromUnsplashExecute(object o)
        {
            
        }

        private void DownloadFromWallhavenExecute(object o)
        {
            var downloader = WallpaperDownloaderFactory.CreateDownloaderInstance(DownloaderType.Wallhaven, @"https://alpha.wallhaven.cc/wallpaper/512644");
            (string path, MemoryStream stream) wallpaper = downloader.DownloadWallpaper();
            ImagePath = wallpaper.path;
            
        }

        private void OnWallpaperModelOnSizeChanged(object sender, EventArgs args)
        {
            RefreshHeightSource(WallpaperViewModel.WallpaperModel.Height);
            RefreshWidthSource(WallpaperViewModel.WallpaperModel.Width);
            Top = Left = 0;

            if (!UseCustomSize)
            {
                Height = WallpaperViewModel.WallpaperModel.Height;
                Width = WallpaperViewModel.WallpaperModel.Width;
                Top = Left = 0;
            }
            else
            {
                if (Height > WallpaperViewModel.WallpaperModel.Height)
                {
                    Height = WallpaperViewModel.WallpaperModel.Height;
                    Top = 0;
                }

                if (Width > WallpaperViewModel.WallpaperModel.Width)
                {
                    Width = WallpaperViewModel.WallpaperModel.Width;
                    Left = 0;
                }
            }
        }

        private void LoadFromFileExecute(object _)
        {
            var file = _openFileService.OpenFile();

            if (file != null)
            {
                WallpaperViewModel.WallpaperModel.Path = file;
                MemoryStream memoryStream = new MemoryStream();
                using (FileStream fs = File.OpenRead(file))
                {
                    fs.CopyTo(memoryStream);
                    WallpaperViewModel.WallpaperModel.Stream = memoryStream;
                    memoryStream.Seek(0, SeekOrigin.Begin);
                }

                MessageQueue.Enqueue($"{file} loaded", "Hide", () => { });
            }
        }

        private void SetWallpaperExecute(object _)
        {
            Debug.WriteLine(WallpaperStyle);
        }

        private bool SetWallpaperCanExecute(object _)
        {
            if (this[nameof(Width)] != string.Empty || this[nameof(Height)] != string.Empty)
            {
                return false;
            }

            if (SelectedStyle == null)
            {
                return false;
            }

            return true;
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
            //var items = _widthArray.Where(t => t <= maxWidth).ToList();
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
            //var items = _heightArray.Where(t => t <= maxHeight).ToList();
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
                            return $"{nameof(Width)} must be between 0 and {WallpaperViewModel.WallpaperModel.Width}";
                        }
                        break;
                    case nameof(Height):
                        if (!WallpaperViewModel.WallpaperModel.ValidateHeight(Height))
                        {
                            return $"{nameof(Height)} must be between 0 and {WallpaperViewModel.WallpaperModel.Height}";
                        }
                        break;
                }
                return string.Empty;
            }
        }

        public string Error => string.Empty;

        public void ValidateAndSetupChange(ref double hc, ref double vc)
        {
            if (vc + Top < 0 || vc + Top + Height > WallpaperViewModel.WallpaperModel.Height)
            {
                vc = 0;
            }

            if (hc + Left < 0 || hc + Left + Width > WallpaperViewModel.WallpaperModel.Width)
            {
                hc = 0;
            }
        }
    }
}