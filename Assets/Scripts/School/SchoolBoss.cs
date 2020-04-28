using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SchoolBoss : SchoolEnemy
{
    protected override int GetMaxHealth()
    {
        return 450;
    }
    
    protected override int GetMaxRandomMove()
    {
        return -1;
    }
}
