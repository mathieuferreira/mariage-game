using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolProgressBar : MonoBehaviour
{
    private ProgressBar progressBar;

    private void Awake()
    {
        progressBar = GetComponent<ProgressBar>();
        progressBar.OnAnimationComplete += ProgressBarOnOnAnimationComplete;
    }

    private void ProgressBarOnOnAnimationComplete(object sender, EventArgs e)
    {
        if (progressBar.IsFull())
        {
            
        }
    }

    public ProgressBar GetProgressBar()
    {
        return progressBar;
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
