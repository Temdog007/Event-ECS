using System;
using System.Collections.ObjectModel;

namespace Event_ECS_WPF.Misc
{
    public class ObservableSet<T> : ObservableCollection<T>
    {
        protected override void InsertItem(int index, T item)
        {
            if (Contains(item))
            {
                throw new Exception("Item is already in set");
            }
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, T item)
        {
            int i = IndexOf(item);
            if(i >= 0 && i != index)
            {
                throw new Exception("Item is already in set");
            }
            base.SetItem(index, item);
        }

        public bool TryAdd(T item)
        {
            try
            {
                this.Add(item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
