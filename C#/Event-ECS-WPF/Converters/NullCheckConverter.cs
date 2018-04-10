using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Event_ECS_WPF.Converters
{
    public class NullCheckConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.All(v => (v is bool && (bool)v) || (v != null));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
