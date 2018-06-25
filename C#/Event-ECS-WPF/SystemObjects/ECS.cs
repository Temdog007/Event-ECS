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
            Send("DispatchEvent|{0}|{1}|{2}", systemName, entityID, eventName);
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
            Send("RemoveComponent|{0}|{1}", systemName, entityID);
        }

        public void Reset()
        {
            Send("Reset");
        }

        public void SetComponentBool(string systemName, int entityID, int componentID, string key, bool value)
        {
            Send("SetComponentBool|{0}|{1}|{2}|{3}|{4}", systemName, entityID, componentID, key, value);
        }

        public void SetComponentEnabled(string systemName, int entityID, int componentID, bool value)
        {
            Send("SetComponentEnabled|{0}|{1}|{2}|{3}", systemName, entityID, componentID, value);
        }

        public void SetComponentNumber(string systemName, int entityID, int componentID, string key, double value)
        {
            Send("SetComponentNumber|{0}|{1}|{2}|{3}|{4}", systemName, entityID, componentID, key, value);
        }

        public void SetComponentString(string systemName, int entityID, int componentID, string key, string value)
        {
            Send("SetComponentString|{0}|{1}|{2}|{3}|{4}", systemName, entityID, componentID, key, value);
        }

        public void SetEntityBool(string systemName, int entityID, string key, bool value)
        {
            Send("SetEntityBool|{0}|{1}|{2}|{3}", systemName, entityID, key, value);
        }

        public void SetEntityEnabled(string systemName, int entityID, bool value)
        {
            Send("SetEntityEnabled|{0}|{1}|{2}", systemName, entityID, value);
        }

        public void SetEntityNumber(string systemName, int entityID, string key, double value)
        {
            Send("SetEntityNumber|{0}|{1}|{2}|{3}", systemName, entityID, key, value);
        }

        public void SetEntityString(string systemName, int entityID, string key, string value)
        {
            Send("SetEntityString|{0}|{1}|{2}|{3}", systemName, entityID, key, value);
        }

        public void SetSystemBool(string systemName, string key, bool value)
        {
            Send("SetSystemBool|{0}|{1}|{2}", systemName, key, value);
        }

        public void SetSystemEnabled(string systemName, bool value)
        {
            Send("SetSystemEnabled|{0}|{1}", systemName, value);
        }

        public void SetSystemNumber(string systemName, string key, double value)
        {
            Send("SetSystemNumber|{0}|{1}|{2}", systemName, key, value);
        }

        public void SetSystemString(string systemName, string key, string value)
        {
            Send("SetSystemString|{0}|{1}|{2}", systemName, key, value);
        }
        #endregion
    }
}
