using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Event_ECS_WPF.SystemObjects
{
    public class Component : ObservableConcurrentDictionary<string, object>, IEquatable<Component>
    {
        public bool Equals(Component other)
        {
            return this.SequenceEqual(other);
        }

        public override bool Equals(object obj)
        {
            if(obj == this)
            {
                return true;
            }
            if(obj == null)
            {
                return false;
            }
            if(obj is Component)
            {
                return Equals((Component)obj);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
