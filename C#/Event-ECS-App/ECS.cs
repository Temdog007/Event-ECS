using Event_ECS_Lib;
using EventECSWrapper;
using System;
using System.ServiceModel;

namespace Event_ECS_App
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.Single)]
    public class ECS : IECSWrapper
    {
        private const string DeserializeLog = "Deserialize";

        private static ECSWrapper ecs;

        private IECSWrapperCallback m_callback;

        public IECSWrapperCallback Callback
        {
            get
            {
                if(m_callback is ICommunicationObject obj)
                {
                    switch(obj.State)
                    {
                        case CommunicationState.Faulted:
                        case CommunicationState.Closed:
                        case CommunicationState.Closing:
                            m_callback = null;
                            break;
                    }
                }
                return m_callback ?? (m_callback = OperationContext.Current?.GetCallbackChannel<IECSWrapperCallback>());
            }
        }

        private static readonly object s_lock = new object();

        public bool CanUpdate { get; set; } = true;

        public bool DisposeRequested { get; set; } = false;

        public bool HasStarted => ecs != null;

        public void AddComponent(string systemName, int entityID, string componentName)
        {
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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

        public void Uninitialize()
        {
            DisposeRequested = true;
        }

        internal void Stop()
        {
            if (ecs == null) { return; }
            try
            {
                ecs?.Dispose();
                ecs = null;
                DisposeRequested = false;
                Log("Project stopped");
            }
            catch (Exception e)
            {
                Log(e);
            }
            finally
            {
                ECSWrapper.LogEvent -= Log;
            }
        }

        public void Execute(string code)
        {
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
            try
            {
                ecs.Execute(code, systemName);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void GetClassName(string systemName)
        {
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs != null) { return; }
            try
            {
                ECSWrapper.LogEvent += Log;
                ecs = new ECSWrapper(initializerCode);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void Initialize(string initializerCode, string executablePath, string identity)
        {
            if (ecs != null) { return; }
            try
            {
                ECSWrapper.LogEvent += Log;
                ecs = new ECSWrapper(initializerCode, executablePath, identity);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void IsComponentEnabled(string systemName, int entityID, int componentID)
        {
            if (ecs == null) { return; }
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

        public void IsEntityEnabled(string systemName, int entityID)
        {
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            try
            {
                CanUpdate = value;
                Callback?.IsUpdatingAutomatically(value);
            }
            catch(Exception e)
            {
                Log(e);
            }
        }

        public void SetComponentBool(string systemName, int entityID, int componentID, string key, bool value)
        {
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
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
            if (ecs == null) { return; }
            try
            {
                ecs.Unregister(modName);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void LoveUpdate()
        {
            if (ecs == null) { return; }
            try
            {
                ecs.UpdateLove();
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public void LoveUpdate(out int returnValue)
        {
            returnValue = -1;
            if (ecs == null) { return; }
            try
            {
                returnValue =  ecs.UpdateLove();
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
                IECSWrapperCallback callback = Callback;
                callback?.IsUpdatingAutomatically(CanUpdate);
                callback?.IsStarted(ecs != null);
                Serialize();
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        private void Log(string message)
        {
            lock (s_lock)
            {
                if (message == DeserializeLog)
                {
                    Serialize();
                }
                else
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