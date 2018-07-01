namespace Event_ECS_WPF.SystemObjects
{
    public delegate void DataReceived(string data);

    public interface IECS
    {
        event DataReceived DataReceived;

        void AddComponent(string systemName, int entityID, string componentName);

        void AddEntity(string systemName);

        void BroadcastEvent(string eventName);

        void DispatchEvent(string systemName, string eventName);

        void DispatchEvent(string systemName, int entityID, string eventName);

        void Execute(string code);

        void RemoveComponent(string systemName, int entityID, int componentID);

        void RemoveEntity(string systemName, int entityID);

        void ReloadModule(string modName);

        void Reset();

        void SetComponentValue(string systemName, int entityID, int componentID, string key, object value);

        void SetEntityValue(string systemName, int entityID, string key, object value);

        void SetSystemValue(string systemName, string key, object value);
    }
}