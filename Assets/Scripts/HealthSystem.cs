using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    public event EventHandler<HealthChangeEvent> OnHealthChange;
    public event EventHandler OnDied;
    
    private int health;
    private int maxHealth;

    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public void Damage(int damageAmount)
    {
        ChangeHealth(health - damageAmount);
    }

    public void Heal(int healAmount)
    {
        ChangeHealth(health + healAmount);
    }

    private void ChangeHealth(int newHealth)
    {
        newHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        int oldHealth = health;
        
        if (newHealth == oldHealth)
            return;

        health = newHealth;
        
        if (OnHealthChange != null)
            OnHealthChange(this, new HealthChangeEvent(oldHealth, newHealth));
        
        if (!IsAlive() && OnDied != null)
            OnDied(this, EventArgs.Empty);
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public float GetHealthNormalized()
    {
        return (float) health / maxHealth;
    }
}

public class HealthChangeEvent : EventArgs
{
    private int oldHealth;
    private int newHealth;

    public HealthChangeEvent(int oldHealth, int newHealth)
    {
        this.oldHealth = oldHealth;
        this.newHealth = newHealth;
    }

    public int GetOldHealth()
    {
        return oldHealth;
    }

    public int GetNewHealth()
    {
        return newHealth;
    }
}