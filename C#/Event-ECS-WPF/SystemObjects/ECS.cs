using Event_ECS_WPF.Commands;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Event_ECS_WPF.SystemObjects
{
    public partial class ECS : NotifyPropertyChanged
    {
        private static ECS s_instance;

        private string _appName = string.Empty;

        private bool m_bIsConnected = false;

        private ActionCommand m_resetProjectCommand;

        public static ECS Instance => s_instance ?? (s_instance = new ECS());

        public string AppName
        {
            get => _appName;
            set
            {
                if(string.IsNullOrWhiteSpace(value) || _appName == value)
                {
                    return;
                }

                _appName = value;
                lock (m_lock)
                {
                    Dispose();
                    TryConnect();
                }
                OnPropertyChanged();
            }
        }

        public bool IsConnected
        {
            get => m_bIsConnected;
            set
            {
                m_bIsConnected = value;
                OnPropertyChanged();
            }
        }

        public ICommand ResetProjectCommand => m_resetProjectCommand ?? (m_resetProjectCommand = new ActionCommand(Reset));

        public bool TargetAppIsRunning => Process.GetProcessesByName(AppName).Any();

        #region IECS
        public void AddComponent(string systemName, int entityID, string componentName)
        {
            Send("AddComponent|{0}|{1}|{2}", systemName, entityID, componentName);
        }

        public void AddEntity(string systemName)
        {
            Send("AddEntity|{0}", systemName);
        }

        public void BroadcastEvent(string eventName)
        {
            Send("BroadcastEvent|{0}", eventName);
        }

        public void DispatchEvent(string systemName, string eventName)
        {
            Send("DispatchEvent|{0}|{1}", systemName, eventName);
        }

        public void DispatchEvent(string systemName, int entityID, string eventName)
        {
            Send("DispatchEventEntity|{0}|{1}|{2}", systemName, entityID, eventName);
        }

        public void Execute(string code)
        {
            Send("Execute|{0}", code);
        }

        public void ReloadModule(string modName)
        {
            Send("ReloadModule|{0}", modName);
        }

        public void RemoveComponent(string systemName, int entityID, int componentID)
        {
            Send("RemoveComponent|{0}|{1}|{2}", systemName, entityID, componentID);
        }

        public void RemoveEntity(string systemName, int entityID)
        {
            Send("RemoveEntity|{0}|{1}", systemName, entityID);
        }

        public void Reset()
        {
            Send("Reset");
        }

        public void SetComponentValue(string systemName, int entityID, int componentID, string key, object value)
        {
            Send("SetComponentValue|{0}|{1}|{2}|{3}|{4}", systemName, entityID, componentID, key, value);
        }

        public void SetEntityValue(string systemName, int entityID, string key, object value)
        {
            Send("SetEntityValue|{0}|{1}|{2}|{3}", systemName, entityID, key, value);
        }

        public void SetSystemValue(string systemName, string key, object value)
        {
            Send("SetSystemValue|{0}|{1}|{2}", systemName, key, value);
        }
        #endregion
    }
}
