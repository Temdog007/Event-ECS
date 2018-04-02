using Event_ECS_Client_Common;
using Event_ECS_Client_WPF.Misc;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace Event_ECS_Client_WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        const string server = "localhost";
        const Int32 port = 9999;

        private TcpClient m_client;

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

        public ObservableCollection<Log> Logs { get; set; } = new ObservableCollection<Log>();

        private void addLog(string message)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => 
            {
                Logs.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Message = message
                });
            }));
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

        public ActionCommand<object> SendMessage => m_sendMessage ?? (m_sendMessage = new ActionCommand<object>(DoSendMessage, obj => IsConnected));
        private ActionCommand<object> m_sendMessage;

        private void DoSendMessage(object obj)
        {
            if((m_client?.Connected ?? false) == false)
            {
                if(!ConnectToServer())
                {
                    return;
                }
            }

            try
            {
                Task.Run(async () => 
                {
                    string response = await Message.SendAsync(Arguments, m_client.GetStream(), true);
                    if(!string.IsNullOrWhiteSpace(response))
                    {
                        addLog(response);
                    }
                });
            }
            catch(Exception e)
            {
                addLog(e.Message);
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
