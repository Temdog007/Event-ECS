namespace Event_ECS_WPF.SystemObjects.Communication
{
    public delegate void DataReceived(string data);

    public interface IECS
    {
        event DataReceived DataReceived;

        void AddComponent(int systemID, int entityID, string componentName);

        void AddEntity(int systemID);

        void BroadcastEvent(string eventName);

        void DispatchEvent(int systemID, string eventName);

        void DispatchEvent(int systemID, int entityID, string eventName);

        void Execute(string code);

        void ReloadModule(string modName);

        void RemoveComponent(int systemID, int entityID, int componentID);

        void RemoveEntity(int systemID, int entityID);

        void Reset();

        void SetComponentEnabled(int systemID, int entityID, int componentID, bool value);

        void SetEntityValue(int systemID, int entityID, string key, object value);

        void SetSystemEnabled(int systemID, bool value);
    }
}