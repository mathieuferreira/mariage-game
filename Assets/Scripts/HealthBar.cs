using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;
    private Transform lifeBar;
    
    private void Awake()
    {
        lifeBar = transform.Find("Wrapper");
    }

    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        this.healthSystem.OnHealthChange += HealthSystemOnOnHealthChange;
    }

    private void HealthSystemOnOnHealthChange(object sender, HealthChangeEvent e)
    {
        lifeBar.localScale = new Vector3(((HealthSystem)sender).GetPercentHealth(), 1, 1);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
