using System;
using System.ComponentModel;

namespace Event_ECS_WPF.SystemObjects
{
    public interface IEntityVariable : IComparable<IEntityVariable>, IEquatable<IEntityVariable>, INotifyPropertyChanged, ICloneable
    {
        Entity Entity { get; }
        string Name { get; }
        Type Type { get; }
        object Value { get; set; }

        IEntityVariable Parent { get; set; }
        void UpdateValue();
    }
}
