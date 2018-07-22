using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Logger;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Event_ECS_WPF.SystemObjects.Communication
{
    public partial class ECS : IECS
    {
        public const ushort Port = 32485;

        public const char ETX = (char)3;

        public static IPEndPoint Endpoint => new IPEndPoint(Host, Port);

        private readonly object m_lock = new object();

        private readonly AutoResetEvent appRunEvent = new AutoResetEvent(false);

        private string leftover = string.Empty;

        internal ECS() { TryConnect(); }

        public event DataReceived DataReceived;

        public event Action ServerDisconnect;

        public static readonly IPAddress Host = IPAddress.Parse("127.0.0.1");

        public bool ShouldUpdateServer { get; set; } = true;

        protected Socket Socket { get; private set; }

        public void Dispose()
        {
            lock (m_lock)
            {
                try
                {
                    Socket.Shutdown(SocketShutdown.Both);
                    Socket.Dispose();
                    Socket = null;
                    LogManager.Instance.Add("Closed connection");
                }
                catch (Exception)
                {

                }
                finally
                {
                    IsConnected = false;
                }
            }
        }

        public void SignalAppStarted()
        {
            appRunEvent.Set();
        }

        public void TryConnect()
        {
            if (!TargetAppIsRunning)
            {
                Task.Run(() => TryConnectOnEvent());
            }
            else
            {
                lock (m_lock)
                {
                    if (Socket == null)
                    {
                        CreateSocket();
                    }
                    else if (Socket.Connected)
                    {
                        return;
                    }

                    try
                    {
                        Socket.BeginConnect(Endpoint, ConnectionCallback, Socket);
                    }
                    catch (Exception e)
                    {
                        LogManager.Instance.Add(e);
                        IsConnected = Socket.Connected;
                        TryConnect();
                    }
                }
            }
        }

        private void ConnectionCallback(IAsyncResult result)
        {
            lock (m_lock)
            {
                try
                {
                    Socket.EndConnect(result);

                    IsConnected = true;

                    StartReceiving(new byte[2048]);

                    LogManager.Instance.Add("Connected to {0}", Socket.RemoteEndPoint);
                }
                catch (Exception e)
                {
                    LogManager.Instance.Add(e);
                    IsConnected = Socket.Connected;
                    TryConnect();
                }
            }
        }

        private void CreateSocket()
        {
            Dispose();
            Socket = new Socket(Host.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveTimeout = 1000,
                SendTimeout = 1000,
                NoDelay = true
            };
            LogManager.Instance.Add("Starting to connect...");
        }

        private void HandleDisconnect()
        {
            LogManager.Instance.Add("Client has disconnected. Server might have shutdown.", LogLevel.Medium);
            ServerDisconnect?.Invoke();
            Dispose();
            TryConnect();
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            lock (m_lock)
            {
                try
                {
                    byte[] buffer = (byte[])ar.AsyncState;
                    int bytesRead = Socket.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        try
                        {
                            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                            // End of message contains ETX.
                            int index = message.IndexOf(ETX);

                            // If not found, haven't read all of the data
                            if(index == -1)
                            {
                                leftover += message;
                            }
                            else
                            {
                                string[] data = message.Split(ETX);
                                Application.Current.Dispatcher.BeginInvoke(DataReceived, leftover + data[0]);
                                leftover = string.Empty;

                                int n = data.Length - 1;
                                for (int i = 1; i < n; ++i)
                                {
                                    Application.Current.Dispatcher.BeginInvoke(DataReceived, data[i]);
                                }
                                if(n > 0)
                                {
                                    leftover = data[n];
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            LogManager.Instance.Add(e);
                        }
                        finally
                        {
                            StartReceiving(buffer);
                        }
                    }
                    else
                    {
                        try
                        {
                            //Ensure that the client is still connected
                            if (Socket.IsConnected())
                            {
                                StartReceiving(buffer);
                            }
                            else
                            {
                                HandleDisconnect();
                            }
                        }
                        catch (Exception)
                        {
                            HandleDisconnect();
                        }
                    }
                }
                catch (SocketException ex)
                {
                    LogManager.Instance.Add(ex);
                    HandleDisconnect();
                }
            }
        }

        private void Send(string message)
        {
            if (!ShouldUpdateServer) { return; }

            if (!message.EndsWith(Environment.NewLine))
            {
                message += Environment.NewLine;
            }
            lock (m_lock)
            {
                try
                {
                    if (IsConnected && Socket.IsConnected())
                    {
                        byte[] byteData = Encoding.ASCII.GetBytes(message);
                        Socket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, SendCallback, Socket);
                        LogManager.Instance.Add(LogLevel.SuperLow, "Sending '{0}' ({1} bytes) to the server", message, byteData.Length);
                    }
                    else
                    {
                        LogManager.Instance.Add(LogLevel.Medium, "Socket isn't connected. So, {0} wasn't sent", message);
                    }
                }
                catch (Exception e)
                {
                    LogManager.Instance.Add(e);
                }
            }
        }

        private void Send(string message, params object[] args)
        {
            Send(string.Format(message, args));
        }

        private void SendCallback(IAsyncResult ar)
        {
            lock (m_lock)
            {
                try
                {
                    int bytes = Socket.EndSend(ar);
                    LogManager.Instance.Add(LogLevel.SuperLow, "Sent {0} bytes to the server", bytes);
                }
                catch (Exception e)
                {
                    LogManager.Instance.Add(e);
                }
            }
        }

        private void StartReceiving(byte[] buffer)
        {
            var endpoint = (EndPoint)Endpoint;
            Socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endpoint, ReceiveCallback, buffer);
        }

        private void TryConnectOnEvent()
        {
            appRunEvent.WaitOne();
            TryConnect();
        }
    }
}
