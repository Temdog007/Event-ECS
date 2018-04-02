using System;
using System.IO;
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
            foreach(var prop in obj.GetType().GetProperties())
            {
                if(prop.PropertyType == typeof(T))
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

        public static K GetAttribute<T,K>(this Enum obj) where T : Attribute
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
            catch(Exception)
            {
                t = null;
            }
            return success;
        }

        #region Synchrounously

        public static void Send<T>(this T stream, byte[] buffer) where T : Stream
        {
            stream.Write(buffer, 0, buffer.Length);
        }

        public static void Send<T>(this T stream, string message) where T : Stream
        {
            if(!message.EndsWith(Environment.NewLine))
            {
                message += Environment.NewLine;
            }
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Send(data);
        }

        public static void Send<T>(this T stream, string message, out string response) where T : Stream
        {
            stream.Send(message);
            response = stream.Receive();
        }

        public static string Receive<T>(this T stream) where T: Stream
        {
            byte[] data = new byte[256];
            Int32 bytes = stream.Read(data, 0, data.Length);
            return Encoding.ASCII.GetString(data, 0, bytes);
        }

        public static void Send<T>(this Event_ECS_Message message, T stream) where T : Stream
        {
            stream.Send(message.GetAttribute<Event_ECS_MessageAttribute, byte[]>());
        }

        public static void Send<T>(this Event_ECS_Message message, string arguments, T stream) where T : Stream
        {
            stream.Send(message.GetAttribute<Event_ECS_MessageAttribute, string>() + arguments);
        }

        public static void Send<T>(this Event_ECS_Message message, T stream, out string response) where T : Stream
        {
            message.Send(stream);
            response = stream.Receive();
        }

        public static void Send<T>(this Event_ECS_Message message, string args, T stream, out string response) where T : Stream
        {
            message.Send(args, stream);
            response = stream.Receive();
        }
        #endregion

        #region async

        public static async Task SendAsync<T>(this T stream, byte[] buffer) where T : Stream
        {
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }

        public static async Task<string> SendAsync<T>(this T stream, string message, bool waitForResponse = false) where T : Stream
        {
            if (!message.EndsWith(Environment.NewLine))
            {
                message += Environment.NewLine;
            }
            byte[] data = Encoding.ASCII.GetBytes(message);
            await stream.SendAsync(data);
            if(waitForResponse)
            {
                return await stream.ReceiveAsync();
            }
            return string.Empty;
        }

        public static async Task<string> ReceiveAsync<T>(this T stream) where T : Stream
        {
            byte[] data = new byte[256];
            Int32 bytes = await stream.ReadAsync(data, 0, data.Length);
            return Encoding.ASCII.GetString(data, 0, bytes);
        }

        public static async Task<string> SendAsync<T>(this Event_ECS_Message message, T stream, bool waitForResponse) where T : Stream
        {
            await stream.SendAsync(message.GetAttribute<Event_ECS_MessageAttribute, byte[]>());
            if(waitForResponse)
            {
                return await stream.ReceiveAsync();
            }
            return string.Empty;
        }

        public static async Task<string> SendAsync<T>(this Event_ECS_Message message, string arguments, T stream, bool waitForResponse) where T : Stream
        {
            await stream.SendAsync(message.GetAttribute<Event_ECS_MessageAttribute, string>() + arguments);
            if (waitForResponse)
            {
                return await stream.ReceiveAsync();
            }
            return string.Empty;
        }

        #endregion
    }
}
