using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Logger;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Event_ECS_WPF.SystemObjects
{
    public partial class ECS : IECS
    {
        public const int ConnectDelay = 2000;

        public const ushort Port = 32485;

        public const string Serialize = "Serialize";

        public static readonly IPEndPoint Endpoint = new IPEndPoint(Host, Port);

        public readonly object m_lock = new object();

        internal ECS(){}

        public event DataReceived DataReceived;

        public static IPAddress Host => IPAddress.Parse("127.0.0.1");

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

        public void TryConnect()
        {
            if (!TargetAppIsRunning)
            {
                // Try again later
                Task.Delay(ConnectDelay).ContinueWith(task => TryConnect());
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
                    catch (InvalidOperationException)
                    {
                        Dispose();
                        TryConnect();
                    }
                    catch (Exception e)
                    {
                        if (!(e is SocketException))
                        {
                            LogManager.Instance.Add(e);
                        }
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
                    if (!(e is SocketException))
                    {
                        LogManager.Instance.Add(e);
                    }
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
                SendTimeout = 1000
            };
            LogManager.Instance.Add("Starting to connect...");
        }
        private void HandleDisconnect()
        {
            LogManager.Instance.Add("Client has disconnected", LogLevel.Medium);
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
                            string[] dataList = message.Split('\n');
                            if (dataList[0] == Serialize)
                            {
                                Application.Current.Dispatcher.BeginInvoke(DataReceived, dataList.SubArray(1));
                            }
                            else
                            {
                                LogManager.Instance.Add(message);
                            }
                            Socket.NoDelay = true;
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
            lock (m_lock)
            {
                try
                {
                    if (IsConnected && Socket.IsConnected())
                    {
                        byte[] byteData = Encoding.ASCII.GetBytes(message);
                        Socket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, SendCallback, Socket);
                    }
                    else
                    {
                        LogManager.Instance.Add("Socket isn't connected. So, {0} wasn't sent", message);
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
                    LogManager.Instance.Add("Sent {0} bytes to the server", bytes);
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
    }
}
