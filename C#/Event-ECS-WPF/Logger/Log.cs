using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows;

namespace Event_ECS_WPF.Logger
{
    public enum LogLevel
    {
        SuperLow = -1,
        Low = 0,
        Medium = 1,
        High = 2
    }

    public struct Log
    {
        internal Log(string message, LogLevel level)
        {
            Message = message;
            Level = level;
            DateTime = DateTime.Now;
        }

        public DateTime DateTime { get; }
        public LogLevel Level { get; }
        public string Message { get; }
    }

    public class LogManager : INotifyPropertyChanged
    {
        private static LogManager m_instance;

        private readonly object m_lock = new object();

        private readonly ObservableCollection<Log> m_logs;

        private LogManager()
        {
            m_logs = new ObservableCollection<Log>();
            Settings.Default.SettingChanging += Default_SettingChanging;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static LogManager Instance => m_instance ?? (m_instance = new LogManager());

        public IEnumerable<Log> FilteredLogs => m_logs.Where(log => log.Level >= Settings.Default.LogLevel);

        public void Add(string message, LogLevel level = LogLevel.Low)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

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
            if (e.InnerException != null)
            {
                Add(e.InnerException);
            }
        }

        public void Clear()
        {
            lock (m_lock)
            {
                m_logs.Clear();
                Default_SettingChanging(null, null);
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
                    m_logs.Insert(0, log);
                    int count = FilteredLogs.Count();
                    while (count-- > Settings.Default.MaxLogs)
                    {
                        m_logs.RemoveAt(m_logs.Count - 1);
                    }
                }
            }));
            Default_SettingChanging(null, null);
        }

        private void Default_SettingChanging(object sender, SettingChangingEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(PropertyChanged, this, new PropertyChangedEventArgs("FilteredLogs"));
        }
    }
}
