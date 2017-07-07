using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media;
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
        private ImageSource _bitmapImage;
        private MemoryStream _stream;

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
                BitmapImage = new BitmapImage(new Uri(value, UriKind.Absolute));
                OnPropertyChanged();
                RefreshSize();
            }
        }

        public ImageSource BitmapImage
        {
            get => _bitmapImage;
            set
            {
                if (Equals(value, _bitmapImage)) return;
                _bitmapImage = value;
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

        public void RefreshSize()
        {
            if (Path ==  null)
            {
                return;
            }

            Width = (int)BitmapImage.Width;
            Height = (int)BitmapImage.Height;
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