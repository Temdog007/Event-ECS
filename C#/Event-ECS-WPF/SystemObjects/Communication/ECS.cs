using Event_ECS_WPF.Commands;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Event_ECS_WPF.SystemObjects.Communication
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
        public void AddComponent(int systemID, int entityID, string componentName)
        {
            Send("AddComponent|{0}|{1}|{2}", systemID, entityID, componentName);
        }

        public void AddEntity(int systemID)
        {
            Send("AddEntity|{0}", systemID);
        }

        public void BroadcastEvent(string eventName)
        {
            Send("BroadcastEvent|{0}", eventName);
        }

        public void DispatchEvent(int systemID, string eventName)
        {
            Send("DispatchEvent|{0}|{1}", systemID, eventName);
        }

        public void DispatchEvent(int systemID, int entityID, string eventName)
        {
            Send("DispatchEventEntity|{0}|{1}|{2}", systemID, entityID, eventName);
        }

        public void Execute(string code)
        {
            Send("Execute|{0}", code);
        }

        public void ReloadModule(string modName)
        {
            Send("ReloadModule|{0}", modName);
        }

        public void RemoveComponent(int systemID, int entityID, int componentID)
        {
            Send("RemoveComponent|{0}|{1}|{2}", systemID, entityID, componentID);
        }

        public void RemoveEntity(int systemID, int entityID)
        {
            Send("RemoveEntity|{0}|{1}", systemID, entityID);
        }

        public void Reset()
        {
            Send("Reset");
        }

        public void SetComponentEnabled(int systemID, int entityID, int componentID, bool value)
        {
            Send("SetComponentEnabled|{0}|{1}|{2}|{3}", systemID, entityID, componentID, value);
        }

        public void SetEntityValue(int systemID, int entityID, string key, object value)
        {
            Send("SetEntityValue|{0}|{1}|{2}|{3}", systemID, entityID, key, value);
        }

        public void SetSystemEnabled(int systemID, bool value)
        {
            Send("SetSystemEnabled|{0}|{1}", systemID, value);
        }
        #endregion
    }
}
