﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using WallSetter_v2.Annotations;
using WallSetter_v2.Models;

namespace WallSetter_v2.ViewModels
{
    public class WallpaperViewModel: INotifyPropertyChanged
    {
        private double _width;
        private double _height;
        private double _scale = 1;
        private WallpaperModel _wallpaperModel = new WallpaperModel();
        private double _topCoordinate;
        private double _leftCoordinate;

        public double TopCoordinate
        {
            get => _topCoordinate;
            set
            {
                if (value.Equals(_topCoordinate)) return;
                _topCoordinate = value;
                OnCoordinateChanged();
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
                OnCoordinateChanged();
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