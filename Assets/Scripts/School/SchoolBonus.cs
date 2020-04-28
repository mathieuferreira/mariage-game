using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolBonus : MonoBehaviour
{
    public event EventHandler<BonusConsumedeventArgs> OnConsume;

    private void OnTriggerEnter2D(Collider2D other)
    {
        SchoolShuriken shuriken = other.GetComponent<SchoolShuriken>();

        if (shuriken != null)
        {
            Destroy(gameObject);
                    
            if(OnConsume != null)
                OnConsume(this, new BonusConsumedeventArgs { shuriken = shuriken});
        }
    }

    public class BonusConsumedeventArgs : EventArgs
    {
        public SchoolShuriken shuriken;
    }
}
