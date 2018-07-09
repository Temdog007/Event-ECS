using Event_ECS_WPF.Logger;
using Event_ECS_WPF.Misc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Event_ECS_WPF.SystemObjects
{
    public class EntityComponentSystem : NotifyPropertyChanged
    {
        public const char Delim = '|';

        private ObservableCollection<Entity> m_entities = new ObservableCollection<Entity>();

        private string m_name = "Entity Component System";

        public EntityComponentSystem()
        {
        }

        public EntityComponentSystem(string systemName, bool isSystemEnabled, IList<string> list)
        {
            Deserialize(systemName, isSystemEnabled, list);
        }

        private bool m_isEnabled = false;
        public bool IsEnabled
        {
            get => m_isEnabled;
            set
            {
                m_isEnabled = value;
                ECS.Instance.SetSystemValue(Name, "enabled", value);
                OnPropertyChanged("IsEnabled");
            }
        }

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

        public void Deserialize(string systemName, bool isSystemEnabled, IList<string> list)
        {
            ECS.Instance.ShouldUpdateServer = false;

            Name = systemName;
            IsEnabled = isSystemEnabled;
            
            List<int> handledIDs = new List<int>();
            Entity entity = null;
            List<string> compNames = new List<string>();

            bool done = false;
            while(!done && list.Count > 0)
            {
                string en = list.First();
                list.RemoveAt(0);
                if (string.IsNullOrWhiteSpace(en))
                {
                    continue;
                }
                string[] enData = en.Split(Delim);
                switch (enData[0])
                {
                    case "Entity":
                        int entityID = int.Parse(enData[1]);

                        handledIDs.Add(entityID);

                        if (entity != null && compNames.Any())
                        {
                            var deadComps = entity.Components.Where(comp => !compNames.Contains(comp.Name));
                            foreach (var deadComp in deadComps.ToList().AsReadOnly())
                            {
                                entity.Components.Remove(deadComp);
                            }
                        }
                        compNames.Clear();

                        entity = Entities.FirstOrDefault(e => e.ID == entityID);
                        if (entity != null)
                        {
                            entity.IsEnabled = Convert.ToBoolean(enData[2]);
                            entity.Name = enData[3];
                        }
                        else
                        {
                            entity = new Entity(this, entityID)
                            {
                                IsEnabled = Convert.ToBoolean(enData[2]),
                                Name = enData[3]
                            };
                        }

                        List<string> events = new List<string>();
                        for (int i = 4; i < enData.Length; ++i)
                        {
                            events.Add(enData[i]);
                        }
                        entity.Events = new ObservableSet<string>(events);
                        break;

                    case "Component":
                        if(entity == null)
                        {
                            break;
                        }

                        LinkedList<string> data = new LinkedList<string>(enData);
                        data.RemoveFirst(); // Remove 'Component'

                        string compName = data.First.Value;
                        compNames.Add(compName);
                        data.RemoveFirst(); // Remove component name

                        List<IComponentVariable> tempVars = new List<IComponentVariable>();
                        while (data.Count > 0 && data.Count % 3 == 0)
                        {
                            string name = data.First.Value;
                            data.RemoveFirst();

                            Type type;
                            switch (data.First.Value)
                            {
                                case "number":
                                    type = typeof(float);
                                    break;
                                case "string":
                                    type = typeof(string);
                                    break;
                                case "boolean":
                                    type = typeof(bool);
                                    break;
                                default:
                                    throw new ArgumentException(string.Format("Couldn't deserialize entity with type {0}", data.First.Value));
                            }
                            data.RemoveFirst();

                            if (!tempVars.Any(v => v.Name == name))
                            {
                                Type generic = typeof(ComponentVariable<>).MakeGenericType(type);
                                tempVars.Add((IComponentVariable)Activator.CreateInstance(generic, new object[] { name, Convert.ChangeType(data.First.Value, type) }));
                            }
                            data.RemoveFirst();
                        }

                        var oldComp = entity.Components.FirstOrDefault(comp => comp.Name == compName);
                        if (oldComp == null)
                        {
                            Component comp = new Component(entity, compName)
                            {
                                Variables = new ObservableSet<IComponentVariable>(tempVars)
                            };
                        }
                        else
                        {
                            foreach (var tempVar in tempVars)
                            {
                                oldComp[tempVar.Name] = tempVar.Value;
                            }
                        }
                        break;

                    case "System":
                        done = true;
                        break;

                    default:
                        LogManager.Instance.Add("Unknown ECS type: {0}", enData[0]);
                        break;
                }
            }

            if (entity != null && compNames.Any())
            {
                var deadComps = entity.Components.Where(comp => !compNames.Contains(comp.Name));
                foreach(var deadComp in deadComps.ToList().AsReadOnly())
                {
                    entity.Components.Remove(deadComp);
                }
                compNames.Clear();
            }

            Entity[] temp = new Entity[Entities.Count];
            Entities.CopyTo(temp, 0);
            foreach (var en in temp)
            {
                if (!handledIDs.Contains(en.ID))
                {
                    Entities.Remove(en);
                }
            }

            ECS.Instance.ShouldUpdateServer = true;
        }
    }
}
