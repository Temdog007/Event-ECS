﻿using Event_ECS_WPF.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Event_ECS_WPF.SystemObjects
{
    public class Component : Collection<ComponentVariable>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private ObservableSet<ComponentVariable> m_variables = new ObservableSet<ComponentVariable>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public object this[string key]
        {
            get => m_variables.SingleOrDefault(v => v.Name == key);
            set
            {
                var val = m_variables.SingleOrDefault(v => v.Name == key);
                if(val != null)
                {
                    val.Value = value;
                }
            }
        }

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
            if (this == obj)
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
            return string.Join(Environment.NewLine, Variables.Select(v => v.ToString()));
        }
    }

    public class ComponentVariable : NotifyPropertyChanged, IEquatable<ComponentVariable>
    {
        private string m_name = string.Empty;

        private object m_value;

        public ComponentVariable() : this(string.Empty, null) { }

        public ComponentVariable(string m_name, object m_value)
        {
            this.m_name = m_name ?? throw new ArgumentNullException(nameof(m_name));
            this.m_value = m_value ?? throw new ArgumentNullException(nameof(m_value));
        }

        public string Name
        {
            get => m_name;
            set
            {
                m_name = value;
                OnPropertyChanged("Name");
            }
        }

        public object Value
        {
            get => m_value;
            set
            {
                m_value = value;
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
            return Name.Equals(other.Name) && Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (this == obj)
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