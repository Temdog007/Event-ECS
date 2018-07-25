using Event_ECS_WPF.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Event_ECS_WPF.Converters
{
    public class ColorIndexToBackgroundConverter : IMultiValueConverter
    {
        public const int Alpha = MaxColor << 24;

        public const int Blue = MaxColor;

        public const int Green = MaxColor << 8;

        public const byte MaxColor = 255;

        public const int Red = MaxColor << 16;

        public const int White = Red | Blue | Green | Alpha;

        private static ColorIndexToBackgroundConverter s_instance;

        public static ColorIndexToBackgroundConverter Instance => s_instance ?? (s_instance = new ColorIndexToBackgroundConverter());
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is string key && values[1] is float v)
            {
                int.TryParse(key, out int index);

                int color;
                switch (index)
                {
                    case 1:
                        color = System.Convert.ToInt32(v * MaxColor) << 16 | Alpha;
                        break;
                    case 2:
                        color = System.Convert.ToInt32(v * MaxColor) << 8 | Alpha;
                        break;
                    case 3:
                        color = System.Convert.ToInt32(v * MaxColor) | Alpha;
                        break;
                    default:
                        color = White;
                        break;
                }
                return color.ToBrush();
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
