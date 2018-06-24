using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Logger;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace Event_ECS_WPF.SystemObjects
{
    public partial class ECS : IECS
    {
        public const ushort Port = 32485;

        public const string Serialize = "Serialize";

        public static IPAddress Host => IPAddress.Parse("127.0.0.1");

        public static readonly IPEndPoint Endpoint = new IPEndPoint(Host, Port);

        public readonly object m_lock = new object();

        internal ECS()
        {
            lock (m_lock)
            {
                Dispose();
                TryConnect();
            }
        }

        public event DataReceived DataReceived;

        protected Socket Socket { get; private set; }

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

        #region Socket
        public void TryConnect()
        {
            lock (m_lock)
            {
                if(Socket == null)
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
                catch (Exception e)
                {
                    LogManager.Instance.Add(e);
                    HandleDisconnect();
                }
            }
        }

        private void StartReceiving(byte[] buffer)
        {
            var endpoint = (EndPoint)Endpoint;
            Socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endpoint, ReceiveCallback, buffer);
        }

        private void HandleDisconnect()
        {
            LogManager.Instance.Add("Client has disconnected", LogLevel.Medium);
            Dispose();
            TryConnect();
        }

        private void Send(string message)
        {
            lock (m_lock)
            {
                try
                {
                    byte[] byteData = Encoding.ASCII.GetBytes(message);
                    Socket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, SendCallback, Socket);
                }
                catch(Exception e)
                {
                    LogManager.Instance.Add(e);
                    HandleDisconnect();
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
                    HandleDisconnect();
                }
            }
        }
        #endregion

        #region IECS
        public void AddComponent(string systemName, int entityID, string componentName)
        {
            Send("AddComponent|{0}|{1}|{2}", systemName, entityID, componentName);
        }
        
        public void AddEntity(string systemName)
        {
            Send("AddEntity|{0}", systemName);
        }

        public void BroadcastEvent(string eventName)
        {
            Send("BroadcastEvent|{0}", eventName);
        }

        public void DispatchEvent(string systemName, string eventName)
        {
            Send("DispatchEvent|{0}|{1}", systemName, eventName);
        }

        public void DispatchEvent(string systemName, int entityID, string eventName)
        {
            Send("DispatchEvent|{0}|{1}|{2}", systemName, entityID, eventName);
        }

        public void Execute(string code)
        {
            Send("Execute|{0}", code);
        }

        public void ReloadModule(string modName)
        {
            Send("ReloadModule|{0}", modName);
        }

        public void RemoveComponent(string systemName, int entityID, int componentID)
        {
            Send("RemoveComponent|{0}|{1}|{2}", systemName, entityID, componentID);
        }

        public void RemoveEntity(string systemName, int entityID)
        {
            Send("RemoveComponent|{0}|{1}", systemName, entityID);
        }

        public void Reset()
        {
            Send("Reset");
        }

        public void SetComponentBool(string systemName, int entityID, int componentID, string key, bool value)
        {
            Send("SetComponentBool|{0}|{1}|{2}|{3}|{4}", systemName, entityID, componentID, key, value);
        }

        public void SetComponentEnabled(string systemName, int entityID, int componentID, bool value)
        {
            Send("SetComponentEnabled|{0}|{1}|{2}|{3}", systemName, entityID, componentID, value);
        }

        public void SetComponentNumber(string systemName, int entityID, int componentID, string key, double value)
        {
            Send("SetComponentNumber|{0}|{1}|{2}|{3}|{4}", systemName, entityID, componentID, key, value);
        }

        public void SetComponentString(string systemName, int entityID, int componentID, string key, string value)
        {
            Send("SetComponentString|{0}|{1}|{2}|{3}|{4}", systemName, entityID, componentID, key, value);
        }

        public void SetEntityBool(string systemName, int entityID, string key, bool value)
        {
            Send("SetEntityBool|{0}|{1}|{2}|{3}", systemName, entityID, key, value);
        }

        public void SetEntityEnabled(string systemName, int entityID, bool value)
        {
            Send("SetEntityEnabled|{0}|{1}|{2}", systemName, entityID, value);
        }

        public void SetEntityNumber(string systemName, int entityID, string key, double value)
        {
            Send("SetEntityNumber|{0}|{1}|{2}|{3}", systemName, entityID, key, value);
        }

        public void SetEntityString(string systemName, int entityID, string key, string value)
        {
            Send("SetEntityString|{0}|{1}|{2}|{3}", systemName, entityID, key, value);
        }
        
        public void SetSystemBool(string systemName, string key, bool value)
        {
            Send("SetSystemBool|{0}|{1}|{2}", systemName, key, value);
        }

        public void SetSystemEnabled(string systemName, bool value)
        {
            Send("SetSystemEnabled|{0}|{1}", systemName, value);
        }

        public void SetSystemNumber(string systemName, string key, double value)
        {
            Send("SetSystemNumber|{0}|{1}|{2}", systemName, key, value);
        }

        public void SetSystemString(string systemName, string key, string value)
        {
            Send("SetSystemString|{0}|{1}|{2}", systemName, key, value);
        }
#endregion
    }
}
