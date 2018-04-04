using System;
using System.Net.Sockets;
using System.Text;

namespace Event_ECS_Client_Console
{
    class Program
    {
        const string server = "localhost";
        const Int32 port = 9999;

        public static readonly int SendTimeout = Convert.ToInt32(TimeSpan.FromSeconds(1).TotalMilliseconds);
        public static readonly int ReceiveTimeout = Convert.ToInt32(TimeSpan.FromSeconds(1).TotalMilliseconds);

        public static object Event_ECS_Message { get; private set; }

        static void Main(string[] args)
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    socket.Connect(server, port);
                    socket.SendTimeout = SendTimeout;
                    socket.ReceiveTimeout = ReceiveTimeout;

                    Console.WriteLine("Connected to server. Enter messages to send.");
                    string input;
                    byte[] buffer = new byte[1024];
                    do
                    {
                        input = Console.ReadLine() + Environment.NewLine;
                        socket.Send(Encoding.ASCII.GetBytes(input));

                        try
                        {
                            int bytesRead = socket.Receive(buffer);
                            Console.WriteLine("Response: {0}", Encoding.ASCII.GetString(buffer, 0, bytesRead));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    } while (!string.IsNullOrEmpty(input));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    socket.Shutdown(SocketShutdown.Both);
                }
            }
        }
    }
}
