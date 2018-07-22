using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Event_ECS_WPF.SystemObjects.EntityAttributes
{
    public class LuaTable : Dictionary<string, IEntityVariable>, IEquatable<LuaTable>
    {
        public LuaTable()
        {
        }

        public LuaTable(int capacity) : base(capacity)
        {
        }

        public LuaTable(IEqualityComparer<string> comparer) : base(comparer)
        {
        }

        public LuaTable(IDictionary<string, IEntityVariable> dictionary) : base(dictionary)
        {
        }

        public LuaTable(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
        {
        }

        public LuaTable(IDictionary<string, IEntityVariable> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
        {
        }

        protected LuaTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public bool Equals(LuaTable other)
        {
            if(Count != other.Count)
            {
                return false;
            }

            foreach(var pair in other)
            {
                if(pair.Value != this[pair.Key])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
