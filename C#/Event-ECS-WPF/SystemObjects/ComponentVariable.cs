using System;
using System.Collections.Generic;

namespace Event_ECS_WPF.SystemObjects
{
    internal interface IComponentVariableSetter
    {
        Component Component { set; }
    }

    public interface IComponentVariable
    {
        string Name { get; }
        Type Type { get; }
        object Value { get; set; }
        Component Component { get; }
    }

    public class ComponentVariable<T> : NotifyPropertyChanged, IEquatable<ComponentVariable<T>>, IComponentVariable, IComponentVariableSetter where T : IEquatable<T>
    {
        private T m_value;

        public ComponentVariable(string name, T value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            Type t = typeof(T);
            if (t != typeof(float) && t != typeof(string) && t != typeof(bool))
            {
                throw new ArgumentException("Invalid variable type", nameof(T));
            }
            this.Name = name;
            this.m_value = value;
        }

        public string Name { get; }

        public Type Type => typeof(T);

        Component IComponentVariableSetter.Component { set => Component = value; }

        public Component Component { get; private set; }

        private void UpdateValue()
        {
            if(Component == null)
            {
                return;
            }

            int entityID = Component.Entity.ID;
            string systemName = Component.Entity.System.Name;
            int compID = (int)Convert.ChangeType(Component.ID, typeof(int));
            ECS.Instance.SetComponentValue(systemName, entityID, compID, Name, Value);
        }

        public T Value
        {
            get => m_value;
            set
            {
                if(m_value?.Equals(value) ?? false)
                {
                    return;
                }

                this.m_value = value;
                UpdateValue();
                OnPropertyChanged("Value");
            }
        }

        object IComponentVariable.Value
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

        public static bool operator !=(ComponentVariable<T> variable1, ComponentVariable<T> variable2)
        {
            return !(variable1 == variable2);
        }

        public static bool operator ==(ComponentVariable<T> variable1, ComponentVariable<T> variable2)
        {
            return EqualityComparer<ComponentVariable<T>>.Default.Equals(variable1, variable2);
        }

        public bool Equals(ComponentVariable<T> other)
        {
            return Name.Equals(other.Name) && Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is ComponentVariable<T>)
            {
                return Equals((ComponentVariable<T>)obj);
            }
            return base.Equals(obj);
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
    }
}
