using Event_ECS_Client_Common;
using Event_ECS_Client_WPF.Misc;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Event_ECS_Client_WPF.SystemObjects
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

        public void ReadJson(JsonTextReader reader)
        {
            bool done = false;
            while (!done && reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        if (reader.ValueEquals("name"))
                        {
                            reader.Read();
                            Name = (string)reader.Value;
                        }
                        //else if (reader.ValueEquals("entities"))
                        //{
                        //    reader.Read();
                        //    ReadEntitesFromJson(reader);
                        //}
                        else if (reader.ValueEquals("registeredComponents"))
                        {
                            reader.Read();
                            reader.ReadArray<string>(item => RegisteredComponents.Add(item));
                        }
                        break;
                    case JsonToken.EndObject:
                        done = true;
                        break;
                }
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
