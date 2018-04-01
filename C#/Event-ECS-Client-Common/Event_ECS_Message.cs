using System;
using System.IO;
using System.Text;

namespace Event_ECS_Client_Common
{
    public enum Event_ECS_Message
    {
        [Event_ECS_Message("getdata")]
        GET_DATA,

        [Event_ECS_Message("close")]
        CLOSE
    }

    public class Event_ECS_MessageAttribute : Attribute
    {
        public Event_ECS_MessageAttribute(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }

        public byte[] Data => Encoding.ASCII.GetBytes(Message + "\n");

        public void Send<T>(T stream) where T : Stream
        {
            byte[] data = Data;
            stream.Write(data, 0, data.Length);
        }
    }
}
