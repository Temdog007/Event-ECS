using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Event_ECS_WPF.SystemObjects.EntityAttributes
{
    public class LuaTable : IDictionary<string, IEntityVariable>, IEquatable<LuaTable>
    {
        private readonly Dictionary<string, IEntityVariable> m_dictionary;

        public LuaTable()
        {
            m_dictionary = new Dictionary<string, IEntityVariable>();
        }

        public LuaTable(int capacity)
        {
            m_dictionary = new Dictionary<string, IEntityVariable>(capacity);
        }

        public LuaTable(IEqualityComparer<string> comparer)
        {
            m_dictionary = new Dictionary<string, IEntityVariable>(comparer);
        }

        public LuaTable(IDictionary<string, IEntityVariable> dictionary)
        {
            m_dictionary = new Dictionary<string, IEntityVariable>(dictionary);
        }

        public LuaTable(int capacity, IEqualityComparer<string> comparer)
        {
            m_dictionary = new Dictionary<string, IEntityVariable>(capacity, comparer);
        }

        public LuaTable(IDictionary<string, IEntityVariable> dictionary, IEqualityComparer<string> comparer)
        {
            m_dictionary = new Dictionary<string, IEntityVariable>(dictionary, comparer);
        }

        public EntityVariable<LuaTable> Parent
        {
            set
            {
                foreach(var v in m_dictionary)
                {
                    v.Value.Parent = value;
                }
            }
        }

        public int Count => m_dictionary.Count;

        public bool IsReadOnly => ((IDictionary<string, IEntityVariable>)m_dictionary).IsReadOnly;

        public ICollection<string> Keys => ((IDictionary<string, IEntityVariable>)m_dictionary).Keys;

        public ICollection<IEntityVariable> Values => ((IDictionary<string, IEntityVariable>)m_dictionary).Values;

        public IEntityVariable this[string key]
        {
            get => m_dictionary[key];
            set
            {
                m_dictionary[key] = value;
                value.PropertyChanged += Value_PropertyChanged;
            }
        }

        public void Add(string key, IEntityVariable value)
        {
            m_dictionary.Add(key, value);
            value.PropertyChanged += Value_PropertyChanged;
        }

        public void Add(KeyValuePair<string, IEntityVariable> item)
        {
            ((IDictionary<string, IEntityVariable>)m_dictionary).Add(item);
        }

        public void Clear()
        {
            m_dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, IEntityVariable> item)
        {
            return ((IDictionary<string, IEntityVariable>)m_dictionary).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return m_dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, IEntityVariable>[] array, int arrayIndex)
        {
            ((IDictionary<string, IEntityVariable>)m_dictionary).CopyTo(array, arrayIndex);
        }

        public bool Equals(LuaTable other)
        {
            if (Count != other.Count)
            {
                return false;
            }

            foreach (var pair in other)
            {
                if (pair.Value != this[pair.Key])
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerator<KeyValuePair<string, IEntityVariable>> GetEnumerator()
        {
            return ((IDictionary<string, IEntityVariable>)m_dictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, IEntityVariable>)m_dictionary).GetEnumerator();
        }

        public bool Remove(string key)
        {
            return m_dictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<string, IEntityVariable> item)
        {
            return ((IDictionary<string, IEntityVariable>)m_dictionary).Remove(item);
        }

        public override string ToString()
        {
            return string.Format("{{{0}}}", string.Join(",",
                this.Select(pair => 
                    int.TryParse(pair.Key, out int result) ?
                    string.Format("[{0}] = {1}", pair.Key, pair.Value.Value) :
                    string.Format("{0} = {1}", pair.Key, pair.Value.Value))));
        }

        public bool TryGetValue(string key, out IEntityVariable value)
        {
            return m_dictionary.TryGetValue(key, out value);
        }

        private void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IEntityVariable val)
            {
                val.Parent?.UpdateValue();
            }
        }
    }
}
