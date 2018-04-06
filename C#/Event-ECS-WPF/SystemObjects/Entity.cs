using Event_ECS_Client_Common;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace Event_ECS_Client_WPF.SystemObjects
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

        public ObservableCollection<string> Events
        {
            get => m_events;
            set
            {
                m_events = value;
                OnPropertyChanged("Events");
            }
        }
        private ObservableCollection<string> m_events = new ObservableCollection<string>();

        public ObservableConcurrentDictionary<string, Component> Components
        {
            get => m_components;
            set
            {
                m_components = value;
                OnPropertyChanged("Components");
            }
        }
        private ObservableConcurrentDictionary<string, Component> m_components = new ObservableConcurrentDictionary<string, Component>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
