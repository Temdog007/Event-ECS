using System;
using System.Collections.ObjectModel;
using Event_ECS_WPF.Misc;


namespace Event_ECS_WPF.SystemObjects
{
    public class Entity : NotifyPropertyChanged, IComparable<Entity>
    {
        private ObservableCollection<Component> m_components = new ObservableCollection<Component>();

        private ObservableSet<string> m_events = new ObservableSet<string>();

        private string m_name = "Entity";

        public Entity(EntityComponentSystem system, int id)
        {
            this.System = system ?? throw new ArgumentNullException(nameof(system));
            this.System.Entities.Add(this);
            ID = id;
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

        private bool m_isEnabled = false;
        public bool IsEnabled
        {
            get => m_isEnabled;
            set
            {
                m_isEnabled = value;
                ECS.Instance.SetEntityValue(System.Name, ID, "enabled", value);
                OnPropertyChanged("IsEnabled");
            }
        }

        internal EntityComponentSystem System { get; }

        public int ID { get; }

        public string Name
        {
            get => m_name;
            set
            {
                m_name = value;
                ECS.Instance.SetEntityValue(System.Name, ID, "enabled", value);
                OnPropertyChanged("Name");
            }
        }

        public int CompareTo(Entity other)
        {
            return ID.CompareTo(other.ID);
        }
    }
}
