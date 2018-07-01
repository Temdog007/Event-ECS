using System.Collections.Generic;

namespace Event_ECS_WPF.SystemObjects
{
    public delegate void DataReceived(IEnumerable<string> data);

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

        void SetComponentBool(string systemName, int entityID, int componentID, string key, bool value);

        void SetComponentNumber(string systemName, int entityID, int componentID, string key, double value);

        void SetComponentString(string systemName, int entityID, int componentID, string key, string value);

        void SetEntityBool(string systemName, int entityID, string key, bool value);

        void SetEntityEnabled(string systemName, int entityID, bool value);

        void SetEntityNumber(string systemName, int entityID, string key, double value);

        void SetEntityString(string systemName, int entityID, string key, string value);

        void SetSystemBool(string systemName, string key, bool value);

        void SetSystemEnabled(string systemName, bool value);

        void SetSystemNumber(string systemName, string key, double value);

        void SetSystemString(string systemName, string key, string value);
    }
}