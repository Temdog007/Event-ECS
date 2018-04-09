using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace Event_ECS_WPF.Logger
{
    public class Log
    {
        public DateTime? DateTime { get; set; }
        public string Message { get; set; }
    }

    public class LogManager : INotifyPropertyChanged, INotifyCollectionChanged, IEnumerable<Log>
    {
        private static LogManager m_instance;
        private object m_lock = new object();

        private ObservableCollection<Log> m_logs;

        private LogManager()
        {
            m_logs = new ObservableCollection<Log>();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                ((INotifyCollectionChanged)m_logs).CollectionChanged += value;
            }

            remove
            {
                ((INotifyCollectionChanged)m_logs).CollectionChanged -= value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                ((INotifyPropertyChanged)m_logs).PropertyChanged += value;
            }

            remove
            {
                ((INotifyPropertyChanged)m_logs).PropertyChanged -= value;
            }
        }
        public static LogManager Instance => m_instance ?? (m_instance = new LogManager());

        public void Add(Log log)
        {
            lock (m_lock)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => m_logs.Add(log)));
            }
        }

        public void Add(string message)
        {
            if (Settings.Default.MultilineLog)
            {
                bool addDate = true;
                foreach (var line in message.Split('\n'))
                {
                    foreach (string str in line.Split(Settings.Default.MaxLogLength))
                    {
                        LogManager.Instance.Add(new Log()
                        {
                            DateTime = addDate ? DateTime.Now : default(DateTime?),
                            Message = str
                        });
                        addDate = false;
                    }
                }
            }
            else
            {
                LogManager.Instance.Add(new Log()
                {
                    DateTime = DateTime.Now,
                    Message = message
                });
            }
        }

        public void Add(string message, params object[] args)
        {
            Add(string.Format(message, args));
        }

        public void Clear()
        {
            lock (m_lock)
            {
                m_logs.Clear();
            }
        }

        public IEnumerator<Log> GetEnumerator()
        {
            return ((IEnumerable<Log>)m_logs).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Log>)m_logs).GetEnumerator();
        }
    }
}
