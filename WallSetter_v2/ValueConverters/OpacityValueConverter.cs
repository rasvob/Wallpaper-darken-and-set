using System;
using System.Globalization;
using System.Web;
using System.Windows.Data;

namespace WallSetter_v2.ValueConverters
{
    public class OpacityValueConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var op = (double) value;
            return $"{op}%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var op = value as string;
            return double.Parse(op?.Substring(0, op.Length - 1))/100;
        }
    }
}