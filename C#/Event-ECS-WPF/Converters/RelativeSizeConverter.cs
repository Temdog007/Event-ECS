using System;
using System.Globalization;
using System.Windows.Data;

namespace Event_ECS_WPF.Converters
{
    public class RelativeSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(double.TryParse(parameter.ToString(), out double relative) && double.TryParse(value.ToString(), out double actual))
            {
                return (relative * actual) / 100;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
