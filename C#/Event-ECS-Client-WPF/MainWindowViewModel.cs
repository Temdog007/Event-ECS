using Event_ECS_Client_Common;
using Event_ECS_Client_WPF.Misc;
using Event_ECS_Client_WPF.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using ECSSystem = Event_ECS_Client_WPF.SystemObjects.System;

namespace Event_ECS_Client_WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

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
                try
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (Settings.Default.MultilineLog)
                        {
                            bool addDate = true;
                            foreach (string str in message.Split(Settings.Default.MaxLogLength))
                            {
                                Logs.Add(new Log()
                                {
                                    DateTime = addDate ? DateTime.Now : default(DateTime?),
                                    Message = str
                                });
                                addDate = false;
                            }
                        }
                        else
                        {
                            Logs.Add(new Log()
                            {
                                DateTime =DateTime.Now,
                                Message = message
                            });
                        }
                    }));
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private void addLog(string message, params object[] args)
        {
            addLog(string.Format(message, args));
        }

        private void clearLogs()
        {
            lock(m_logLock)
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Logs.Clear();
                    }));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public ActionCommand<object> ClearLogCommand => m_clearLogCommand ?? (m_clearLogCommand = new ActionCommand<object>(clearLogs));
        private ActionCommand<object> m_clearLogCommand;

        public AsyncActionCommand<object> SendMessage => m_sendMessage ?? (m_sendMessage = new AsyncActionCommand<object>(DoSendMessage));
        private AsyncActionCommand<object> m_sendMessage;

        private void DoSendMessage(object obj)
        {
            try
            {
                //Message.BeginSend(Arguments, m_socket);
                addLog("Sent Message: {0}", Message);
            }
            catch (Exception e)
            {
                addLog(e.Message);
            }
        }

        private void HandleResponse(string response)
        {
            if (!string.IsNullOrWhiteSpace(response))
            {
                addLog("Received for system: {0}", response);
                foreach (Event_ECS_MessageResponse ev in Enum.GetValues(typeof(Event_ECS_MessageResponse)))
                {
                    string eStr = ev.ToString();
                    if (response.Contains(eStr))
                    {
                        try
                        {
                            HandleMessage(ev, response.Replace(eStr, string.Empty));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }
                }
            }
        }

        private void HandleMessage(Event_ECS_MessageResponse response, string args)
        {
            switch(response)
            {
                case Event_ECS_MessageResponse.SYSTEM_DATA:
                    using (JsonTextReader reader = new JsonTextReader(new StringReader(args)))
                    {
                        while(reader.Read())
                        {
                            switch(reader.TokenType)
                            {
                                case JsonToken.PropertyName:
                                    if(reader.ValueEquals("system"))
                                    {
                                        System.ReadJson(reader);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
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
