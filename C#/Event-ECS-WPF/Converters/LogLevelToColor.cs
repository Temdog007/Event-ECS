using Event_ECS_WPF.Logger;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Event_ECS_WPF.Converters
{
    public class LogLevelToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is LogLevel)
            {
                switch((LogLevel)value)
                {
                    case LogLevel.High:
                        return Brushes.Red;
                    case LogLevel.Medium:
                        return Brushes.Yellow;
                    default:
                        return Brushes.White;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Color)
            {
                if (value.Equals(Colors.LightCoral))
                {
                    return LogLevel.High;
                }
                else if(value.Equals(Colors.LightYellow))
                {
                    return LogLevel.Medium;
                }
                else
                {
                    return Colors.White;
                }
            }
            return null;
        }
    }
}
