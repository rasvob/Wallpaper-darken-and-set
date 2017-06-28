using System;

namespace WallSetter_v2.Models
{
    public class WallpaperModel
    {
        private int _width;
        private int _height;

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

        public string Path { get; set; }

        public event EventHandler SizeChanged;

        protected virtual void OnSizeChanged()
        {
            SizeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}