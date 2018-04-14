using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Event_ECS_WPF.SystemObjects
{
    public class EntityComponentSystem : NotifyPropertyChanged
    {
        private ObservableCollection<Entity> m_entities = new ObservableCollection<Entity>();
        private int m_entityID = 0;

        private string m_name = "Entity Component System";

        private ObservableCollection<string> m_registeredComponents = new ObservableCollection<string>();

        private string m_selectedComponent = string.Empty;

        public ObservableCollection<Entity> Entities
        {
            get => m_entities;
            set
            {
                m_entities = value;
                OnPropertyChanged("Entities");
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
        public ObservableCollection<string> RegisteredComponents
        {
            get => m_registeredComponents;
            set
            {
                m_registeredComponents = value;
                OnPropertyChanged("RegisteredComponents");
            }
        }

        public string SelectedComponent
        {
            get => m_selectedComponent;
            set
            {
                m_selectedComponent = value;
                OnPropertyChanged("SelectedComponent");
            }
        }

        public void Deserialize(List<string> list)
        {
            string system = list[0];
            int index = system.IndexOf(',');
            if (index < 0)
            {
                Name = system.Substring(0, index);
                List<string> newList = new List<string>();
                foreach(var comp in system.Substring(index).Split(','))
                {
                    newList.Add(comp);
                }
                RegisteredComponents = new ObservableCollection<string>(newList);
            }
            else
            {
                Name = system;
                RegisteredComponents.Clear();
            }
        }

        internal void SetUniqueID(Entity entity)
        {
            entity.ID = m_entityID++;
        }
    }
}
