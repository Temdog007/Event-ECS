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
            
        }
    }
}
