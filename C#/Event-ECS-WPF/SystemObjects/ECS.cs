using Event_ECS_Lib;
using Event_ECS_WPF.Commands;
using Event_ECS_WPF.Logger;
using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Event_ECS_WPF.SystemObjects
{
    public delegate void DisposeDelegate();
    
    public class ECS : ECS_Callback
    {
        private const string DeserializeLog = "Deserialize";

        private const string EventQuit = "eventquit";

        private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);

        private static ECS s_instance;

        private readonly object m_lock = new object();

        private IECSWrapper m_channel;

        private bool m_isUpdatingAutomatically = false;

        private bool m_isApplicationRunning = false;

        private ActionCommand m_resetProjectCommand;

        internal ECS()
        {
            Listeners.Add(UpdateApplicationRunning);
        }

        public static event Action DeserializeRequested;

        public static ECS Instance => s_instance ?? (s_instance = new ECS());

        public bool IsApplicationRunning
        {
            get => m_isApplicationRunning;
            set
            {
                m_isApplicationRunning = value;
                OnPropertyChanged();
            }
        }

        public bool IsUpdatingAutomaticallyProperty
        {
            get => m_isUpdatingAutomatically;
            set
            {
                if (m_isApplicationRunning == value)
                {
                    return;
                }
                m_isUpdatingAutomatically = value;
                OnPropertyChanged();
                Channel?.SetAutoUpdate(IsUpdatingAutomaticallyProperty);
            }
        }

        public ICommand ResetProjectCommand => m_resetProjectCommand ?? (m_resetProjectCommand = new ActionCommand(ResetProject));

        private static NetNamedPipeBinding Binding => new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
        {
            OpenTimeout = Timeout,
            CloseTimeout = Timeout,
            ReceiveTimeout = Timeout,
            SendTimeout = Timeout
        };

        private static EndpointAddress Endpoint => new EndpointAddress(ECSWrapperValues.ECSAddress);

        private IECSWrapper Channel => GetChannel();

        private bool ChannelValid
        {
            get
            {
                if (m_channel == null)
                {
                    return false;
                }

                if (!(m_channel is ICommunicationObject channel))
                {
                    return false;
                }

                switch (channel.State)
                {
                    case CommunicationState.Closed:
                    case CommunicationState.Closing:
                    case CommunicationState.Faulted:
                        return false;
                }
                return true;
            }
        }

        private InstanceContext Context => new InstanceContext(this);

        public void CreateInstance(string code)
        {
            lock (m_lock)
            {
                Dispose();
                GetChannel(true).Initialize(code);
                LogManager.Instance.Add(LogLevel.Medium, "Project Started");
            }
        }

        public void CreateInstance(string code, string path, string name)
        {
            lock (m_lock)
            {
                Dispose();
                GetChannel(true).Initialize(code, path, name);
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
                Channel.Dispose();
            }
        }

        public void ResetProject()
        {
            if (UseWrapper(ResetProjectFunc))
            {
                LogManager.Instance.Add("Project restarted", LogLevel.Medium);
            }
        }

        public void SetAutoUpdate(bool value)
        {
            Channel?.SetAutoUpdate(value);
        }

        public void Update()
        {
            lock (m_lock)
            {
                if (!ChannelValid)
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
                if (!ChannelValid)
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
                if (!ChannelValid)
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
                if (ChannelValid)
                {
                    m_channel?.Dispose();
                    m_channel = null;
                    LogManager.Instance.Add(LogLevel.Medium, "Project Stopped");
                }
            }
        }

        private IECSWrapper GetChannel(bool create = false)
        {
            try
            {
                if (create)
                {
                    if (ChannelValid)
                    {
                        DoDispose();
                    }

                    StartECSApp();
                    Thread.Sleep(100);

                    m_channel = DuplexChannelFactory<IECSWrapper>.CreateChannel(Context, Binding, Endpoint);
                    var channel = m_channel as ICommunicationObject;

                    channel.Closed += (o, e) =>
                    {
                        LogManager.Instance.Add("Channel is closed.");
                        m_channel = null;
                    };

                    channel.Faulted += (o, e) =>
                    {
                        LogManager.Instance.Add("Channel is faulted.", LogLevel.High);
                        m_channel = null;
                    };

                    channel.Closing += (o, e) =>
                    {
                        LogManager.Instance.Add("Channel is closing.");
                        m_channel = null;
                    };

                    channel.Opened += (o, e) => LogManager.Instance.Add("Channel is opened");
                    channel.Opening += (o, e) => LogManager.Instance.Add("Channel is opening");

                    LogManager.Instance.Add("Connected to ECS application", LogLevel.Medium);
                }
            }
            catch (Exception e)
            {
                LogManager.Instance.Add(e);
            }
            return m_channel;
        }

        private void ResetProjectFunc(IECSWrapper ecs)
        {
            ecs.Reset();
        }

        private void StartECSApp()
        {
            if (!Process.GetProcessesByName("Event-ECS-App").Any())
            {
                Process.Start("Event-ECS-App.exe");
            }
        }

        private void UpdateAction(IECSWrapper ecs)
        {
            ecs.Update();
        }

        private bool UpdateApplicationRunning(string funcName, object result)
        {
            switch (funcName)
            {
                case "IsStarted":
                    IsApplicationRunning = (bool)result;
                    break;
                case "IsUpdatingAutomatically":
                    IsUpdatingAutomaticallyProperty = (bool)result;
                    break;
            }
            return false;
        }
    }
}
