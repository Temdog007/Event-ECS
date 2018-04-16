using System;
using System.Collections.ObjectModel;
using Event_ECS_WPF.Misc;

namespace Event_ECS_WPF.SystemObjects
{
    public class Entity : NotifyPropertyChanged, IComparable<Entity>
    {
        private readonly EntityComponentSystem m_system;

        private ObservableCollection<Component> m_components = new ObservableSet<Component>();

        private ObservableSet<string> m_events = new ObservableSet<string>();

        private int m_id;

        private string m_name = "Entity";

        public Entity(EntityComponentSystem system)
        {
            this.m_system = system ?? throw new ArgumentNullException(nameof(system));
            this.m_system.Entities.Add(this);

            this.m_system.SetUniqueID(this);
        }

        public ObservableCollection<string> AvailableComponents => System.RegisteredComponents;

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

        internal EntityComponentSystem System => m_system;

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

        public int CompareTo(Entity other)
        {
            return ID.CompareTo(other.ID);
        }
    }
}
