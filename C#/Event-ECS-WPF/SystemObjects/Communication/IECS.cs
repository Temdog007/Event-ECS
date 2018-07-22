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

        void RemoveComponent(int systemID, int entityID, int componentID);

        void RemoveEntity(int systemID, int entityID);

        void ReloadModule(string modName);

        void Reset();

        void SetEntityValue(int systemID, int entityID, string key, object value);
    }
}