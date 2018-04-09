using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Event_ECS_WPF.Converters
{
    public class EnumToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && value.GetType().IsEnum)
            {
                var enumVal = Enum.Parse(value.GetType(), parameter?.ToString() ?? string.Empty);
                return Enum.Equals(value, enumVal);
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool && (bool)value)
            {
                return parameter;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
