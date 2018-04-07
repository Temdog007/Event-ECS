using System.Collections.ObjectModel;

namespace Event_ECS_WPF.SystemObjects
{
    public class System : NotifyPropertyChanged
    {
        public string Name
        {
            get => m_name;
            set
            {
                m_name = value;
                OnPropertyChanged("Name");
            }
        }
        private string m_name = string.Empty;

        public ObservableCollection<string> RegisteredComponents
        {
            get => m_registeredComponents;
            set
            {
                m_registeredComponents = value;
                OnPropertyChanged("RegisteredComponents");
            }
        }
        private ObservableCollection<string> m_registeredComponents = new ObservableCollection<string>();

        public ObservableCollection<Entity> Entities
        {
            get => m_entities;
            set
            {
                m_entities = value;
                OnPropertyChanged("Entities");
            }
        }
        private ObservableCollection<Entity> m_entities = new ObservableCollection<Entity>();
    }
}
