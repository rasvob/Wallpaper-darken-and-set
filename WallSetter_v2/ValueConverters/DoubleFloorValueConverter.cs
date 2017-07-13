using System;
using System.Globalization;
using System.Windows.Data;

namespace WallSetter_v2.ValueConverters
{
    public class DoubleFloorValueConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0;
            double val = (double) value;
            if (parameter == null) return 0;
            return int.TryParse(parameter as string, out int digits) ? Math.Round(val, digits) : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}