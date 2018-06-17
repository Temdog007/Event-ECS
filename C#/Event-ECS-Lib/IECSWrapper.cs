using System;
using System.ServiceModel;

namespace Event_ECS_Lib
{
    [ServiceContract(CallbackContract = typeof(IECSWrapperCallback), SessionMode = SessionMode.Allowed)]
    public interface IECSWrapper : IDisposable
    {
        [OperationContract(IsOneWay = true)]
        void AddComponent(string systemName, int entityID, string componentName);

        [OperationContract(IsOneWay = true)]
        void AddComponents(string systemName, int entityID, string[] componentNames);

        [OperationContract(IsOneWay = true)]
        void AddEntity(string systemName);

        [OperationContract(IsOneWay = true)]
        void BroadcastEvent(string eventName);

        [OperationContract(Name = "DispatchEvent1", IsOneWay = true)]
        void DispatchEvent(string systemName, string eventName);

        [OperationContract(Name = "DispatchEvent2", IsOneWay =true)]
        void DispatchEvent(string systemName, int entityID, string eventName);

        [OperationContract(Name = "Execute1", IsOneWay = true)]
        void Execute(string code);

        [OperationContract(Name = "Execute2", IsOneWay = true)]
        void Execute(string code, string systemName);

        [OperationContract(IsOneWay = true)]
        void GetClassName(string systemName);

        [OperationContract(IsOneWay = true)]
        void GetComponentBool(string systemName, int entityID, int componentID, string key);

        [OperationContract(Name = "GetComponentNumber1")]
        void GetComponentNumber(string systemName, int entityID, int componentID, string key);

        [OperationContract(Name = "GetComponentNumber2")]
        void GetComponentNumber(string systemName, int entityID, int componentID, int key);

        [OperationContract(IsOneWay = true)]
        void GetComponentString(string systemName, int entityID, int componentID, string key);

        [OperationContract(IsOneWay = true)]
        void GetEntityBool(string systemName, int entityID, string key);

        [OperationContract(IsOneWay = true)]
        void GetEntityNumber(string systemName, int entityID, string key);

        [OperationContract(IsOneWay = true)]
        void GetEntityString(string systemName, int entityID, string key);

        [OperationContract(IsOneWay = true)]
        void GetSystemBool(string systemName, string key);

        [OperationContract(IsOneWay = true)]
        void GetSystemNumber(string systemName, string key);

        [OperationContract(IsOneWay = true)]
        void GetSystemString(string systemName, string key);

        [OperationContract(Name = "Initialize1", IsOneWay = true)]
        void Initialize(string initializerCode);

        [OperationContract(Name = "Initialize2", IsOneWay = true)]
        void Initialize(string initializerCode, string executablePath, string identity);

        [OperationContract(IsOneWay = true)]
        void IsComponentEnabled(string systemName, int entityID, int componentID);

        [OperationContract(IsOneWay = true)]
        void IsDisposing();

        [OperationContract(IsOneWay = true)]
        void IsEntityEnabled(string systemName, int entityID);

        [OperationContract(IsOneWay = true)]
        void IsLoggingEvents();

        [OperationContract(IsOneWay = true)]
        void IsStarted();

        [OperationContract(IsOneWay = true)]
        void IsSystemEnabled(string systemName);
        
        [OperationContract(IsOneWay = true)]
        void RemoveComponent(string systemName, int entityID, int componentID);

        [OperationContract(IsOneWay = true)]
        void RemoveEntity(string systemName, int entityID);

        [OperationContract(IsOneWay = true)]
        void Reset();

        [OperationContract(IsOneWay = true)]
        void Serialize();

        [OperationContract(IsOneWay = true)]
        void SerializeComponent(string systemName, int entityID, int componentID);

        [OperationContract(IsOneWay = true)]
        void SerializeEntity(string systemName, int entityID);

        [OperationContract(IsOneWay = true)]
        void SerializeSystem(string systemName);

        [OperationContract(IsOneWay = true)]
        void SetAutoUpdate(bool value);

        [OperationContract(IsOneWay = true)]
        void SetComponentBool(string systemName, int entityID, int componentID, string key, bool value);

        [OperationContract(IsOneWay = true)]
        void SetComponentEnabled(string systemName, int entityID, int componentID, bool value);

        [OperationContract(Name = "SetComponentNumber1", IsOneWay = true)]
        void SetComponentNumber(string systemName, int entityID, int componentID, int key, double value);

        [OperationContract(Name = "SetComponentNumber2", IsOneWay = true)]
        void SetComponentNumber(string systemName, int entityID, int componentID, string key, double value);

        [OperationContract(IsOneWay = true)]
        void SetComponentString(string systemName, int entityID, int componentID, string key, string value);

        [OperationContract(IsOneWay = true)]
        void SetEntityBool(string systemName, int entityID, string key, bool value);

        [OperationContract(IsOneWay = true)]
        void SetEntityEnabled(string systemName, int entityID, bool value);

        [OperationContract(IsOneWay = true)]
        void SetEntityNumber(string systemName, int entityID, string key, double value);

        [OperationContract(IsOneWay = true)]
        void SetEntityString(string systemName, int entityID, string key, string value);

        [OperationContract(IsOneWay = true)]
        void SetEventsToIgnore(string[] args);

        [OperationContract(IsOneWay = true)]
        void SetLoggingEvents(bool value);

        [OperationContract(IsOneWay = true)]
        void SetSystemBool(string systemName, string key, bool value);

        [OperationContract(IsOneWay = true)]
        void SetSystemEnabled(string systemName, bool value);

        [OperationContract(IsOneWay = true)]
        void SetSystemNumber(string systemName, string key, double value);

        [OperationContract(IsOneWay = true)]
        void SetSystemString(string systemName, string key, string value);

        [OperationContract(IsOneWay = true)]
        void Unregister(string modName);

        [OperationContract(IsOneWay = true)]
        void Update();
    }
}