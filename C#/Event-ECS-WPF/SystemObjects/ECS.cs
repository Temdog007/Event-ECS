using Event_ECS_WPF.Logger;
using EventECSWrapper;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Event_ECS_WPF.SystemObjects
{
    public delegate void AutoUpdateChanged(object sender, AutoUpdateChangedArgs e);

    public delegate void DisposeDelegate();

    public delegate void DoActionOnMainThreadDelegate(Action action);

    public delegate void LogDelegate(string message);
    public class AutoUpdateChangedArgs : EventArgs
    {
        public AutoUpdateChangedArgs(bool autoUpdate)
        {
            AutoUpdate = autoUpdate;
        }

        public bool AutoUpdate { get; private set; }
    }

    public class ECS : NotifyPropertyChanged, IDisposable
    {
        private const string DeserializeLog = "Deserialize";

        private static readonly TimeSpan WaitTimeSpan = TimeSpan.FromMilliseconds(10);

        private static ECS s_instance;

        private readonly object m_lock = new object();

        private ECSWrapper m_ecs;

        static ECS()
        {
            ECSWrapper.LogEvent = HandleLogFromECS;
        }
        
        internal ECS(){}

        private double LoveDraw(ECSWrapper ecs)
        {
            return ecs.LoveDraw();
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            UseWrapper(LoveDraw, out double sleepMilliseconds);
        }

        public static event Action DeserializeRequested;
        public static event AutoUpdateChanged OnAutoUpdateChanged;

        public static ECS Instance => s_instance ?? (s_instance = new ECS());

        public bool ProjectStarted => m_ecs != null;
        
        public void CreateInstance(string code)
        {
            lock (m_lock)
            {
                Dispose();
                m_ecs = new ECSWrapper(code);
                LogManager.Instance.Add(LogLevel.Medium, "Project Started");
            }
        }

        public void CreateInstance(string code, string path, string name)
        {
            lock (m_lock)
            {
                Dispose();
                m_ecs = new ECSWrapper(code, path, name);
                CompositionTarget.Rendering += CompositionTarget_Rendering;
                LogManager.Instance.Add(LogLevel.Medium, "Project Started");
            }
        }

        public void Dispose()
        {
            lock (m_lock)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
                if (m_ecs != null)
                {
                    DisposeDelegate d = DoDispose;
                    Application.Current.Dispatcher.BeginInvoke(d);
                }
            }
        }

        public bool GetAutoUpdate()
        {
            lock (m_lock)
            {
                return m_ecs?.GetAutoUpdate() ?? false;
            }
        }

        public void SetAutoUpdate(bool value)
        {
            lock (m_lock)
            {
                m_ecs?.SetAutoUpdate(value);
                OnAutoUpdateChanged?.Invoke(this, new AutoUpdateChangedArgs(value));
            }
        }

        public void Update()
        {
            lock (m_lock)
            {
                if (m_ecs == null)
                {
                    return;
                }
                UseWrapper(UpdateAction);
                LogManager.Instance.Add(LogLevel.Low, "Manual Update");
            }
        }

        public bool UseWrapper<T>(Func<ECSWrapper, T> action, out T t)
        {
            lock (m_lock)
            {
                if (m_ecs == null)
                {
                    LogManager.Instance.Add(LogLevel.High, "Project has not been started. Cannot run function");
                    t = default(T);
                    return false;
                }
                else
                {
                    t = Application.Current.Dispatcher.Invoke(() => action(m_ecs));
                    return true;
                }
            }
        }

        public bool UseWrapper<K>(Action<ECSWrapper, K> action, K argument)
        {
            lock (m_lock)
            {
                if (m_ecs == null)
                {
                    LogManager.Instance.Add(LogLevel.High, "Project has not been started. Cannot run function");
                    return false;
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => action(m_ecs, argument)));
                    return true;
                }
            }
        }

        public bool UseWrapper<T, K>(Func<ECSWrapper, K, T> action, K argument, out T t)
        {
            lock (m_lock)
            {
                if (m_ecs == null)
                {
                    LogManager.Instance.Add(LogLevel.High, "Project has not been started. Cannot run function");
                    t = default(T);
                    return false;
                }
                else
                {
                    t = Application.Current.Dispatcher.Invoke(() => action(m_ecs, argument));
                    return true;
                }
            }
        }

        public bool UseWrapper(Action<ECSWrapper> action)
        {
            lock (m_lock)
            {
                if (m_ecs == null)
                {
                    LogManager.Instance.Add(LogLevel.High, "Project has not been started. Cannot run function");
                    return false;
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        try { action(m_ecs); }
                        catch (Exception e) { LogManager.Instance.Add(e.Message, LogLevel.High); }
                    });
                    return true;
                }
            }
        }

        private static void HandleLogFromECS(string message)
        {
            LogManager.Instance.Add(message);
            if (message == DeserializeLog)
            {
                DeserializeRequested?.Invoke();
            }
        }

        private static void UpdateAction(ECSWrapper ecs)
        {
            ecs.LoveUpdate();
        }
        
        private void DoDispose()
        {
            lock (m_lock)
            {
                m_ecs?.Dispose();
                m_ecs = null;
                LogManager.Instance.Add(LogLevel.Medium, "Project Stopped");
            }
        }
    }
}
