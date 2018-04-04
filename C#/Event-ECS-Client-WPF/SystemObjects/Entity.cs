using Event_ECS_Client_Common;
using System.Collections.Generic;

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
        } private int m_id;

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

        public HashSet<string> Events
        {
            get => m_events;
            set
            {
                m_events = value;
                OnPropertyChanged("Events");
            }
        }
        private HashSet<string> m_events = new HashSet<string>();

        public HashSet<dynamic> Components
        {
            get => m_components;
            set
            {
                m_components = value;
                OnPropertyChanged("Components");
            }
        }
        private HashSet<dynamic> m_components = new HashSet<dynamic>();
    }
}
