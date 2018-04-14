using System;
using System.Collections.Generic;
using System.Reflection;

namespace Event_ECS_WPF.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<string> Split(this string str, uint length)
        {
            for(uint i = 0, n = (uint)str.Length; i < n; i += length )
            {
                if(i + length >= n)
                {
                    yield return str.Substring((int)i);
                }
                else
                {
                    yield return str.Substring((int)i, (int)Math.Min(length, n - 1));
                }
            }
        }

        public static PropertyInfo GetProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName);
        }

        public static T GetProperty<T>(this object obj, string propertyName, T value)
        {
            return (T)obj.GetProperty(propertyName).GetValue(obj);
        }

        public static void SetProperty<T>(this object obj, string propertyName, T value)
        {
            obj.GetProperty(propertyName).SetValue(obj, value);
        }

        public static System.Windows.Input.Key Convert(this System.Windows.Forms.Keys key)
        {
            try
            {
                return (System.Windows.Input.Key)Enum.Parse(typeof(System.Windows.Input.Key), key.ToString());
            }
            catch(Exception)
            {
                return System.Windows.Input.Key.None;
            }
        }
    }
}
