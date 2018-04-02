using Event_ECS_Client_Common;
using System;
using System.Net.Sockets;

namespace Event_ECS_Client_Console
{
    class Program
    {
        const string server = "localhost";
        const Int32 port = 9999;

        public static readonly int SendTimeout = Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds);
        public static readonly int ReceiveTimeout = Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds);

        public static object Event_ECS_Message { get; private set; }

        static void Main(string[] args)
        {
            try
            {
                using (TcpClient client = new TcpClient(server, port))
                {
                    client.SendTimeout = SendTimeout;
                    client.ReceiveTimeout = ReceiveTimeout;

                    using (var stream = client.GetStream())
                    {
                        Console.WriteLine("Connected to server. Enter messages to send.");
                        string input;
                        do
                        {
                            input = Console.ReadLine();
                            stream.Send(input, out string response);
                            Console.WriteLine("Response: {0}", response);

                        } while (!string.IsNullOrEmpty(input));
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
