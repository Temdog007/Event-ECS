using Event_ECS_Client_Common;
using Event_ECS_Client_WPF.Misc;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ECSSystem = Event_ECS_Client_WPF.SystemObjects.System;

namespace Event_ECS_Client_WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        const string server = "localhost";
        const Int32 port = 9999;

        private TcpClient m_client;

        private object m_lock = new object();

        public MainWindowViewModel()
        {
            m_client = new TcpClient();
            m_client.ReceiveTimeout = 1000;
            m_client.SendTimeout = 1000;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    m_client?.Close();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public ECSSystem System
        {
            get => m_system;
            set
            {
                m_system = value;
                OnPropertyChanged("System");
            }
        }private ECSSystem m_system = new ECSSystem();

        public ObservableCollection<Log> Logs
        {
            get => m_logs;
            set
            {
                m_logs = value;
                OnPropertyChanged("Logs");
            }
        }
        private ObservableCollection<Log> m_logs = new ObservableCollection<Log>();

        private object m_logLock = new object();
        private void addLog(string message)
        {
            lock (m_logLock)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Logs.Add(new Log()
                    {
                        DateTime = DateTime.Now,
                        Message = message
                    });
                }));
            }
        }

        private bool ConnectToServer()
        {
            try
            {
                m_client?.Connect(server, port);
                return true;
            }
            catch(Exception e)
            {
                addLog(e.Message);
                return false;
            }
            finally
            {
                OnPropertyChanged("IsConnected");
                SendMessage.UpdateCanExecute(this, EventArgs.Empty);
            }
        }

        public ActionCommand<object> ConnectCommand => m_connectCommand ?? (m_connectCommand = new ActionCommand<object>(obj => ConnectToServer()));
        private ActionCommand<object> m_connectCommand;

        public bool IsConnected => m_client?.Connected ?? false;

        public AsyncActionCommand<object> SendMessage => m_sendMessage ?? (m_sendMessage = new AsyncActionCommand<object>(DoSendMessage, obj => IsConnected));
        private AsyncActionCommand<object> m_sendMessage;

        private void DoSendMessage(object obj)
        {
            try
            {
                if (Monitor.TryEnter(m_lock))
                {
                    if ((m_client?.Connected ?? false) == false)
                    {
                        if (!ConnectToServer())
                        {
                            return;
                        }
                    }

                    Message.Send(Arguments, m_client.GetStream(), out string response);
                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        HandleResponse(response);
                        addLog(response);
                    }
                }
                else
                {
                    addLog("Previous send message task hasn't completed");
                }
            }
            catch (Exception e)
            {
                addLog(e.Message);
            }
            finally
            {
                Monitor.Exit(m_lock);
            }
        }

        private void HandleResponse(string response)
        {
            foreach(Event_ECS_MessageResponse e in Enum.GetValues(typeof(Event_ECS_MessageResponse)))
            {
                string eStr = e.ToString();
                if(response.Contains(eStr))
                {
                    HandleMessage(e, response.Replace(eStr, string.Empty));
                }
            }
        }

        private void HandleMessage(Event_ECS_MessageResponse response, string args)
        {
            switch(response)
            {
                case Event_ECS_MessageResponse.SYSTEM_DATA:
                    System = JsonConvert.DeserializeObject<ECSSystem>(args);
                    break;
                default:
                    break;
            }
        }

        public string Arguments
        {
            get => m_arguments;
            set
            {
                m_arguments = value;
                OnPropertyChanged("Arguments");
            }
        } private string m_arguments = string.Empty;

        public Event_ECS_Message Message
        {
            get => m_message;
            set
            {
                m_message = value;
                OnPropertyChanged("Message");
            }
        }
        private Event_ECS_Message m_message;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
