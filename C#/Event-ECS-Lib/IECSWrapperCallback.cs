using System.ServiceModel;

namespace Event_ECS_Lib
{
    public interface IECSWrapperCallback
    {
        [OperationContract(IsOneWay = true)]
        void AddEntity(string value);

        [OperationContract(IsOneWay = true)]
        void BroadcastEvent(string value);

        [OperationContract(IsOneWay = true)]
        void DispatchEvent(int value);
        
        [OperationContract(IsOneWay = true)]
        void GetClassName(string value);

        [OperationContract(IsOneWay = true)]
        void GetComponentBool(bool value);

        [OperationContract(IsOneWay = true)]
        void GetComponentNumber(double value);

        [OperationContract(IsOneWay = true)]
        void GetComponentString(string value);

        [OperationContract(IsOneWay = true)]
        void GetEntityBool(bool value);

        [OperationContract(IsOneWay = true)]
        void GetEntityNumber(double value);

        [OperationContract(IsOneWay = true)]
        void GetEntityString(string value);

        [OperationContract(IsOneWay = true)]
        void GetSystemBool(bool value);

        [OperationContract(IsOneWay = true)]
        void GetSystemNumber(double value);

        [OperationContract(IsOneWay = true)]
        void GetSystemString(string value);

        [OperationContract(IsOneWay = true)]
        void IsComponentEnabled(bool value);

        [OperationContract(IsOneWay = true)]
        void IsEntityEnabled(bool value);

        [OperationContract(IsOneWay = true)]
        void IsLoggingEvents(bool value);

        [OperationContract(IsOneWay = true)]
        void IsStarted(bool value);

        [OperationContract(IsOneWay = true)]
        void IsSystemEnabled(bool value);

        [OperationContract(IsOneWay = true)]
        void IsUpdatingAutomatically(bool value);

        [OperationContract(IsOneWay = true)]
        void LogEvent(string log);

        [OperationContract(IsOneWay = true)]
        void RemoveComponent(bool value);

        [OperationContract(IsOneWay = true)]
        void RemoveEntity(bool value);

        [OperationContract(IsOneWay = true)]
        void Serialize(string[] value);

        [OperationContract(IsOneWay = true)]
        void SerializeComponent(string value);

        [OperationContract(IsOneWay = true)]
        void SerializeEntity(string value);

        [OperationContract(IsOneWay = true)]
        void SerializeSystem(string value);
    }
}
