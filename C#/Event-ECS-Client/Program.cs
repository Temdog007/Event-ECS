using Event_ECS_Client_Common;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;

namespace Event_ECS_Client_Test
{
    class Program
    {
        const string server = "localhost";
        const Int32 port = 9999;

        public static readonly int SendTimeout = Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds);
        public static readonly int ReceiveTimeout = Convert.ToInt32(TimeSpan.FromSeconds(5).TotalMilliseconds);

        static void Main(string[] args)
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    socket.Connect(server, port);
                    socket.SendTimeout = SendTimeout;
                    socket.ReceiveTimeout = ReceiveTimeout;

                    // Get system data
                    Console.WriteLine("Requesting system data");
                    Event_ECS_Message.GET_DATA.Send(socket, out string response);
                    Console.WriteLine("Received: {0}", response);

                    // Add an entity
                    Console.WriteLine("Adding entity");
                    Event_ECS_Message.ADD_ENTITY.Send(socket, out response);
                    Console.WriteLine("Received: {0}", response);

                    // Show entity ID
                    response = response.Replace(Event_ECS_MessageResponse.ENTITY_DATA.ToString(), string.Empty);
                    dynamic obj = JsonConvert.DeserializeObject(response);
                    Console.WriteLine("Deserialized entity: ID = {0}", obj.id);

                    // Remove entity
                    Console.WriteLine("Requesting entity to be removed");
                    Event_ECS_Message.REMOVE_ENTITY.Send(((object)obj.id).ToString(), socket, out response);
                    response = response.Replace(Event_ECS_MessageResponse.REMOVED_ENTITIES.ToString(), string.Empty);
                    Console.WriteLine("Removed {0} entities", response);

                    // Get system data
                    Console.WriteLine("Requesting system data");
                    Event_ECS_Message.GET_DATA.Send(socket, out response);
                    Console.WriteLine("Received: {0}", response);

                    // Close
                    Event_ECS_Message.CLOSE.Send(socket);
                    Console.WriteLine("Requested socket to be closed");
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
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
