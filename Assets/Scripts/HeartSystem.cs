using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSystem
{
    public event EventHandler OnDamaged;
    public event EventHandler OnDied;
    
    private int maxHearts;
    private int hearts;

    public HeartSystem(int maxHearts)
    {
        this.maxHearts = maxHearts;
        hearts = maxHearts;
    }

    public void Damage()
    {
        if (hearts <= 0)
            return;
            
        hearts--;
        
        if (OnDamaged != null)
            OnDamaged(this, EventArgs.Empty);
        
        if (hearts == 0 && OnDied != null)
            OnDied(this, EventArgs.Empty);
    }

    public int GetMaxHearts()
    {
        return maxHearts;
    }

    public int GetHearts()
    {
        return hearts;
    }
}
