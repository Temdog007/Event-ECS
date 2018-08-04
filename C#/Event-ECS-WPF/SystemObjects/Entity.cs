using Event_ECS_WPF.Misc;
using Event_ECS_WPF.SystemObjects.Communication;
using Event_ECS_WPF.SystemObjects.EntityAttributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Event_ECS_WPF.SystemObjects
{
    public class Entity : ECS_Object
    {
        private const string defaultName = "Entity";

        private static Entity s_empty;

        private ObservableSet<Component> m_components = new ObservableSet<Component>();

        private ObservableSet<IEntityVariable> m_variables = new ObservableSet<IEntityVariable>();

        public Entity(EntityComponentSystem system)
        {
            this.System = system ?? throw new ArgumentNullException(nameof(system));
            this.System.Entities.Add(this);
            Name = defaultName;
        }

        public static Entity Empty => s_empty ?? (s_empty = new Entity(EntityComponentSystem.Empty));

        public ObservableSet<Component> Components
        {
            get => m_components;
            set
            {
                m_components = value;
                OnPropertyChanged("Components");
            }
        }

        public ObservableSet<IEntityVariable> Variables
        {
            get => m_variables;
            set
            {
                m_variables = value;
                OnPropertyChanged("Variables");
            }
        }

        internal EntityComponentSystem System { get; }

        public override void Deserialize(IEnumerable<ECSData> ecsDataList)
        {
            foreach (var ecsData in ecsDataList)
            {
                var variable = Variables.FirstOrDefault(v => v.Name == ecsData.Name);
                if (variable != null && ecsData.Type == "table" && variable.Value is LuaTable table)
                {
                    var newTable = ParseTable(this, ecsData.Value);
                    if (newTable.Count == table.Count)
                    {
                        foreach (var tableVar in newTable)
                        {
                            var key = tableVar.Key;
                            if (table.ContainsKey(key))
                            {
                                IEntityVariable t;
                                t = table[key];
                                t.Value = Convert.ChangeType(tableVar.Value.Value, tableVar.Value.Type);
                                t.Parent = variable;
                            }
                            else
                            {
                                table.Add(key, tableVar.Value);
                                tableVar.Value.Parent = table[key];
                            }
                        }
                    }
                    else
                    {
                        variable.Value = newTable;
                    }
                }
                else
                {
                    if(variable != null)
                    {
                        Variables.Remove(variable);
                    }

                    if (ecsData.Type == "table")
                    {
                        var t = new LuaTable(ParseTable(this, ecsData.Value));
                        var v = new EntityVariable<LuaTable>(this, ecsData.Name, t);
                        t.Parent = v;
                        Variables.Add(v);
                    }
                    else
                    {
                        Type type = GetType(ecsData.Type);
                        Type generic = typeof(EntityVariable<>).MakeGenericType(type);
                        Variables.Add((IEntityVariable)Activator.CreateInstance(generic, new object[] { this, ecsData.Name, Convert.ChangeType(ecsData.Value, type) }));
                    }
                }
            }
        }

        public bool HasObject(ECS_Object obj)
        {
            if(this == obj)
            {
                return true;
            }

            return Components.Contains(obj);
        }

        public bool HasObject(IEnumerable<ECS_Object> objs)
        {
            return objs.Any(obj => HasObject(obj));
        }

        public override bool Remove()
        {
            return System.Entities.Remove(this);
        }

        protected override void ValueChanged(string key, object value)
        {
            if (ID > 0)
            {
                ECS.Instance.SetEntityValue(System.ID, ID, key, value);
            }
        }
    }
}
