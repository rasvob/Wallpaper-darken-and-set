using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WallSetter_v2.Annotations;
using WallSetter_v2.Models;

namespace WallSetter_v2.ViewModels
{
    public class WallpaperViewModel:INotifyPropertyChanged
    {
        private double _left;
        private double _top;
        private double _width;
        private double _height;
        private double _scale;
        public WallpaperModel WallpaperModel { get; set; } = new WallpaperModel();

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

        public double Width
        {
            get => _width;
            set
            {
                if (value.Equals(_width)) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                if (value.Equals(_height)) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public WallpaperViewModel()
        {
            WallpaperModel.SizeChanged += WallpaperModelOnSizeChanged;
        }

        private void WallpaperModelOnSizeChanged(object sender, EventArgs eventArgs)
        {
            Width = WallpaperModel.Width;
            Height = WallpaperModel.Height;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}