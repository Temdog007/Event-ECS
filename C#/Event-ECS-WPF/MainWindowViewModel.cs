using Event_ECS_Client_WPF.Properties;
using EventECSWrapper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using ECSSystem = Event_ECS_Client_WPF.SystemObjects.System;

namespace Event_ECS_Client_WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private ECSWrapper m_wrapper;

        public MainWindowViewModel()
        {
            m_wrapper = new ECSWrapper();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    m_wrapper.Dispose();
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

        public AsyncActionCommand<object> UpdateStateCommand => m_updateState ?? (m_updateState = new AsyncActionCommand<object>(UpdateState));
        private AsyncActionCommand<object> m_updateState;

        private void UpdateState()
        {
            try
            {
                m_wrapper.Init();
                addLog(m_wrapper.GetState());
            }
            catch (Exception e)
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
