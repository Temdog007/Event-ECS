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
    public enum LogLevel
    {
        High,
        Medium,
        Low
    }

    public class Log
    {
        public DateTime DateTime { get; private set; }
        public string Message { get; private set; }
        public LogLevel Level { get; private set; }

        internal Log(string message, LogLevel level)
        {
            Message = message;
            Level = level;
            DateTime = DateTime.Now;
        }
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

        private void Add(Log log)
        {
            lock (m_lock)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => m_logs.Add(log)));
            }
        }

        public void Add(string message, LogLevel level)
        {
            if (Settings.Default.MultilineLog)
            {
                List<string> strList = new List<string>();
                foreach (var line in message.Split('\n'))
                {
                    foreach (string str in line.Split(Settings.Default.MaxLogLength))
                    {
                        strList.Add(str);
                    }
                }
                LogManager.Instance.Add(new Log(string.Join(Environment.NewLine, strList), level));
            }
            else
            {
                LogManager.Instance.Add(new Log(message, level));
            }
        }

        public void Add(string message, params object[] args)
        {
            Add(LogLevel.Low, message, args);
        }

        public void Add(LogLevel level, string message, params object[] args)
        {
            Add(string.Format(message, args), level);
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
