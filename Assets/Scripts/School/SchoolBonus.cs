using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolBonus : MonoBehaviour
{
    public event EventHandler OnConsume;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(OnConsume != null)
            OnConsume(this, EventArgs.Empty);
        
        Destroy(gameObject);
    }
}
