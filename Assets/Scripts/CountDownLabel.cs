using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownLabel : MonoBehaviour
{
    public event EventHandler OnAnimationEnd;

    private Text text;

    public void Awake()
    {
        text = GetComponent<Text>();
    }

    public void Init(string label)
    {
        text.text = label;
        gameObject.SetActive(false);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void PlaySound()
    {
        SoundManager.GetInstance().Play("Countdown");
    }

    public void AnimationEnded()
    {
        if (OnAnimationEnd != null)
            OnAnimationEnd(this, EventArgs.Empty);
        
        Destroy(gameObject);
    }
}
