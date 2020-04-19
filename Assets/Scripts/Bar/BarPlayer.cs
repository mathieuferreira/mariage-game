using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarPlayer : BaseRPGPlayer
{
    private BarConsumableList consumableList;
    
    protected override void Awake()
    {
        base.Awake();
        LockMove();
        consumableList = new BarConsumableList();
    }

    public BarConsumableList GetConsumableList()
    {
        return consumableList;
    }
}
