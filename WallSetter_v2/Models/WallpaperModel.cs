using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using WallpaperDownloader;
using WallSetter_v2.Annotations;

namespace WallSetter_v2.Models
{
    public class WallpaperModel: INotifyPropertyChanged
    {
        private int _width;
        private int _height;
        private string _path;

        public int Width
        {
            get => _width;
            set
            {
                _width = value; 
                OnSizeChanged();
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                OnSizeChanged();
            }
        }

        public string Path
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged();
                RefreshSize();
            }
        }

        public MemoryStream Stream { get; set; }

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

        public void RefreshSize()
        {
            if (Path ==  null)
            {
                return;
            }

            BitmapImage img = new BitmapImage(new Uri(Path, UriKind.Absolute));
            Width = (int)img.Width;
            Height = (int)img.Height;
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