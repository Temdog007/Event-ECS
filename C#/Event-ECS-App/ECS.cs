using Event_ECS_Lib;
using EventECSWrapper;
using System;
using System.ServiceModel;

namespace Event_ECS_App
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class ECS : IECSWrapper
    {
        private ECSWrapper ecs;

        public ECS()
        {
            ECSWrapper.LogEvent += Log;
        }

        public event Action Disposing;

        public event Action Starting;

        public event Action<int> Updated;

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
                Log(e);
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
                Log(e);
            }
        }

        public void AddEntity(string systemName)
        {
            try
            {
                string value = ecs.AddEntity(systemName);
                Callback?.AddEntity(value);
            }
            catch (Exception e)
            {
                Log(e);
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
                Log(e);
            }
        }

        public void DispatchEvent(string systemName, string eventName)
        {
            try
            {
                var value = ecs.DispatchEvent(systemName, eventName);
                Callback?.DispatchEvent(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void DispatchEvent(string systemName, int entityID, string eventName)
        {
            try
            {
                var value = ecs.DispatchEvent(systemName, entityID, eventName);
                Callback?.DispatchEvent(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void Dispose()
        {
            try
            {
                ecs?.Dispose();
                ecs = null;
                Disposing?.Invoke();
            }
            catch (Exception e)
            {
                Log(e);
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
                Log(e);
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
                Log(e);
            }
        }

        public void GetAutoUpdate()
        {
            Callback?.GetAutoUpdate(CanUpdate);
        }

        public void GetClassName(string systemName)
        {
            try
            {
                var value = ecs.GetClassName(systemName);
                Callback?.GetClassName(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void GetComponentBool(string systemName, int entityID, int componentID, string key)
        {
            try
            {
                var value = ecs.GetComponentBool(systemName, entityID, componentID, key);
                Callback?.GetComponentBool(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void GetComponentNumber(string systemName, int entityID, int componentID, string key)
        {
            try
            {
                var value = ecs.GetComponentNumber(systemName, entityID, componentID, key);
                Callback?.GetComponentNumber(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void GetComponentNumber(string systemName, int entityID, int componentID, int key)
        {
            try
            {
                var value = ecs.GetComponentNumber(systemName, entityID, componentID, key);
                Callback?.GetComponentNumber(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void GetComponentString(string systemName, int entityID, int componentID, string key)
        {
            try
            {
                var value = ecs.GetComponentString(systemName, entityID, componentID, key);
                Callback?.GetComponentString(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void GetEntityBool(string systemName, int entityID, string key)
        {
            try
            {
                var value = ecs.GetEntityBool(systemName, entityID, key);
                Callback?.GetEntityBool(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void GetEntityNumber(string systemName, int entityID, string key)
        {
            try
            {
                var value = ecs.GetEntityNumber(systemName, entityID, key);
                Callback?.GetEntityNumber(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void GetEntityString(string systemName, int entityID, string key)
        {
            try
            {
                var value = ecs.GetEntityString(systemName, entityID, key);
                Callback?.GetEntityString(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void GetSystemBool(string systemName, string key)
        {
            try
            {
                var value = ecs.GetSystemBool(systemName, key);
                Callback?.GetSystemBool(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void GetSystemNumber(string systemName, string key)
        {
            try
            {
                var value = ecs.GetSystemNumber(systemName, key);
                Callback?.GetSystemNumber(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void GetSystemString(string systemName, string key)
        {
            try
            {
                var value = ecs.GetSystemString(systemName, key);
                Callback?.GetSystemString(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void Initialize(string initializerCode)
        {
            try
            {
                ecs = new ECSWrapper(initializerCode);
                Starting?.Invoke();
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void Initialize(string initializerCode, string executablePath, string identity)
        {
            try
            {
                ecs = new ECSWrapper(initializerCode, executablePath, identity);
                Starting?.Invoke();
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void IsComponentEnabled(string systemName, int entityID, int componentID)
        {
            try
            {
                var value = ecs.IsComponentEnabled(systemName, entityID, componentID);
                Callback?.IsComponentEnabled(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void IsDisposing()
        {
            try
            {
                var value = ecs.IsDisposing();
                Callback?.IsDisposing(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void IsEntityEnabled(string systemName, int entityID)
        {
            try
            {
                var value = ecs.IsEntityEnabled(systemName, entityID);
                Callback?.IsEntityEnabled(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void IsLoggingEvents()
        {
            try
            {
                var value = ecs.IsLoggingEvents();
                Callback?.IsLoggingEvents(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void IsStarted()
        {
            try
            {
                Callback?.IsStarted(ecs != null);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void IsSystemEnabled(string systemName)
        {
            try
            {
                var value = ecs.IsSystemEnabled(systemName);
                Callback?.IsSystemEnabled(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void RemoveComponent(string systemName, int entityID, int componentID)
        {
            try
            {
                var value = ecs.RemoveComponent(systemName, entityID, componentID);
                Callback?.RemoveComponent(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void RemoveEntity(string systemName, int entityID)
        {
            try
            {
                var value = ecs.RemoveEntity(systemName, entityID);
                Callback?.RemoveEntity(value);
            }
            catch (Exception e)
            {
                Log(e);
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
                Log(e);
            }
        }

        public void Serialize()
        {
            try
            {
                var value = ecs.Serialize();
                Callback?.Serialize(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void SerializeComponent(string systemName, int entityID, int componentID)
        {
            try
            {
                var value = ecs.SerializeComponent(systemName, entityID, componentID);
                Callback?.SerializeComponent(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void SerializeEntity(string systemName, int entityID)
        {
            try
            {
                var value = ecs.SerializeEntity(systemName, entityID);
                Callback?.SerializeEntity(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void SerializeSystem(string systemName)
        {
            try
            {
                var value = ecs.SerializeSystem(systemName);
                Callback?.SerializeSystem(value);
            }
            catch (Exception e)
            {
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
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
                Log(e);
            }
        }

        public void Update()
        {
            try
            {
                var value = ecs.UpdateLove();
                Callback?.Update(value);
                Updated?.Invoke(value);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        private void Log(string message)
        {
            lock (this)
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

        private void Log(Exception e)
        {
            Log(e.Message);
            if(e.InnerException != null)
            {
                Log(e.InnerException);
            }
        }
    }
}