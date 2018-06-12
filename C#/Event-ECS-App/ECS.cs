using Event_ECS_Lib;
using EventECSWrapper;
using System;
using System.ServiceModel;

namespace Event_ECS_App
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.Single)]
    public class ECS : IECSWrapper
    {
        private ECSWrapper ecs;

        public ECS()
        {
            ECSWrapper.LogEvent += Log;
        }

        public IECSWrapperCallback Callback => OperationContext.Current?.GetCallbackChannel<IECSWrapperCallback>();

        public bool CanUpdate { get; set; } = true;

        public void AddComponent(string systemName, int entityID, string componentName)
        {
            try
            {
                ecs.AddComponent(systemName, entityID, componentName);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void AddComponents(string systemName, int entityID, string[] componentNames)
        {
            try
            {
                ecs.AddComponents(systemName, entityID, componentNames);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public string AddEntity(string systemName)
        {
            try
            {
                return ecs.AddEntity(systemName);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return string.Empty;
            }
        }

        public void BroadcastEvent(string eventName)
        {
            try
            {
                ecs.BroadcastEvent(eventName);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public int DispatchEvent(string systemName, string eventName)
        {
            try
            {
                return ecs.DispatchEvent(systemName, eventName);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return -1;
            }
        }

        public int DispatchEvent(string systemName, int entityID, string eventName)
        {
            try
            {
                return ecs.DispatchEvent(systemName, entityID, eventName);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return -1;
            }
        }

        public void Dispose()
        {
            try
            {
                ecs?.Dispose();
                ecs = null;
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void Execute(string code)
        {
            try
            {
                ecs.Execute(code);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void Execute(string code, string systemName)
        {
            try
            {
                ecs.Execute(code, systemName);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public bool GetAutoUpdate()
        {
            return CanUpdate;
        }

        public string GetClassName(string systemName)
        {
            try
            {
                return ecs.GetClassName(systemName);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return string.Empty;
            }
        }

        public bool GetComponentBool(string systemName, int entityID, int componentID, string key)
        {
            try
            {
                return ecs.GetComponentBool(systemName, entityID, componentID, key);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return false;
            }
        }

        public double GetComponentNumber(string systemName, int entityID, int componentID, string key)
        {
            try
            {
                return ecs.GetComponentNumber(systemName, entityID, componentID, key);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return 0;
            }
        }

        public double GetComponentNumber(string systemName, int entityID, int componentID, int key)
        {
            try
            {
                return ecs.GetComponentNumber(systemName, entityID, componentID, key);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return 0;
            }
        }

        public string GetComponentString(string systemName, int entityID, int componentID, string key)
        {
            try
            {
                return ecs.GetComponentString(systemName, entityID, componentID, key);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return string.Empty;
            }
        }

        public bool GetEntityBool(string systemName, int entityID, string key)
        {
            try
            {
                return ecs.GetEntityBool(systemName, entityID, key);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return false;
            }
        }

        public double GetEntityNumber(string systemName, int entityID, string key)
        {
            try
            {
                return ecs.GetEntityNumber(systemName, entityID, key);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return 0;
            }
        }

        public string GetEntityString(string systemName, int entityID, string key)
        {
            try
            {
                return ecs.GetEntityString(systemName, entityID, key);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return string.Empty;
            }
        }

        public bool GetSystemBool(string systemName, string key)
        {
            try
            {
                return ecs.GetSystemBool(systemName, key);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return false;
            }
        }

        public double GetSystemNumber(string systemName, string key)
        {
            try
            {
                return ecs.GetSystemNumber(systemName, key);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return 0;
            }
        }

        public string GetSystemString(string systemName, string key)
        {
            try
            {
                return ecs.GetSystemString(systemName, key);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return string.Empty;
            }
        }

        public void Initialize(string initializerCode)
        {
            try
            {
                ecs = new ECSWrapper(initializerCode);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void Initialize(string initializerCode, string executablePath, string identity)
        {
            try
            {
                ecs = new ECSWrapper(initializerCode, executablePath, identity);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public bool IsComponentEnabled(string systemName, int entityID, int componentID)
        {
            try
            {
                return ecs.IsComponentEnabled(systemName, entityID, componentID);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return false;
            }
        }

        public bool IsDisposing()
        {
            try
            {
                return ecs.IsDisposing();
            }
            catch (Exception e)
            {
                Log(e.Message);
                return true;
            }
        }

        public bool IsEntityEnabled(string systemName, int entityID)
        {
            try
            {
                return ecs.IsEntityEnabled(systemName, entityID);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return false;
            }
        }

        public bool IsLoggingEvents()
        {
            try
            {
                return ecs.IsLoggingEvents();
            }
            catch (Exception e)
            {
                Log(e.Message);
                return false;
            }
        }

        public bool IsStarted()
        {
            try
            {
                return ecs != null;
            }
            catch (Exception e)
            {
                Log(e.Message);
                return false;
            }
        }

        public bool IsSystemEnabled(string systemName)
        {
            try
            {
                return ecs.IsSystemEnabled(systemName);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return false;
            }
        }

        public bool RemoveComponent(string systemName, int entityID, int componentID)
        {
            try
            {
                return ecs.RemoveComponent(systemName, entityID, componentID);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return false;
            }
        }

        public bool RemoveEntity(string systemName, int entityID)
        {
            try
            {
                return ecs.RemoveEntity(systemName, entityID);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return false;
            }
        }

        public void Reset()
        {
            try
            {
                ecs.Reset();
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public string[] Serialize()
        {
            try
            {
                return ecs.Serialize();
            }
            catch (Exception e)
            {
                Log(e.Message);
                return string.Empty.Split('\n');
            }
        }

        public string SerializeComponent(string systemName, int entityID, int componentID)
        {
            try
            {
                return ecs.SerializeComponent(systemName, entityID, componentID);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return string.Empty;
            }
        }

        public string SerializeEntity(string systemName, int entityID)
        {
            try
            {
                return ecs.SerializeEntity(systemName, entityID);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return string.Empty;
            }
        }

        public string SerializeSystem(string systemName)
        {
            try
            {
                return ecs.SerializeSystem(systemName);
            }
            catch (Exception e)
            {
                Log(e.Message);
                return string.Empty;
            }
        }

        public void SetAutoUpdate(bool value)
        {
            CanUpdate = value;
        }

        public void SetComponentBool(string systemName, int entityID, int componentID, string key, bool value)
        {
            try
            {
                ecs.SetComponentBool(systemName, entityID, componentID, key, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetComponentEnabled(string systemName, int entityID, int componentID, bool value)
        {
            try
            {
                ecs.SetComponentEnabled(systemName, entityID, componentID, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetComponentNumber(string systemName, int entityID, int componentID, int key, double value)
        {
            try
            {
                ecs.SetComponentNumber(systemName, entityID, componentID, key, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetComponentNumber(string systemName, int entityID, int componentID, string key, double value)
        {
            try
            {
                ecs.SetComponentNumber(systemName, entityID, componentID, key, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetComponentString(string systemName, int entityID, int componentID, string key, string value)
        {
            try
            {
                ecs.SetComponentString(systemName, entityID, componentID, key, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetEntityBool(string systemName, int entityID, string key, bool value)
        {
            try
            {
                ecs.SetEntityBool(systemName, entityID, key, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetEntityEnabled(string systemName, int entityID, bool value)
        {
            try
            {
                ecs.SetEntityEnabled(systemName, entityID, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetEntityNumber(string systemName, int entityID, string key, double value)
        {
            try
            {
                ecs.SetEntityNumber(systemName, entityID, key, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetEntityString(string systemName, int entityID, string key, string value)
        {
            try
            {
                ecs.SetEntityString(systemName, entityID, key, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetEventsToIgnore(string[] args)
        {
            try
            {
                ecs.SetEventsToIgnore(args);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetLoggingEvents(bool value)
        {
            try
            {
                ecs.SetLoggingEvents(value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetSystemBool(string systemName, string key, bool value)
        {
            try
            {
                ecs.SetSystemBool(systemName, key, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetSystemEnabled(string systemName, bool value)
        {
            try
            {
                ecs.SetSystemEnabled(systemName, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetSystemNumber(string systemName, string key, double value)
        {
            try
            {
                ecs.SetSystemNumber(systemName, key, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void SetSystemString(string systemName, string key, string value)
        {
            try
            {
                ecs.SetSystemString(systemName, key, value);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public void Unregister(string modName)
        {
            try
            {
                ecs.Unregister(modName);
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        public int Update()
        {
            try
            {
                return ecs.UpdateLove();
            }
            catch (Exception e)
            {
                Log(e.Message);
                return -2;
            }
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
            try
            {
                Callback?.LogEvent(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}