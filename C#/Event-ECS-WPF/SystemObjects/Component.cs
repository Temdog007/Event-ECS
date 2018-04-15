using Event_ECS_WPF.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Event_ECS_WPF.SystemObjects
{
    public class Component : Collection<ComponentVariable>, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable
    {
        private ObservableSet<ComponentVariable> m_variables = new ObservableSet<ComponentVariable>();

        private readonly Entity m_entity;

        private readonly string m_name;

        public Component(Entity entity, string name)
        {
            this.m_entity = entity ?? throw new ArgumentNullException(nameof(entity));
            this.m_entity.Components.Add(this);

            this.m_name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public object this[string key]
        {
            get => m_variables.SingleOrDefault(v => v.Name == key)?.Value;
            set
            {
                var val = m_variables.SingleOrDefault(v => v.Name == key);
                if(val != null)
                {
                    val.Value = value;
                }
            }
        }

        public Entity Entity => m_entity;

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                ((INotifyCollectionChanged)Variables).CollectionChanged += value;
            }

            remove
            {
                ((INotifyCollectionChanged)Variables).CollectionChanged -= value;
            }
        }

        public ObservableSet<ComponentVariable> Variables
        {
            get => m_variables;
            set
            {
                m_variables = value;
                OnPropertyChanged("Variables");
            }
        }

        public bool IsReadOnly => ((ICollection<ComponentVariable>)Variables).IsReadOnly;

        public string Name => m_name;

        public static bool operator !=(Component component1, Component component2)
        {
            return !(component1 == component2);
        }

        public static bool operator ==(Component component1, Component component2)
        {
            return EqualityComparer<Component>.Default.Equals(component1, component2);
        }

        public bool Equals(Component other)
        {
            return Variables.Equals(other.Variables);
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
            if (obj is Component)
            {
                return Equals((Component)obj);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return -1853421134 + EqualityComparer<ObservableSet<ComponentVariable>>.Default.GetHashCode(Variables);
        }

        public override string ToString()
        {
            return string.Format("Name: {0}\nComponents:\n{1}", Name, string.Join(Environment.NewLine, Variables.Select(v => v.ToString())));
        }

        public void Dispose()
        {
            m_entity.Components.Remove(this);
        }
    }

    public class ComponentVariable : NotifyPropertyChanged, IEquatable<ComponentVariable>
    {
        private readonly string m_name;
        private readonly Type m_type;
        private object m_value;

        public ComponentVariable(string name, Type type, object value)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.m_name = name;
            if(!value.GetType().IsAssignableFrom(type))
            {
                throw new Exception("Type must match type of value");
            }
            this.m_type = type;
            this.m_value = value;
        }

        public string Name => m_name;

        public Type Type => m_type;

        public object Value
        {
            get => m_value;
            set
            {
                this.m_value = value;
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