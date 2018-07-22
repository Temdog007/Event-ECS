using System;

namespace Event_ECS_WPF.SystemObjects
{
    public interface IEntityVariable : IComparable<IEntityVariable>, IEquatable<IEntityVariable>
    {
        Entity Entity { get; }
        string Name { get; }
        Type Type { get; }
        object Value { get; set; }
    }
}
