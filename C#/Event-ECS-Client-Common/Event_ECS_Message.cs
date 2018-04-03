using System;
using System.Text;

namespace Event_ECS_Client_Common
{
    public enum Event_ECS_Message
    {
        [Event_ECS_Message("getdata")]
        GET_DATA,

        [Event_ECS_Message("addentity")]
        ADD_ENTITY,

        [Event_ECS_Message("removeentity")]
        REMOVE_ENTITY,

        [Event_ECS_Message("close")]
        CLOSE
    }

    public enum Event_ECS_MessageResponse
    {
        SYSTEM_DATA,
        ENTITY_DATA,
        REMOVED_ENTITIES,
        DISPATCHED_EVENT,
        CLOSING
    }

    public class Event_ECS_MessageAttribute : Attribute
    {
        public Event_ECS_MessageAttribute(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }

        public byte[] Data => Encoding.ASCII.GetBytes(Message + "\n");
    }
}
