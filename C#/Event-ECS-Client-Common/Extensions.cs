using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Event_ECS_Client_Common
{
    public static class Extensions
    {
        public static T GetProperty<T>(this object obj, string propName)
        {
            return (T)obj.GetType().GetProperty(propName).GetValue(obj);
        }

        public static T GetProperty<T>(this object obj)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(T))
                {
                    return (T)prop.GetValue(obj);
                }
            }
            throw new ArgumentException("Property not found");
        }

        public static T GetAttribute<T>(this Enum obj) where T : Attribute
        {
            var type = obj.GetType();
            var memInfo = type.GetMember(obj.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T)attributes[0] : null;
        }

        public static K GetAttribute<T, K>(this Enum obj) where T : Attribute
        {
            var attr = GetAttribute<T>(obj);
            return attr.GetProperty<K>();
        }

        public static bool GetAttribute<T>(this Enum obj, out T t) where T : Attribute
        {
            bool success = false;
            try
            {
                t = obj.GetAttribute<T>();
                success = true;
            }
            catch (Exception)
            {
                t = null;
            }
            return success;
        }

        #region Synchrounously

        public static void Send(this Socket socket, string message)
        {
            if (!message.EndsWith(Environment.NewLine))
            {
                message += Environment.NewLine;
            }
            socket.Send(Encoding.ASCII.GetBytes(message));
        }

        public static void Send(this Event_ECS_Message message, Socket socket)
        {
            socket.Send(message.GetAttribute<Event_ECS_MessageAttribute, string>());
        }

        public static void Send(this Event_ECS_Message message, string arguments, Socket socket)
        {
            socket.Send(message.GetAttribute<Event_ECS_MessageAttribute, string>() + arguments);
        }

        public static void Send(this Event_ECS_Message message, Socket socket, out string response)
        {
            message.Send(socket);
            response = socket.Receive();
        }

        public static void Send(this Event_ECS_Message message, string args, Socket socket, out string response)
        {
            message.Send(args, socket);
            response = socket.Receive();
        }

        public static string Receive(this Socket socket, int size = 1024)
        {
            byte[] buffer = new byte[size];
            int bytesRead = socket.Receive(buffer);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }

        #endregion

        #region Asynchrounously

        public static IAsyncResult BeginSend(this Socket socket, string message)
        {
            return AsyncHandler.StartAsyncSend(message, socket);
        }

        public static IAsyncResult BeginSend(this Event_ECS_Message message, Socket socket)
        {
            return AsyncHandler.StartAsyncSend(message.GetAttribute<Event_ECS_MessageAttribute, string>(), socket);
        }

        public static IAsyncResult BeginSend(this Event_ECS_Message message, string arguments, Socket socket)
        {
            return AsyncHandler.StartAsyncSend(message.GetAttribute<Event_ECS_MessageAttribute, string>() + arguments, socket);
        }

        public static IAsyncResult BeginReceive(this Socket socket, Func<string, bool> callback = null)
        {
            return AsyncHandler.StartAsyncReceive(socket, callback);
        }

        #endregion

        public static bool IsValid(this Socket socket)
        {
            try
            {
                return socket.Connected && (!socket.Poll(1, SelectMode.SelectRead) || socket.Available != 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static IEnumerable<string> Split(this string s, int parts)
        {
            if (!string.IsNullOrEmpty(s) && parts > 0)
            {
                for (int i = 0, n = s.Length; i < n; i += parts)
                {
                    if (i + parts > n)
                    {
                        yield return s.Substring(i);
                    }
                    else
                    {
                        yield return s.Substring(i, Math.Min(parts, n - 1));
                    }
                }
            }
        }
    }
}