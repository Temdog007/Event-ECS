using System.ServiceModel;

namespace Event_ECS_Lib
{
    public interface IECSWrapperCallback
    {
        [OperationContract(IsOneWay = true)]
        void LogEvent(string log);
    }
}
