using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Event_ECS_WPF.Converters
{
    public class EnabledToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool)
            {
                return (bool)value ? Brushes.Lime : Brushes.LightPink;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
