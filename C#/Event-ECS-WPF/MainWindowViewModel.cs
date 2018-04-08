using Event_ECS_WPF.Properties;
using EventECSWrapper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using ECSSystem = Event_ECS_WPF.SystemObjects.EntityComponentSystem;

namespace Event_ECS_WPF
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private ECSWrapper ecs;

        public MainWindowViewModel()
        {
            System.Name = "Entity Component System";
            System.RegisteredComponents.Add("Components");
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ecs?.Dispose();
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

        public AsyncActionCommand<object> InitECSCommand => m_initECSCommand ?? (m_initECSCommand = new AsyncActionCommand<object>(InitECS));
        private AsyncActionCommand<object> m_initECSCommand;

        public ActionCommand<Window> CloseCommand => m_closeCommand ?? (m_closeCommand = new ActionCommand<Window>(CloseWindow));
        public ActionCommand<Window> m_closeCommand;

        private void CloseWindow(Window window)
        {
            window.Close();
        }

        private void InitECS()
        {
            ecs?.Dispose();
            try
            {
                ecs = new ECSWrapper();
                ecs.Require("eventecs");
                ecs?.DoString("System = require 'system'");
                addLog("Initialized Entity Component System");
            }
            catch(Exception e)
            {
                addLog(e.Message);
                ecs?.Dispose();
                ecs = null;
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
