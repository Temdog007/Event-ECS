using Event_ECS_WPF.Misc;

namespace Event_ECS_WPF.SystemObjects
{
    public class Entity : NotifyPropertyChanged
    {
        public int ID
        {
            get => m_id;
            set
            {
                m_id = value;
                OnPropertyChanged("ID");
            }
        }
        private int m_id;

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

        public ObservableSet<string> Events
        {
            get => m_events;
            set
            {
                m_events = value;
                OnPropertyChanged("Events");
            }
        }
        private ObservableSet<string> m_events = new ObservableSet<string>();

        public ObservableSet<Component> Components
        {
            get => m_components;
            set
            {
                m_components = value;
                OnPropertyChanged("Components");
            }
        }
        private ObservableSet<Component> m_components = new ObservableSet<Component>();
    }
}
