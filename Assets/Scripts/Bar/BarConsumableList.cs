using System;
using System.Collections.Generic;

namespace Bar
{
    public class BarConsumableList
    {
        public event EventHandler Change;
    
        private readonly List<BarConsumable> list;
        private readonly bool allowMultipleType;
        private readonly int limit;

        public BarConsumableList(int limit)
        {
            list = new List<BarConsumable>();
            allowMultipleType = true;
            this.limit = limit;
        }

        public BarConsumableList(int limit, bool allowMultipleType)
        {
            list = new List<BarConsumable>();
            this.allowMultipleType = allowMultipleType;
            this.limit = limit;
        
        }

        public bool TryAdd(BarConsumable consumable)
        {
            if (IsFull())
                return false;
        
            if (!allowMultipleType && ContainsType(consumable.GetKind()))
                return false;
        
            list.Add(consumable);

            Change?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public bool IsFull()
        {
            return list.Count >= limit;
        }
    
        public bool ContainsType(BarConsumable.Kind kind)
        {
            foreach (BarConsumable consumable in list)
            {
                if (consumable.GetKind() == kind)
                {
                    return true;
                }
            }

            return false;
        }

        public int CountType(BarConsumable.Kind kind)
        {
            int count = 0;
        
            foreach (BarConsumable consumable in list)
            {
                if (consumable.GetKind() == kind)
                {
                    count++;
                }
            }

            return count;
        }
    
        public BarConsumable TryConsume(BarConsumable.Kind kind)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].GetKind() == kind)
                {
                    BarConsumable consumable = list[i];
                    list.RemoveAt(i);

                    Change?.Invoke(this, EventArgs.Empty);

                    return consumable;
                }
            }

            return null;
        }

        public int Count()
        {
            return list.Count;
        }

        public BarConsumable Get(int i)
        {
            if (i < list.Count)
                return list[i];

            return null;
        }
    }
}
