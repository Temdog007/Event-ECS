using Event_ECS_WPF.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Event_ECS_WPF.Converters
{
    public class ColorIndexToForegroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(ColorIndexToBackgroundConverter.Instance.Convert(values, targetType, parameter, culture) is SolidColorBrush brush)
            {
                return new SolidColorBrush(brush.Color.Invert());
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
