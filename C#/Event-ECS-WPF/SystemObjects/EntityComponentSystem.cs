﻿using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Misc;
using EventECSWrapper;
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

        private ObservableCollection<string> m_registeredComponents = new ObservableCollection<string>();

        public EntityComponentSystem()
        {
            Instance = this;
        }

        public static EntityComponentSystem Instance { get; private set; }

        public ObservableCollection<Entity> Entities
        {
            get => m_entities;
            set
            {
                m_entities = value;
                OnPropertyChanged("Entities");
            }
        }

        public int FrameRate
        {
            get
            {
                if (ECS.Instance.ProjectStarted && ECS.Instance.UseWrapper(GetFrameRateFunc, out int fps))
                {
                    return fps;
                }
                return 0;
            }
            set
            {
                ECS.Instance.UseWrapper(ecs => ecs.SetSystemNumber("frameRate", value));
                OnPropertyChanged("FrameRate");
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

        public void Deserialize()
        {
            if (ECS.Instance.UseWrapper(DeserializeFunc, out string[] data))
            {
                Deserialize(data);
            }
        }

        public void Deserialize(IList<string> list)
        {
            OnPropertyChanged("FrameRate");

            string systemData = list[0];
            string[] systemDataList = systemData.Split(Delim);
            if (systemDataList.Length > 1)
            {
                Name = systemDataList[0];
                List<string> newList = new List<string>();
                foreach (var comp in systemDataList.SubArray(1))
                {
                    newList.Add(comp);
                }
                RegisteredComponents = new ObservableCollection<string>(newList);
            }
            else
            {
                Name = systemDataList[0];
                RegisteredComponents.Clear();
            }

            List<string> enList = new List<string>(list.SubArray(1));
            List<int> handledIDs = new List<int>();
            Entity entity = null;
            foreach (string en in enList.AsReadOnly())
            {
                string[] enData = en.Split(Delim);
                if (int.TryParse(enData[0], out int entityID)) // Is Entity
                {
                    handledIDs.Add(entityID);

                    entity = Entities.FirstOrDefault(e => e.ID == entityID);
                    if (entity != null)
                    {
                        entity.Name = enData[1];
                        entity.Components.Clear();
                    }
                    else
                    {
                        entity = new Entity(this);
                        entity.Name = enData[1];
                        entity.ID = entityID;
                    }

                    List<string> events = new List<string>();
                    for (int i = 2; i < enData.Length; ++i)
                    {
                        events.Add(enData[i]);
                    }
                    entity.Events = new ObservableSet<string>(events);
                }
                else // Is Component
                {
                    string compName = enData[0];
                    LinkedList<string> data = new LinkedList<string>(enData);
                    data.RemoveFirst(); // Remove component name

                    int id = 0;
                    bool enabled = true;
                    List<IComponentVariable> tempVars = new List<IComponentVariable>();
                    while (data.Count > 0)
                    {
                        string name = data.First.Value;
                        data.RemoveFirst();
                        if (name == "enabled")
                        {
                            data.RemoveFirst(); // Type must be a boolean
                            enabled = (bool)Convert.ChangeType(data.First.Value, typeof(bool));
                        }
                        else if (name == "id")
                        {
                            data.RemoveFirst(); // Type must be a number
                            id = (int)Convert.ChangeType(data.First.Value, typeof(int));
                        }
                        else
                        {
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

                            Type generic = typeof(ComponentVariable<>).MakeGenericType(type);
                            tempVars.Add((IComponentVariable)Activator.CreateInstance(generic, new object[] { name, Convert.ChangeType(data.First.Value, type) }));
                        }
                        data.RemoveFirst();
                    }

                    Component comp = new Component(entity, compName, id, enabled);
                    comp.Variables = new ObservableSet<IComponentVariable>(tempVars);
                }
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
        }

        private string[] DeserializeFunc(ECSWrapper ecs)
        {
            return ecs.Serialize().Split('\n');
        }

        private int GetFrameRateFunc(ECSWrapper ecs)
        {
            return (int)Convert.ChangeType(ecs.GetSystemNumber("frameRate"), typeof(int));
        }
    }
}
