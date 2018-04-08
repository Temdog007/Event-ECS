using System;
using System.Collections.ObjectModel;
using Event_ECS_WPF.Misc;

namespace Event_ECS_WPF.SystemObjects
{
    public class Entity : NotifyPropertyChanged, IDisposable
    {
        private readonly EntityComponentSystem m_system;

        private ObservableCollection<Component> m_components = new ObservableSet<Component>();

        private ObservableSet<string> m_events = new ObservableSet<string>();

        private int m_id;

        private string m_name = "Entity";

        public Entity(EntityComponentSystem m_system)
        {
            this.m_system = m_system ?? throw new ArgumentNullException(nameof(m_system));
            this.m_system.Entities.Add(this);

            this.m_system.SetUniqueID(this);
        }

        public void AddComponent()
        {
            var comp = m_system.SelectedComponent;
            if(!string.IsNullOrEmpty(comp))
            {
                new Component(this, comp);
            }
        }

        public ObservableCollection<Component> Components
        {
            get => m_components;
            set
            {
                m_components = value;
                OnPropertyChanged("Components");
            }
        }

        public ObservableSet<string> Events
        {
            get => m_events;
            set
            {
                m_events = value;
                OnPropertyChanged("Events");
            }
        }

        public int ID
        {
            get => m_id;
            set
            {
                m_id = value;
                OnPropertyChanged("ID");
            }
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

        public void Dispose()
        {
            m_system.Entities.Remove(this);
        }
    }
}
