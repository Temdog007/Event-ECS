using Event_ECS_WPF.Extensions;
using Event_ECS_WPF.Logger;
using Event_ECS_WPF.SystemObjects.Communication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Event_ECS_WPF.SystemObjects
{
    public class EntityComponentSystem : ECS_Object
    {        
        private const string defaultName = "Entity Component System";

        private ObservableCollection<Entity> m_entities = new ObservableCollection<Entity>();

        public EntityComponentSystem()
        {
            Name = defaultName;
        }

        public EntityComponentSystem(string systemName, bool isSystemEnabled, int id, IList<string> list)
        {
            Name = systemName;
            IsEnabled = isSystemEnabled;
            ID = id;
            Deserialize(list);
        }

        public IEnumerable<ECS_Object> AllObjects
        {
            get
            {
                foreach (var entity in Entities.AsReadOnly())
                {
                    yield return entity;
                    foreach (var component in entity.Components.AsReadOnly())
                    {
                        yield return component;
                    }
                }
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

        public void Deserialize(IList<string> lines)
        {
            try
            {
                ECS.Instance.ShouldUpdateServer = false;

                Entity entity = null;

                bool done = false;
                HashSet<ECS_Object> handledObjs = new HashSet<ECS_Object>();

                while (!done && lines.Count > 0)
                {
                    foreach (string line in lines.AsReadOnly())
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            lines.RemoveAt(0);
                            continue;
                        }

                        string type = string.Empty;
                        switch ((type = GetData(line, out ECSData nameData, out ECSData enabledData, out ECSData idData, out List<ECSData> ecsDataList)))
                        {
                            case "entity":
                                entity = Entities.FirstOrDefault(en => en.ID == Convert.ToInt32(idData.Value)) ?? new Entity(this);
                                entity.Deserialize(nameData, enabledData, idData, ecsDataList);
                                handledObjs.Add(entity);
                                lines.RemoveAt(0);
                                break;

                            case "component":
                                if (entity == null)
                                {
                                    lines.RemoveAt(0);
                                    break;
                                }

                                var component = entity.Components.FirstOrDefault(ecs => ecs.ID == Convert.ToInt32(idData.Value)) ?? new Component(entity);
                                component.Deserialize(nameData, enabledData, idData, ecsDataList);
                                handledObjs.Add(component);
                                lines.RemoveAt(0);
                                break;

                            case "system":
                                done = true;
                                break;

                            default:
                                LogManager.Instance.Add("Unknown ECS type: {0}", type);
                                lines.RemoveAt(0);
                                break;
                        }
                    }
                }

                foreach (var unusedObj in from obj in AllObjects where !handledObjs.Contains(obj) select obj)
                {
                    unusedObj.Remove();
                }
            }
            finally
            {
                ECS.Instance.ShouldUpdateServer = true;
            }
        }

        public override bool Remove()
        {
            throw new NotImplementedException("Not a valid function for a system");
        }

        protected override void ValueChanged(string key, object value)
        {
            if(key == "enabled")
            {
                ECS.Instance.SetSystemEnabled(ID, Convert.ToBoolean(value));
            }
        }
    }
}
