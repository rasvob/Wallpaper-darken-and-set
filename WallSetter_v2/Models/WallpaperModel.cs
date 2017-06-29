using System;
using System.Windows.Media.Imaging;
using WallSetter_v2.Annotations;

namespace WallSetter_v2.Models
{
    public class WallpaperModel
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
                RefreshSize();
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

            BitmapImage img = new BitmapImage(new Uri(Path, UriKind.Absolute));
            Width = (int)img.Width;
            Height = (int)img.Height;
        }
    }
}