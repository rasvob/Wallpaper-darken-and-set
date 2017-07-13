using System;
using System.Globalization;
using System.Windows.Data;

namespace WallSetter_v2.ValueConverters
{
    public class BorderThicknessValueConverter: IValueConverter
    {
        private static double BorderThickness = 2;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double scale = (double) value;
            return (1.0 / scale) * BorderThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}