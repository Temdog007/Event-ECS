using Event_ECS_WPF.SystemObjects.Communication;
using System;

namespace Event_ECS_WPF.SystemObjects
{
    public class Component : ECS_Object
    {
        public Component(Entity entity)
        {
            Entity = entity;
            Entity.Components.Add(this);
        }

        public Entity Entity { get; }

        public override bool Remove()
        {
            return Entity.Components.Remove(this);
        }

        protected override void ValueChanged(string key, object value)
        {
            if (key == "enabled")
            {
                ECS.Instance.SetComponentEnabled(Entity.System.ID, Entity.ID, ID, Convert.ToBoolean(value));
            }
        }
    }
}
