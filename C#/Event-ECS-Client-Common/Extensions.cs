using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Event_ECS_Client_Common
{
    public static class Extensions
    {
        public static T GetAttribute<T>(this Enum obj) where T : Attribute
        {
            var type = obj.GetType();
            var memInfo = type.GetMember(obj.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T)attributes[0] : null;
        }

        public static bool GetAttribute<T>(this Enum obj, out T t) where T : Attribute
        {
            bool success = false;
            try
            {
                t = obj.GetAttribute<T>();
                success = true;
            }
            catch(Exception)
            {
                t = null;
            }
            return success;
        }

        public static byte[] GetData(this Event_ECS_Message message)
        {
            return message.GetAttribute<Event_ECS_MessageAttribute>().Data;
        }

        public static void Send<T>(this Event_ECS_Message message, T stream) where T : Stream
        {
            message.GetAttribute<Event_ECS_MessageAttribute>().Send(stream);
        }

        public static int Send<T>(this T socket, Event_ECS_Message message) where T : Socket
        {
            byte[] data = message.GetData();
            return socket.Send(data, data.Length, SocketFlags.None);
        }

        public static IEnumerable<string> Receive<T>(this T socket) where T : Socket
        {
            int bytes = 0;
            byte[] buffer = new byte[256];
            do
            {
                bytes = socket.Receive(buffer, buffer.Length, SocketFlags.None);
                yield return Encoding.ASCII.GetString(buffer, 0, bytes);
            } while (bytes < 0);
        }
    }
}
