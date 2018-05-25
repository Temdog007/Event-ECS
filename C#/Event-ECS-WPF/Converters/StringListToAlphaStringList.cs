using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Event_ECS_WPF.Converters
{
    public class StringListToAlphaStringList : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is IEnumerable<string> strs && parameter is string key)
            {
                return strs.Select(s => s.StartsWith(key, StringComparison.OrdinalIgnoreCase));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
