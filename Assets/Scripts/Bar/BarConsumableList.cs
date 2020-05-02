using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarConsumableList
{
    public event EventHandler change;
    
    private List<BarConsumable> list;
    private bool allowMultipleType;
    private int limit;

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
        
        if (!allowMultipleType && ContainsType(consumable.GetType()))
            return false;
        
        list.Add(consumable);
        
        if(change != null)
            change(this, EventArgs.Empty);

        return true;
    }

    public bool IsFull()
    {
        return list.Count >= limit;
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
                
                if (change != null)
                    change(this, EventArgs.Empty);
                
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
