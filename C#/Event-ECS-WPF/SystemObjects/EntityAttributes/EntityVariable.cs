using Event_ECS_WPF.SystemObjects.Communication;
using System;
using System.Collections.Generic;

namespace Event_ECS_WPF.SystemObjects.EntityAttributes
{
    public class EntityVariable<T> : NotifyPropertyChanged, IEntityVariable where T : IEquatable<T>
    {
        private T m_value;

        public EntityVariable(string name, T value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;

            Type t = typeof(T);
            if (t == typeof(LuaTable) || t == typeof(float) || t == typeof(string) || t == typeof(bool))
            {
                this.m_value = value;
            }
            throw new ArgumentException("Invalid variable type", nameof(T));
        }

        public Entity Entity { get; private set; }

        public string Name { get; }

        public Type Type => typeof(T);

        public T Value
        {
            get => m_value;
            set
            {
                if (m_value?.Equals(value) ?? false)
                {
                    return;
                }

                this.m_value = value;
                UpdateValue();
                OnPropertyChanged("Value");
            }
        }

        object IEntityVariable.Value
        {
            get => Value;
            set
            {
                if (value is T t)
                {
                    Value = t;
                }
            }
        }

        public static bool operator !=(EntityVariable<T> variable1, EntityVariable<T> variable2)
        {
            return !(variable1 == variable2);
        }

        public static bool operator ==(EntityVariable<T> variable1, EntityVariable<T> variable2)
        {
            return EqualityComparer<EntityVariable<T>>.Default.Equals(variable1, variable2);
        }

        public int CompareTo(IEntityVariable other)
        {
            if (Equals(other))
            {
                return 0;
            }

            if (int.TryParse(Name, out int rval1) && int.TryParse(other.Name, out int rval2))
            {
                return rval1.CompareTo(rval2);
            }

            return Name.CompareTo(other.Name);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is IEntityVariable)
            {
                return Equals((IEntityVariable)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(IEntityVariable other)
        {
            if (other.Type is T)
            {
                return Name.Equals(other.Name) && Value.Equals((T)other.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -244751520;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value);
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Value);
        }

        private void UpdateValue()
        {
            if(Entity == null)
            {
                return;
            }

            int entityID = Entity.ID;
            int systemID = Entity.System.ID;
            ECS.Instance.SetEntityValue(systemID, entityID, Name, Value);
        }
    }
}
