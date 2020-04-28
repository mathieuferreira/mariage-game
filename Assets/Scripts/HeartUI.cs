using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    private const float animationTimerMax = .6f;
    
    private RectTransform heartFull;

    private bool visible;
    private float timer;
    private Vector2 initialSize;
    private CubicBezierCurve curve;

    private void Awake()
    {
        heartFull = transform.Find("HeartFull").GetComponent<RectTransform>();
        initialSize = heartFull.sizeDelta;
        visible = true;
        curve = new CubicBezierCurve(0f, .3f, 1.1f, 1f);
    }

    private void FixedUpdate()
    {
        if (timer > 0f)
        {
            timer -= Time.fixedDeltaTime;

            if (timer < 0f)
                timer = 0f;
            
            float factor = curve.Ease(animationTimerMax - timer, 1f, -1f, animationTimerMax);
            heartFull.sizeDelta = initialSize * factor;
        }
    }

    public void Hide()
    {
        if (!visible)
            return;
        
        visible = false;
        timer = animationTimerMax;
    }

    public bool IsVisible()
    {
        return visible;
    }
}
