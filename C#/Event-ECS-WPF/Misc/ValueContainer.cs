using System;

namespace Event_ECS_WPF.Misc
{
    public class ValueContainer<T> : NotifyPropertyChanged, 
        ICloneable, IComparable,
        IEquatable<T>, IComparable<T>, 
        IEquatable<ValueContainer<T>>, IComparable<ValueContainer<T>>
        where T : class, ICloneable, IEquatable<T>, IComparable<T>
    {
        private T _value;

        public T Value
        {
            get => _value ?? (_value = default(T));
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public static implicit operator T(ValueContainer<T> val)
        {
            return (T)val.Value.Clone();
        }

        public static implicit operator ValueContainer<T>(T t)
        {
            return new ValueContainer<T>()
            {
                Value = t
            };
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int CompareTo(T other)
        {
            return Value.CompareTo(other);
        }

        public int CompareTo(ValueContainer<T> other)
        {
            return CompareTo(other.Value);
        }

        public bool Equals(T other)
        {
            return Value.Equals(other);
        }

        public bool Equals(ValueContainer<T> other)
        {
            return Equals(other.Value);
        }

        public int CompareTo(object obj)
        {
            if(obj is ValueContainer<T> vt)
            {
                return CompareTo(vt);
            }
            if(obj is T t)
            {
                return CompareTo(t);
            }
            throw new ArgumentException("Cannot convert paramter", nameof(obj));
        }
    }
}
