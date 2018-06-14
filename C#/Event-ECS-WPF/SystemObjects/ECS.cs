using Event_ECS_Lib;
using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Windows;
using System.Windows.Input;

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

    public class ECS : ECS_Callback
    {
        private const string DeserializeLog = "Deserialize";

        private const string EventQuit = "eventquit";

        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

        private static ECS s_instance;

        private readonly object m_lock = new object();

        private IECSWrapper m_channel;

        private ActionCommand m_resetProjectCommand;

        internal ECS() { }

        public static event Action DeserializeRequested;

        public static event Action ProjectEnded;

        public event AutoUpdateChanged OnAutoUpdateChanged;

        public event Action ProjectRestarted;

        public static ECS Instance => s_instance ?? (s_instance = new ECS());

        public bool ProjectStarted { get; private set; }

        public ICommand ResetProjectCommand => m_resetProjectCommand ?? (m_resetProjectCommand = new ActionCommand(ResetProject));

        private static NetNamedPipeBinding Binding => new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
        {
            OpenTimeout = Timeout,
            CloseTimeout = Timeout,
            ReceiveTimeout = Timeout,
            SendTimeout = Timeout
        };

        private static EndpointAddress Endpoint => new EndpointAddress(ECSWrapperValues.ECSAddress);

        private bool ChannelValid
        {
            get
            {
                if (m_channel == null)
                {
                    return false;
                }

                var channel = m_channel as IChannelFactory;
                switch(channel.State)
                {
                    case CommunicationState.Closed:
                    case CommunicationState.Closing:
                    case CommunicationState.Faulted:
                        return false;
                }
                return true;
            }
        }

        private IECSWrapper Channel
        {
            get
            {
                try
                {
                    if (!ChannelValid)
                    {
                        StartECSApp();
                        Thread.Sleep(100);

                        m_channel = DuplexChannelFactory<IECSWrapper>.CreateChannel(Context, Binding, Endpoint);
                        var channel = m_channel as IChannelFactory;
                        
                        channel.Closed += (o, e) =>
                        {
                            LogManager.Instance.Add("Channel is closed. Must restart the application.", LogLevel.High);
                            m_channel = null;
                        };

                        channel.Faulted += (o, e) =>
                        {
                            LogManager.Instance.Add("Channel is faulted. Must restart the application.", LogLevel.High);
                            m_channel = null;
                        };

                        channel.Closing += (o, e) =>
                        {
                            LogManager.Instance.Add("Channel is closing. Must restart the application.", LogLevel.High);
                            m_channel = null;
                        };

                        channel.Opened += (o, e) => LogManager.Instance.Add("Channel is opened", LogLevel.High);
                        channel.Opening += (o, e) => LogManager.Instance.Add("Channel is opening", LogLevel.High);

                        LogManager.Instance.Add("Connected to ECS application", LogLevel.Medium);
                    }
                }
                catch (Exception e)
                {
                    LogManager.Instance.Add(e);
                }
                return m_channel;
            }
        }

        private InstanceContext Context => new InstanceContext(this);

        public void CreateInstance(string code)
        {
            lock (m_lock)
            {
                Dispose();
                Channel.Initialize(code);
                LogManager.Instance.Add(LogLevel.Medium, "Project Started");
            }
        }

        public void CreateInstance(string code, string path, string name)
        {
            lock (m_lock)
            {
                Dispose();
                Channel.Initialize(code, path, name);
                LogManager.Instance.Add(LogLevel.Medium, "Project Started");
            }
        }

        public override void Dispose()
        {
            lock (m_lock)
            {
                if (m_channel != null)
                {
                    DisposeDelegate d = DoDispose;
                    Application.Current.Dispatcher.BeginInvoke(d);
                }
            }
        }

        public override void LogEvent(string message)
        {
            LogManager.Instance.Add(message);
            if (message == DeserializeLog)
            {
                DeserializeRequested?.Invoke();
            }
            else if (message.Contains(EventQuit))
            {
                ProjectEnded?.Invoke();
            }
        }

        public void ResetProject()
        {
            if (UseWrapper(ResetProjectFunc))
            {
                ProjectRestarted?.Invoke();
                LogManager.Instance.Add("Project restarted", LogLevel.Medium);
            }
        }

        public bool GetAutoUpdate() => AutoUpdate;

        public void SetAutoUpdate(bool value)
        {
            lock (m_lock)
            {
                Channel?.SetAutoUpdate(value);
                AutoUpdate = value;
                OnAutoUpdateChanged?.Invoke(this, new AutoUpdateChangedArgs(value));
            }
        }

        private bool AutoUpdate { get; set; } = false;

        public void Update()
        {
            lock (m_lock)
            {
                if (Channel == null)
                {
                    return;
                }
                UseWrapper(UpdateAction);
                LogManager.Instance.Add(LogLevel.Low, "Manual Update");
            }
        }

        public bool UseWrapper(Action<IECSWrapper> action, Func<string, object, bool> listener)
        {
            lock (m_lock)
            {
                if (Channel == null)
                {
                    LogManager.Instance.Add(LogLevel.High, "Project has not been started. Cannot run function");
                    return false;
                }
                else
                {
                    Listeners.Add(listener);
                    action(Channel);
                    return true;
                }
            }
        }

        public bool UseWrapper<K>(Action<IECSWrapper, K> action, K argument)
        {
            lock (m_lock)
            {
                if (Channel == null)
                {
                    LogManager.Instance.Add(LogLevel.High, "Project has not been started. Cannot run function");
                    return false;
                }
                else
                {
                    action(Channel, argument);
                    return true;
                }
            }
        }

        public bool UseWrapper<T, K>(Func<IECSWrapper, K, T> action, K argument, out T t)
        {
            lock (m_lock)
            {
                if (Channel == null)
                {
                    LogManager.Instance.Add(LogLevel.High, "Project has not been started. Cannot run function");
                    t = default(T);
                    return false;
                }
                else
                {
                    t = action(Channel, argument);
                    return true;
                }
            }
        }

        public bool UseWrapper(Action<IECSWrapper> action)
        {
            lock (m_lock)
            {
                if (Channel == null)
                {
                    LogManager.Instance.Add(LogLevel.High, "Project has not been started. Cannot run function");
                    return false;
                }
                else
                {
                    action(Channel);
                    return true;
                }
            }
        }

        private void DoDispose()
        {
            lock (m_lock)
            {
                m_channel?.Dispose();
                m_channel = null;
                LogManager.Instance.Add(LogLevel.Medium, "Project Stopped");
            }
        }

        private void ResetProjectFunc(IECSWrapper ecs)
        {
            ecs.Reset();
            ProjectRestarted?.Invoke();
        }

        private void StartECSApp()
        {
            if(!Process.GetProcessesByName("Event-ECS-App").Any())
            {
                Process.Start("Event-ECS-App.exe");
            }
        }

        private void UpdateAction(IECSWrapper ecs)
        {
            ecs.Update();
        }
    }
}
