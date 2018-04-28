using Event_ECS_WPF.Logger;
using EventECSWrapper;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Event_ECS_WPF.SystemObjects
{
    public delegate void AutoUpdateChanged(object sender, AutoUpdateChangedArgs e);

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
        private static readonly TimeSpan WaitTimeSpan = TimeSpan.FromMilliseconds(10);

        private static ECS s_instance;
        private readonly object m_lock = new object();
        private readonly Func<bool> UpdateFunc;
        private ECSWrapper m_ecs;
        static ECS()
        {
            ECSWrapper.LogEvent = str => LogManager.Instance.Add(str);
        }

        internal ECS()
        {
            UpdateFunc = UpdateAction;
        }

        public static event AutoUpdateChanged OnAutoUpdateChanged;
        public static ECS Instance => s_instance ?? (s_instance = new ECS());
        public bool ProjectStarted => m_ecs != null;

        public void CreateInstance()
        {
            lock (m_lock)
            {
                Dispose();
                m_ecs = new ECSWrapper();
                LogManager.Instance.Add(LogLevel.Medium, "Project Started");
            }
        }

        public void Dispose()
        {
            lock (m_lock)
            {
                if (m_ecs != null)
                {
                    Application.Current.Dispatcher.Invoke(() => m_ecs.Dispose());
                    m_ecs = null;
                    LogManager.Instance.Add(LogLevel.Medium, "Project Stopped");
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

        public bool InitializeLove(string name)
        {
            lock (m_lock)
            {
                if (m_ecs == null)
                {
                    return false;
                }
                UseWrapper(ecs =>
                    ecs.InitializeLove(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name, UpdateOnMainThread),
                    out bool rval);
                return rval;
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

        public bool Update()
        {
            lock (m_lock)
            {
                if (m_ecs == null)
                {
                    return false;
                }
                UseWrapper(UpdateAction, out bool rval);
                LogManager.Instance.Add(LogLevel.Low, "Manual Update");
                return rval;
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

        public void UseWrapper(Action<ECSWrapper> action)
        {
            lock (m_lock)
            {
                if (m_ecs == null)
                {
                    LogManager.Instance.Add(LogLevel.High, "Project has not been started. Cannot run function");
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        try { action(m_ecs); }
                        catch (Exception e) { LogManager.Instance.Add(e.Message, LogLevel.High); }
                    });
                }
            }
        }

        private static bool UpdateAction(ECSWrapper ecs)
        {
            return ecs.LoveUpdate();
        }

        private bool UpdateAction()
        {
            lock (m_lock)
            {
                try
                {
                    return m_ecs == null ? false : m_ecs.LoveUpdate();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        private bool UpdateOnMainThread()
        {
            try
            {
                DispatcherOperation operation = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, UpdateFunc);
                while (operation.Wait(WaitTimeSpan) != DispatcherOperationStatus.Completed)
                {
                    if ((m_ecs?.GetAutoUpdate() ?? false) == false)
                    {
                        return true;
                    }
                }
                return (operation.Result as bool?) ?? false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
