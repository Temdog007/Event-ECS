﻿using Event_ECS_WPF.Misc;
using EventECSWrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Event_ECS_WPF.SystemObjects
{
    public class Component : Collection<IComponentVariable>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private readonly Entity m_entity;
        private readonly int m_id;
        private readonly string m_name;
        private bool m_isEnabled;
        private ObservableSet<IComponentVariable> m_variables = new ObservableSet<IComponentVariable>();
        public Component(Entity m_entity, string m_name, int m_id, bool m_isEnabled = true)
        {
            this.m_entity = m_entity ?? throw new ArgumentNullException(nameof(m_entity));
            this.m_entity.Components.Add(this);

            this.m_name = m_name ?? throw new ArgumentNullException(nameof(m_name));
            this.m_id = m_id;
            this.m_isEnabled = m_isEnabled;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public Entity Entity => m_entity;

        public int ID => m_id;

        private void SetEnabled(ECSWrapper ecs)
        {
            ecs.SetEnabled(Entity.ID, ID, IsEnabled);
        }

        public bool IsEnabled
        {
            get => m_isEnabled;
            set
            {
                m_isEnabled = value;
                ECS.Instance.UseWrapper(SetEnabled);
                OnPropertyChanged("IsEnabled");
            }
        }

        public bool IsReadOnly => ((ICollection<IComponentVariable>)Variables).IsReadOnly;

        public string Name => m_name;

        public ObservableSet<IComponentVariable> Variables
        {
            get => m_variables;
            set
            {
                m_variables = value;
                foreach (var v in m_variables.Cast< IComponentVariableSetter>())
                {
                    v.Component = this;
                }
                OnPropertyChanged("Variables");
            }
        }

        public object this[string key]
        {
            get => m_variables.SingleOrDefault(v => v.Name == key)?.Value;
            set
            {
                var val = m_variables.SingleOrDefault(v => v.Name == key);
                if (val != null)
                {
                    val.Value = value;
                }
            }
        }

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
            return Variables.SequenceEqual(other.Variables);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is Component)
            {
                return Equals((Component)obj);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return -1853421134 + EqualityComparer<ObservableSet<IComponentVariable>>.Default.GetHashCode(Variables);
        }

        public override string ToString()
        {
            return string.Format("Name: {0}\nComponents:\n{1}", Name, string.Join(Environment.NewLine, Variables.Select(v => v.ToString())));
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}