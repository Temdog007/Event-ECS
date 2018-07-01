using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Event_ECS_WPF.Logger
{
    public enum LogLevel
    {
        Low,
        Medium,
        High
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

        private LogLevel m_filter;

        private LogManager()
        {
            m_logs = new ObservableCollection<Log>();
        }

        private NotifyCollectionChangedEventHandler m_collectionChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                m_collectionChanged += value;
                ((INotifyCollectionChanged)m_logs).CollectionChanged += value;
            }

            remove
            {
                m_collectionChanged -= value;
                ((INotifyCollectionChanged)m_logs).CollectionChanged -= value;
            }
        }

        private PropertyChangedEventHandler m_propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                m_propertyChanged += value;
                ((INotifyPropertyChanged)m_logs).PropertyChanged += value;
            }

            remove
            {
                m_propertyChanged -= value;
                ((INotifyPropertyChanged)m_logs).PropertyChanged -= value;
            }
        }
        public static LogManager Instance => m_instance ?? (m_instance = new LogManager());

        public LogLevel Filter
        {
            get => m_filter;
            set
            {
                m_filter = value;
                m_propertyChanged?.Invoke(this, new PropertyChangedEventArgs("Filter"));
                m_collectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        private void Add(Log log)
        {
            var app = Application.Current;
            if (app == null) { return; }
            app.Dispatcher.BeginInvoke(new Action(() => 
            {
                lock (m_lock)
                {
                    m_logs.Add(log);
                }
            }));
        }

        public void Add(string message, LogLevel level = LogLevel.Low)
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
                Add(new Log(string.Join(Environment.NewLine, strList), level));
            }
            else
            {
                Add(new Log(message, level));
            }
        }

        public void Add(string message, params object[] args)
        {
            Add(LogLevel.Low, message, args);
        }

        public void Add(IEnumerable<string> messages, LogLevel level = LogLevel.Low)
        {
            foreach (string message in messages)
            {
                Add(message, level);
            }
        }

        public void Add(LogLevel level, string message, params object[] args)
        {
            Add(string.Format(message, args), level);
        }

        public void Add(Exception e)
        {
            Add(e.Message, LogLevel.High);
            if(e.InnerException != null)
            {
                Add(e.InnerException);
            }
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
            return m_logs.Where(log => log.Level >= this.Filter).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
