using Event_ECS_Client_Common;
using System;
using System.Net.Sockets;

namespace Event_ECS_Client
{
    class Program
    {
        const string server = "localhost";
        const Int32 port = 9999;
        
        static void Main(string[] args)
        {
            using (TcpClient client = new TcpClient(server, port))
            {
                client.SendTimeout = 1000;
                client.ReceiveTimeout = 1000;

                using (var stream = client.GetStream())
                {
                    Event_ECS_Message.GET_DATA.Send(stream);
                    Console.WriteLine("Requested 'GetData'");

                    byte[] data = new byte[256];
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    string responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: {0}", responseData);

                    Event_ECS_Message.CLOSE.Send(stream);
                    Console.WriteLine("Requested socket to be closed");
                }
            }
        }
    }
}
