using System.Collections.Generic;
using System.ServiceModel;

namespace Event_ECS_Service
{   
    [ServiceContract(CallbackContract = typeof(IECSWrapperCallback))]
    public interface IECSWrapper
    {
        [OperationContract]
        bool InitializeLove(string executablePath, string identity);

        [OperationContract]
        string AddEntity();

        [OperationContract(IsOneWay = true)]
        bool RemoveEntity(int entityID);

        [OperationContract(IsOneWay = true)]
        int DispatchEvent(string eventName);

        [OperationContract(IsOneWay = true)]
        void RegisterComponent(string moduleName, bool replace);

        [OperationContract(IsOneWay = true)]
        void AddComponent(int entityID, string componentName);

        [OperationContract(IsOneWay = true)]
        void AddComponents(int entityID, IEnumerable<string> componentNames);

        [OperationContract]
        bool RemoveComponent(int entityID, int componentID);

        [OperationContract(IsOneWay = true)]
        void SetSystemBool(string key, bool value);

        [OperationContract(IsOneWay = true)]
        void SetSystemNumber(string key, double value);

        [OperationContract(IsOneWay = true)]
        void SetSystemString(string key, string value);

        [OperationContract(IsOneWay = true)]
        void SetEntityBool(int entityID, string key, bool value);

        [OperationContract(IsOneWay = true)]
        void SetEntityNumber(int entityID, string key, double value);

        [OperationContract(IsOneWay = true)]
        void SetEntityString(int entityID, string key, string value);

        [OperationContract(IsOneWay = true)]
        void SetEnabled(int entityID, int componentID, bool value);

        [OperationContract(IsOneWay = true)]
        void SetComponentBool(int entityID, int componentID, string key, bool value);

        [OperationContract(IsOneWay = true)]
        void SetComponentNumber(int entityID, int componentID, string key, double value);

        [OperationContract(IsOneWay = true)]
        void SetComponentString(int entityID, int componentID, string key, string value);

        [OperationContract]
        bool IsEnabled(int entityID, int componentID);

        [OperationContract]
        bool GetSystemBool(string key);

        [OperationContract]
        double GetSystemNumber(string key);

        [OperationContract]
        string GetSystemString(string key);

        [OperationContract]
        bool GetEntityBool(int entityID, string key);

        [OperationContract]
        double GetEntityNumber(int entityID, string key);

        [OperationContract]
        string GetEntityString(int entityID, string key);

        [OperationContract]
        bool GetComponentBool(int entityID, int componentID, string key);

        [OperationContract]
        double GetComponentNumber(int entityID, int componentID, string key);

        [OperationContract]
        string GetComponentString(int entityID, int componentID, string key);

        [OperationContract]
        string Serialize();

        [OperationContract]
        string SerializeEntity(int entityID);

        [OperationContract]
        string SerializeComponent(int entityID, int componentID);

        [OperationContract(IsOneWay = true)]
        void SetAutoUpdate(bool value);

        [OperationContract]
        bool GetAutoUpdate();

        [OperationContract]
        bool IsDisposing();

        [OperationContract]
        bool LoveUpdate();
    }

    public interface IECSWrapperCallback
    {
        [OperationContract(IsOneWay = true)]
        void LogEvent(string log);
    }
}
