using System;
using System.Globalization;
using System.Windows.Data;

namespace WallSetter_v2.ValueConverters
{
    public class ScaleValueConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double scale = (double) value;
            return 1.0 / scale;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}