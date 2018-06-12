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

        [OperationContract]
        string AddEntity(string systemName);

        [OperationContract(IsOneWay = true)]
        void BroadcastEvent(string eventName);

        [OperationContract(Name = "DispatchEvent1")]
        int DispatchEvent(string systemName, string eventName);

        [OperationContract(Name = "DispatchEvent2")]
        int DispatchEvent(string systemName, int entityID, string eventName);

        [OperationContract(Name = "Execute1", IsOneWay = true)]
        void Execute(string code);

        [OperationContract(Name = "Execute2", IsOneWay = true)]
        void Execute(string code, string systemName);

        [OperationContract]
        bool GetAutoUpdate();

        [OperationContract]
        string GetClassName(string systemName);

        [OperationContract]
        bool GetComponentBool(string systemName, int entityID, int componentID, string key);

        [OperationContract(Name = "GetComponentNumber1")]
        double GetComponentNumber(string systemName, int entityID, int componentID, string key);

        [OperationContract(Name = "GetComponentNumber2")]
        double GetComponentNumber(string systemName, int entityID, int componentID, int key);

        [OperationContract]
        string GetComponentString(string systemName, int entityID, int componentID, string key);

        [OperationContract]
        bool GetEntityBool(string systemName, int entityID, string key);

        [OperationContract]
        double GetEntityNumber(string systemName, int entityID, string key);

        [OperationContract]
        string GetEntityString(string systemName, int entityID, string key);

        [OperationContract]
        bool GetSystemBool(string systemName, string key);

        [OperationContract]
        double GetSystemNumber(string systemName, string key);

        [OperationContract]
        string GetSystemString(string systemName, string key);

        [OperationContract(Name = "Initialize1", IsOneWay = true)]
        void Initialize(string initializerCode);

        [OperationContract(Name = "Initialize2", IsOneWay = true)]
        void Initialize(string initializerCode, string executablePath, string identity);

        [OperationContract]
        bool IsComponentEnabled(string systemName, int entityID, int componentID);

        [OperationContract]
        bool IsDisposing();

        [OperationContract]
        bool IsEntityEnabled(string systemName, int entityID);

        [OperationContract]
        bool IsLoggingEvents();

        [OperationContract]
        bool IsStarted();

        [OperationContract]
        bool IsSystemEnabled(string systemName);
        
        [OperationContract]
        bool RemoveComponent(string systemName, int entityID, int componentID);

        [OperationContract]
        bool RemoveEntity(string systemName, int entityID);

        [OperationContract]
        void Reset();

        [OperationContract]
        string[] Serialize();

        [OperationContract]
        string SerializeComponent(string systemName, int entityID, int componentID);

        [OperationContract]
        string SerializeEntity(string systemName, int entityID);

        [OperationContract]
        string SerializeSystem(string systemName);

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

        [OperationContract]
        int Update();
    }
}