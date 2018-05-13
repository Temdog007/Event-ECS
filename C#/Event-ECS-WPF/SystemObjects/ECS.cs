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

        internal ECS() { }

        public static event Action DeserializeRequested;
        public static event AutoUpdateChanged OnAutoUpdateChanged;

        public static ECS Instance => s_instance ?? (s_instance = new ECS());

        public uint FrameRate
        {
            get => UseWrapper(GetFrameRate, out uint fps) ? fps : 0;
            set => UseWrapper(ecs => ecs.SetSystemNumber("frameRate", value));
        }

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
                bool returnRval =
                    UseWrapper(ecs =>
                    ecs.InitializeLove(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name),
                    out bool rval) && rval;
                CompositionTarget.Rendering += CompositionTarget_Rendering;
                return returnRval;
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

        private static void DoHandleLogFromECS(string message)
        {
            LogManager.Instance.Add(message);
            if (message == DeserializeLog)
            {
                DeserializeRequested?.Invoke();
            }
        }

        private static void HandleLogFromECS(string message)
        {
            LogDelegate d = DoHandleLogFromECS;
            Application.Current.Dispatcher.BeginInvoke(d, message);
        }

        private static bool UpdateAction(ECSWrapper ecs)
        {
            return ecs.LoveUpdate();
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            UseWrapper(ecs => ecs.LoveDraw());
        }

        private void DoDispose()
        {
            lock (m_lock)
            {
                m_ecs.Dispose();
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
                m_ecs = null;
                LogManager.Instance.Add(LogLevel.Medium, "Project Stopped");
            }
        }

        private uint GetFrameRate(ECSWrapper ecs)
        {
            return (uint)ecs.GetSystemNumber("frameRate");
        }
    }
}
