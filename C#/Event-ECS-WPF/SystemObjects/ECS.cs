using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Properties;
using EventECSWrapper;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Event_ECS_WPF.SystemObjects
{
    public delegate void AutoUpdateChanged(object sender, AutoUpdateChangedArgs e);

    public delegate void DoActionOnMainThreadDelegate(Action action);

    public delegate void DisposeDelegate();

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
        private ECSWrapper m_ecs;
        static ECS()
        {
            ECSWrapper.LogEvent = str => LogManager.Instance.Add(str);
        }

        internal ECS(){}

        public static event AutoUpdateChanged OnAutoUpdateChanged;
        public static ECS Instance => s_instance ?? (s_instance = new ECS());
        public bool ProjectStarted => m_ecs != null;

        public void CreateInstance()
        {
            lock (m_lock)
            {
                Dispose();
                m_ecs = new ECSWrapper();
                m_ecs.OnMainThread += DoOnMainThread;
                LogManager.Instance.Add(LogLevel.Medium, "Project Started");
            }
        }

        public void Dispose()
        {
            lock (m_lock)
            {
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

        public bool InitializeLove(string name)
        {
            lock (m_lock)
            {
                if (m_ecs == null)
                {
                    return false;
                }
                UseWrapper(ecs =>
                    ecs.InitializeLove(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name),
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

        private void DoAction(Action action)
        {
            action();
        }

        private void DoOnMainThread(Action action)
        {
            try
            {
                DoActionOnMainThreadDelegate d = DoAction;
                DispatcherOperation operation = Application.Current.Dispatcher.BeginInvoke(Settings.Default.LoveUpdatePriority, d, action);
                while (operation.Wait(WaitTimeSpan) != DispatcherOperationStatus.Completed && operation.Status != DispatcherOperationStatus.Aborted)
                {
                    if (m_ecs == null || m_ecs.GetAutoUpdate() == false || m_ecs.IsDisposing())
                    {
                        int tries = 0;
                        while (!operation.Abort())
                        {
                            if (++tries < 3)
                            {
                                operation.Priority = DispatcherPriority.Normal;
                            }
                            else
                            {
                                throw new Exception("Must exit this thread!");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogManager.Instance.Add(LogLevel.High, e.Message);
            }
        }

        private void DoDispose()
        {
            m_ecs.Dispose();
            m_ecs = null;
            LogManager.Instance.Add(LogLevel.Medium, "Project Stopped");
        }
    }
}
