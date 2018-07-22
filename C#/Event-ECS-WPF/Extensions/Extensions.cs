using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Event_ECS_WPF.Extensions
{
    public static class Extensions
    {
        public static IReadOnlyList<T> AsReadOnly<T>(this IEnumerable<T> list)
        {
            return list.ToList().AsReadOnly();
        }

        public static System.Windows.Input.Key Convert(this System.Windows.Forms.Keys key)
        {
            try
            {
                return (System.Windows.Input.Key)Enum.Parse(typeof(System.Windows.Input.Key), key.ToString());
            }
            catch (Exception)
            {
                return System.Windows.Input.Key.None;
            }
        }

        public static T Copy<T>(this T list) where T : IList
        {
            return (T)Activator.CreateInstance(typeof(T), list);
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj != null)
            {
                for (int i = 0, n = VisualTreeHelper.GetChildrenCount(obj); i < n; ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null)
                    {
                        return childItem;
                    }
                }
            }
            return null;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Func<T, T> action)
        {
            foreach (T t in list)
            {
                yield return action(t);
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

        public static string GetResourceFileContents(this string resourcename)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourcename))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsHidden(this string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return ((attr & FileAttributes.Hidden) == FileAttributes.Hidden);
        }

        public static void SetProperty<T>(this object obj, string propertyName, T value)
        {
            obj.GetProperty(propertyName).SetValue(obj, value);
        }

        public static IEnumerable<string> Split(this string str, uint length)
        {
            for (uint i = 0, n = (uint)str.Length; i < n; i += length)
            {
                if (i + length >= n)
                {
                    yield return str.Substring((int)i);
                }
                else
                {
                    yield return str.Substring((int)i, (int)Math.Min(length, n - 1));
                }
            }
        }

        public static IEnumerable<T> SubArray<T>(this IList<T> list, int start)
        {
            return SubArray(list, start, list.Count);
        }

        public static IEnumerable<T> SubArray<T>(this IList<T> list, int start, int end)
        {
            for (int i = start; i < end; ++i)
            {
                yield return list[i];
            }
        }

        public static bool Contains(this string str1, string str2, StringComparison comp)
        {
            if(str1.Contains(str2))
            {
                return true;
            }

            return str1.IndexOf(str2, comp) >= 0;
        }
    }
}
