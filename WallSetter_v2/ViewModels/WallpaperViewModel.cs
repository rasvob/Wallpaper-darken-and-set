using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using WallSetter_v2.Annotations;
using WallSetter_v2.Models;
using WallSetter_v2.ValueConverters;

namespace WallSetter_v2.ViewModels
{
    public class WallpaperViewModel: INotifyPropertyChanged
    {
        private double _width;
        private double _height;
        private double _scale = 4;
        private WallpaperModel _wallpaperModel = new WallpaperModel();
        private double _topCoordinate;
        private double _leftCoordinate;
        private Visibility _isVisible = Visibility.Collapsed;

        public static readonly double MinWidth = 800;
        public static readonly double MinHeight = 600;

        public Visibility IsVisible
        {
            get => _isVisible;
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public double TopCoordinate
        {
            get => _topCoordinate;
            set
            {
                if (value.Equals(_topCoordinate)) return;
                _topCoordinate = value;
                OnPropertyChanged();
            }
        }

        public double LeftCoordinate
        {
            get => _leftCoordinate;
            set
            {
                if (value.Equals(_leftCoordinate)) return;
                _leftCoordinate = value;
                OnPropertyChanged();
            }
        }

        public WallpaperModel WallpaperModel
        {
            get => _wallpaperModel;
            set
            {
                if (Equals(value, _wallpaperModel)) return;
                _wallpaperModel = value;
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

        public double Width
        {
            get => _width;
            set
            {
                if (value.Equals(_width)) return;
                _width = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NewSizeInPercent));
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

        public string NewSizeInPercent
        {
            get
            {
                if (WallpaperModel.Width == 0)
                {
                    return string.Empty;
                }
                double val = Width * 100.0 / WallpaperModel.Width;
                var formattable = $"({(int)val} %)";
                return formattable;
            }
        }

        public WallpaperViewModel()
        {

        }

        public void UpdateCoordinates()
        {
            OnCoordinateChanged();
        }

        public void SetDefaultSize(double top, double left)
        {
            Width = WallpaperModel.Width;
            Height = WallpaperModel.Height;
            TopCoordinate = top;
            LeftCoordinate = left;
            UpdateCoordinates();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CoordinateChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnCoordinateChanged()
        {
            CoordinateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}