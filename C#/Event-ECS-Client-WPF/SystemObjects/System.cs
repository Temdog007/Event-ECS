using Event_ECS_Client_Common;
using Event_ECS_Client_WPF.Misc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public HashSet<string> RegisteredComponents
        {
            get => m_registeredComponents;
            set
            {
                m_registeredComponents = value;
                OnPropertyChanged("RegisteredComponents");
            }
        }
        private HashSet<string> m_registeredComponents = new HashSet<string>();

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
                        else if (reader.ValueEquals("entities"))
                        {
                            reader.Read();
                            ReadEntitesFromJson(reader);
                        }
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

        private void ReadEntitesFromJson(JsonTextReader reader)
        {
            List<Entity> temp = new List<Entity>();
            Entity current = null;
            bool done = false;
            while (!done && reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        current = new Entity();
                        break;
                    case JsonToken.EndObject:
                        temp.Add(current);
                        break;
                    case JsonToken.PropertyName:
                        if (reader.ValueEquals("id"))
                        {
                            reader.Read(); // property : id
                            current.ID = Convert.ToInt32(reader.Value);
                        }
                        else if (reader.ValueEquals("name"))
                        {
                            reader.Read(); // property : name
                            current.Name = (string)reader.Value;
                        }
                        else if (reader.ValueEquals("events"))
                        {
                            reader.Read(); // property : events
                            reader.ReadArray<string>(item => current.Events.Add(item));
                        }
                        else if(reader.ValueEquals("components"))
                        {
                            reader.Read(); // property : components
                            reader.ReadArray<string>(item => current.Components.Add(item));
                        }
                        break;
                    case JsonToken.EndArray:
                        done = true;
                        break;
                }
            }
            reader.Read();
            Entities = new ObservableCollection<Entity>(temp);
        }
    }
}
