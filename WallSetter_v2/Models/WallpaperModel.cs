﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using WallpaperDownloader;
using WallSetter_v2.Annotations;

namespace WallSetter_v2.Models
{
    public class WallpaperModel: INotifyPropertyChanged
    {
        private string _path;
        private MemoryStream _stream;

        public int Width { get; set; }
        public int Height { get; set; }
        public double Ratio => (double)Width / Height;
        
        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }

        public MemoryStream Stream
        {
            get => _stream;
            set
            {
                if (Equals(value, _stream)) return;
                _stream = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler SizeChanged;

        protected virtual void OnSizeChanged()
        {
            SizeChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool ValidateWidth(int width)
        {
            return width <= Width;
        }

        public bool ValidateHeight(int height)
        {
            return height <= Height;
        }

        public Size RefreshSize()
        {
            if (Path ==  null)
            {
                throw new ApplicationException("Path can't be null");
            }
            BitmapImage img = new BitmapImage(new Uri(Path, UriKind.Absolute));
            Width = (int)img.Width;
            Height = (int)img.Height;
            return new Size(Width, Height);
        }

        public void DownloadWallpaper(DownloaderType type, string url)
        {
            IWallpaperDownloader downloader = WallpaperDownloaderFactory.CreateDownloaderInstance(type, url);
            (string path, MemoryStream stream) wallpaper = downloader.DownloadWallpaper();
            Path = wallpaper.path;
            Stream = wallpaper.stream;
            RefreshSize();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}