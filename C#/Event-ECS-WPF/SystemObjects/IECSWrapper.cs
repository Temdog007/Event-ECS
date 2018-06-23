namespace Event_ECS_WPF.SystemObjects
{
    public interface IECSWrapper
    {
        void AddComponent(string systemName, int entityID, string componentName);

        void AddComponents(string systemName, int entityID, string[] componentNames);

        void AddEntity(string systemName);

        void BroadcastEvent(string eventName);

        void DispatchEvent(string systemName, string eventName);

        void DispatchEvent(string systemName, int entityID, string eventName);

        void Execute(string code);

        void Execute(string code, string systemName);

        void GetClassName(string systemName);

        void GetComponentBool(string systemName, int entityID, int componentID, string key);

        void GetComponentNumber(string systemName, int entityID, int componentID, string key);

        void GetComponentNumber(string systemName, int entityID, int componentID, int key);

        void GetComponentString(string systemName, int entityID, int componentID, string key);

        void GetEntityBool(string systemName, int entityID, string key);

        void GetEntityNumber(string systemName, int entityID, string key);

        void GetEntityString(string systemName, int entityID, string key);

        void GetSystemBool(string systemName, string key);

        void GetSystemNumber(string systemName, string key);

        void GetSystemString(string systemName, string key);

        void Initialize(string initializerCode);

        void Initialize(string initializerCode, string executablePath, string identity);

        void IsComponentEnabled(string systemName, int entityID, int componentID);

        void IsEntityEnabled(string systemName, int entityID);

        void IsLoggingEvents();

        void IsStarted();

        void IsSystemEnabled(string systemName);

        void LoveUpdate();

        void RemoveComponent(string systemName, int entityID, int componentID);

        void RemoveEntity(string systemName, int entityID);

        void Reset();

        void Serialize();

        void SerializeComponent(string systemName, int entityID, int componentID);

        void SerializeEntity(string systemName, int entityID);

        void SerializeSystem(string systemName);

        void SetAutoUpdate(bool value);

        void SetComponentBool(string systemName, int entityID, int componentID, string key, bool value);

        void SetComponentEnabled(string systemName, int entityID, int componentID, bool value);

        void SetComponentNumber(string systemName, int entityID, int componentID, int key, double value);

        void SetComponentNumber(string systemName, int entityID, int componentID, string key, double value);

        void SetComponentString(string systemName, int entityID, int componentID, string key, string value);

        void SetEntityBool(string systemName, int entityID, string key, bool value);

        void SetEntityEnabled(string systemName, int entityID, bool value);

        void SetEntityNumber(string systemName, int entityID, string key, double value);

        void SetEntityString(string systemName, int entityID, string key, string value);

        void SetEventsToIgnore(string[] args);

        void SetLoggingEvents(bool value);

        void SetSystemBool(string systemName, string key, bool value);

        void SetSystemEnabled(string systemName, bool value);

        void SetSystemNumber(string systemName, string key, double value);

        void SetSystemString(string systemName, string key, string value);

        void Uninitialize();

        void Unregister(string modName);

        void Update();
    }
}