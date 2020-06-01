using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public event EventHandler onDisplayChanged;
    
    [SerializeField] private bool shownOnStart = false;

    private bool shown;
    
    private void Awake()
    {
        SetActive(shownOnStart);
    }

    private void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);

        if (shown != isActive)
        {
            shown = isActive;
            onDisplayChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Hide()
    {
        SetActive(false);
    }

    public void Show()
    {
        SetActive(true);
    }

    public bool IsShown()
    {
        return shown;
    }
}
