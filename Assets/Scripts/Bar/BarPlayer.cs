using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarPlayer : BaseRPGPlayer
{
    private const int MAX_CONSUMABLE = 3;
    
    private BarConsumableList consumableList;
    
    protected override void Awake()
    {
        base.Awake();
        LockMove();
        consumableList = new BarConsumableList(MAX_CONSUMABLE);
    }

    public BarConsumableList GetConsumableList()
    {
        return consumableList;
    }
}
