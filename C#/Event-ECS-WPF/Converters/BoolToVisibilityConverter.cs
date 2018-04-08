using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Event_ECS_WPF.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool)
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Visibility)
            {
                switch((Visibility)value)
                {
                    case Visibility.Collapsed:
                        return false;
                    case Visibility.Hidden:
                        break;
                    case Visibility.Visible:
                        return true;
                }
            }

            return null;
        }
    }
}
