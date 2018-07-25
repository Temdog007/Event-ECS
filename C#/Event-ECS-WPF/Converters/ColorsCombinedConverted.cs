using Event_ECS_WPF.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Event_ECS_WPF.Converters
{
    public class ColorsCombinedConverted : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 4 && values[0] is float r && values[1] is float g && values[2] is float b && values[3] is float a)
            {
                int color = System.Convert.ToInt32(ColorIndexToBackgroundConverter.Red * r) |
                    System.Convert.ToInt32(ColorIndexToBackgroundConverter.Green * g) |
                    System.Convert.ToInt32(ColorIndexToBackgroundConverter.Blue * b) |
                    System.Convert.ToInt32(ColorIndexToBackgroundConverter.Alpha * a);
                return new SolidColorBrush(color.ToBrush().Color);
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
