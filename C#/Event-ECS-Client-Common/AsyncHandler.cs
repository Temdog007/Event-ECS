using System;
using System.Net.Sockets;
using System.Text;

namespace Event_ECS_Client_Common
{
    public class AsyncHandler
    {
        private AsyncHandler(Socket socket) 
            : this(socket, new byte[1024])
        {

        }

        private AsyncHandler(Socket socket, byte[] buffer)
        {
            Socket = socket ?? throw new ArgumentNullException(nameof(socket));
            Buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        }

        private AsyncHandler(Socket socket, string message) 
            : this(socket, Encoding.ASCII.GetBytes(message.EndsWith(Environment.NewLine) ? message : (message + Environment.NewLine)))
        {
        }

        private AsyncHandler(AsyncHandler asyncHandler) 
            : this(asyncHandler.Socket, asyncHandler.Buffer)
        {
        }

        public static IAsyncResult StartAsyncSend(string message, Socket socket)
        {
            return new AsyncHandler(socket, message).BeginSend();
        }

        public static IAsyncResult StartAsyncSend(byte[] message, Socket socket)
        {
            return new AsyncHandler(socket, message).BeginSend();
        }

        public static IAsyncResult StartAsyncSend(AsyncHandler handler)
        {
            return new AsyncHandler(handler).BeginSend();
        }

        public static IAsyncResult StartAsyncReceive(Socket socket, Func<string, bool> callback)
        {
            return new AsyncHandler(socket).BeginReceive(callback);
        }

        private Socket Socket { get; set; }

        private byte[] Buffer { get; set; }

        private IAsyncResult BeginSend()
        {
            return Socket.BeginSend(Buffer, 0, Buffer.Length, SocketFlags.None, AsyncSendCallback, null);
        }

        private IAsyncResult BeginReceive(Func<string, bool> callback)
        {
            return Socket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, AsyncReceiveCallback, callback);
        }

        private void AsyncSendCallback(IAsyncResult ar)
        {
            try
            {
                int bytesSent = Socket.EndSend(ar);
                Console.WriteLine("Sent {0} bytes", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void AsyncReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int bytesRead = Socket.EndReceive(ar);
                Console.WriteLine("Read {0} bytes", bytesRead);

                string response = Encoding.ASCII.GetString(Buffer, 0, bytesRead);

                Func<string, bool> callback = ar.AsyncState as Func<string, bool>;
                if (callback?.Invoke(response) ?? false)
                {
                    BeginReceive(callback);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
