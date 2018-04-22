using EventECSWrapper;
using System;
using System.Collections.Generic;

namespace Event_ECS_WPF.SystemObjects
{
    public class ComponentVariable : NotifyPropertyChanged, IEquatable<ComponentVariable>
    {
        private readonly string m_name;
        private readonly Type m_type;
        private object m_value;
        
        public ComponentVariable(string name, Type type, object value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.m_name = name;
            if (!value.GetType().IsAssignableFrom(type))
            {
                throw new Exception("Type must match type of value");
            }
            this.m_type = type;
            this.m_value = value;
        }

        public string Name => m_name;

        public Type Type => m_type;

        public Component Component
        {
            get; internal set;
        }

        private void UpdateValue(ECSWrapper ecs)
        {
            int entityID = Component.Entity.ID;
            int compID = (int)Convert.ChangeType(Component["id"], typeof(int));
            if (Type == typeof(float))
            {
                ecs.SetComponentNumber(entityID, compID, Name, (float)Convert.ChangeType(Value, Type));
            }
            else if (Type == typeof(string))
            {
                ecs.SetComponentString(entityID, compID, Name, (string)Convert.ChangeType(Value, typeof(string)));
            }
            else if (Type == typeof(bool))
            {
                ecs.SetComponentBool(entityID, compID, Name, (bool)Convert.ChangeType(Value, typeof(bool)));
            }
            else
            {
                throw new Exception(string.Format("Unknown type: {0}", Type.FullName));
            }
        }

        public object Value
        {
            get => m_value;
            set
            {
                this.m_value = value;
                if(Component != null)
                {
                    ECS.Instance.UseWrapper(UpdateValue);
                }
                OnPropertyChanged("Value");
            }
        }

        public static bool operator !=(ComponentVariable variable1, ComponentVariable variable2)
        {
            return !(variable1 == variable2);
        }

        public static bool operator ==(ComponentVariable variable1, ComponentVariable variable2)
        {
            return EqualityComparer<ComponentVariable>.Default.Equals(variable1, variable2);
        }

        public bool Equals(ComponentVariable other)
        {
            return Name.Equals(other.Name) && Type.Equals(other.Type) && Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
            if (this == obj)
#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast
            {
                return true;
            }
            if (obj is ComponentVariable)
            {
                return Equals((ComponentVariable)obj);
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
