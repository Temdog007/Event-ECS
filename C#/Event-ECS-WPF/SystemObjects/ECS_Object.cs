using Event_ECS_WPF.SystemObjects.EntityAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Event_ECS_WPF.SystemObjects
{
    public abstract class ECS_Object : NotifyPropertyChanged, IComparable<ECS_Object>, IEquatable<ECS_Object>
    {
        public const char Delim = '|';

        public const char TableDelim = ',';

        private int _iD;

        private bool _isEnabled;

        private string _name = string.Empty;

        public int ID
        {
            get => _iD; set
            {
                if(ID == value)
                {
                    return;
                }

                _iD = value;
                OnPropertyChanged("ID");
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if(IsEnabled == value)
                {
                    return;
                }

                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
                ValueChanged("enabled", _isEnabled);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if(Name == value)
                {
                    return;
                }

                _name = value;
                OnPropertyChanged("Name");
                ValueChanged("name", _name);
            }
        }

        public static string GetData(string line, out ECSData nameData, out ECSData enabledData, out ECSData idData, out List<ECSData> ecsDataList)
        {
            string[] enData = line.Split(Delim);
            ecsDataList = new List<ECSData>();

            for (int i = 1; i < enData.Length; i += 3)
            {
                ecsDataList.Add(new ECSData { Name = enData[i], Type = enData[i + 1], Value = enData[i + 2] });
            }

            // name, enabled, and id must be defined. If not, we want to throw an error
            nameData = ecsDataList.First(d => d.Name == "name");
            enabledData = ecsDataList.First(d => d.Name == "enabled");
            idData = ecsDataList.First(d => d.Name == "id");

            return enData[0];
        }

        public static Type GetType(string str)
        {
            switch (str)
            {
                case "number":
                    return typeof(float);
                case "string":
                    return typeof(string);
                case "boolean":
                    return typeof(bool);
                case "table":
                    return typeof(LuaTable);
                default:
                    throw new ArgumentException(string.Format("Unknown type: {0}", str));
            }
        }

        public static LuaTable ParseTable(Entity entity, string data)
        {
            LuaTable dict = new LuaTable();
            StringBuilder buildStr = new StringBuilder(data);
            buildStr.Remove(data.Length-1, 1);
            buildStr.Remove(0, 1);
            data = buildStr.ToString();

            while (true)
            {
                int start = data.IndexOf("{");
                int end = data.LastIndexOf("}");
                if(start == -1 || end == -1) { break; }

                var tabStr = new StringBuilder(data.Substring(start, end - start+1));

                string str = tabStr.ToString();
                data = data.Replace(str, str.Replace(TableDelim, '|').Replace("{", string.Empty).Replace("}", string.Empty));
            }

            string[] list = data.Split(TableDelim);
            if (list.Length >= 3)
            {
                for (int i = 0; i < list.Length; i += 3)
                {
                    string name = list[i];
                    Type type = GetType(list[i + 1]);
                    object value;
                    if (type == typeof(LuaTable))
                    {
                        value = ParseTable(entity, "{"+list[i + 2].Replace('|', TableDelim)+"}");
                    }
                    else
                    {
                        value = Convert.ChangeType(list[i + 2], type);
                    }
                    if (!dict.ContainsKey(name))
                    {
                        Type generic = typeof(EntityVariable<>).MakeGenericType(type);
                        dict[name] = (IEntityVariable)Activator.CreateInstance(generic, new object[] { entity, name, value });
                    }
                    else
                    {
                        IEntityVariable enVar = dict[name];
                        enVar.Value = value;
                    }
                }
            }
            return dict;
        }

        public int CompareTo(ECS_Object other)
        {
            return ID.CompareTo(other.ID);
        }

        public void Deserialize(ECSData nameData, ECSData enabledData, ECSData idData, IList<ECSData> ecsDataList)
        {
            Name = nameData.Value;
            IsEnabled = Convert.ToBoolean(enabledData.Value);
            ID = Convert.ToInt32(idData.Value);

            ecsDataList.Remove(nameData);
            ecsDataList.Remove(enabledData);
            ecsDataList.Remove(idData);

            Deserialize(ecsDataList);
        }

        public virtual void Deserialize(IEnumerable<ECSData> ecsData) { }

        public bool Equals(ECS_Object other)
        {
            return ID.Equals(other.ID) && Name.Equals(other.Name) && IsEnabled.Equals(other.IsEnabled);
        }

        public abstract bool Remove();

        public override string ToString()
        {
            return string.Format("Name: {0}, ID: {1}, Enabled: {2}", Name, ID, IsEnabled);
        }

        protected abstract void ValueChanged(string key, object value);
    }
}
