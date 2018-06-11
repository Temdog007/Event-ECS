using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Event_ECS_WPF.Converters
{
    public class ItemToItemIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length == 2 && values[0] is IList list && values[1] is object item)
            {
                int index = list.IndexOf(item);
                if(parameter is Array arr)
                {
                    foreach(object obj in arr)
                    {
                        if(int.TryParse(obj.ToString(), out int result))
                        {
                            index += result;
                        }
                        if(obj is string format)
                        {
                            return string.Format(format, index);
                        }
                    }
                }
                else if (parameter is string format)
                {
                    return string.Format(format, index);
                }
                else
                {
                    return index;
                }
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
