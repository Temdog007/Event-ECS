using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Event_ECS_WPF.Converters
{
    class ColorIndexToNameConverter : IValueConverter
    {
        private static readonly Dictionary<int, string> colors = new Dictionary<int, string>() { { 1, "Red" }, { 2,"Green" }, { 3,"Blue" }, { 4,"Alpha" } };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = -1;
            if (value is string str)
            {
                int.TryParse(str, out index);
            }
            else if (value is int)
            {
                index = (int)value;
            }
            else if(value is float)
            {
                index = System.Convert.ToInt32(value);
            }

            if(colors.TryGetValue(index, out string color))
            {
                return color;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string color)
            {
                foreach(var pair in colors)
                {
                    if(pair.Value == color)
                    {
                        return pair.Key;
                    }
                }
            }
            return null;
        }
    }
}
