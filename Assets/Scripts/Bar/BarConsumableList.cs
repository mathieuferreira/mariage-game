using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarConsumableList
{
    public event EventHandler change;
    
    private const int MAX_CONSUMMABLE_COUNT = 3;
    
    private List<BarConsumable> list;

    public BarConsumableList()
    {
        list = new List<BarConsumable>();
    }

    public void Add(BarConsumable consumable)
    {
        list.Add(consumable);
        
        if(change != null)
            change(this, EventArgs.Empty);
    }

    public bool IsFull()
    {
        return list.Count >= MAX_CONSUMMABLE_COUNT;
    }
    
    public bool ContainsType(BarConsumable.Type type)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetType() == type)
            {
                return true;
            }
        }

        return false;
    }

    public int CountType(BarConsumable.Type type)
    {
        int count = 0;
        
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetType() == type)
            {
                count++;
            }
        }

        return count;
    }
    
    public BarConsumable TryConsume(BarConsumable.Type type)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetType() == type)
            {
                BarConsumable consumable = list[i];
                list.RemoveAt(i);
                return consumable;
            }
        }

        return null;
    }
}
