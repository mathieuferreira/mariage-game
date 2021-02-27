using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProgressBar))]
public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;
    private ProgressBar progressBar;

    private void Awake()
    {
        progressBar = GetComponent<ProgressBar>();
        Debug.Log(progressBar);
    }

    public void Setup(HealthSystem system)
    {
        healthSystem = system;
        healthSystem.OnHealthChange += HealthSystemOnOnHealthChange;
    }

    private void HealthSystemOnOnHealthChange(object sender, HealthChangeEvent e)
    {
        progressBar.SetFillAmount(healthSystem.GetHealthNormalized());
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
