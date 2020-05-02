using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunIndicator : MonoBehaviour
{
    public event EventHandler OnFunExpired;
    
    private const int UNIT_BY_GUEST = 100;
    private const float SPEED = 3f;
    private const int UNIT_TALK_SATISFIED = 10;
    private const int UNIT_CAKE_SATISFIED = 50;
    private const int UNIT_BEER_SATISFIED = 40;
    
    private BarGuest[] guests;
    private float currentIndex;
    private bool active;
    private Transform indicator;
    private float size;

    private void Awake()
    {
        currentIndex = 0;
        SetActive(false);
        indicator = transform.Find("Indicator");
        size = transform.Find("Bar").GetComponent<RectTransform>().sizeDelta.x;
    }

    private void FixedUpdate()
    {
        if (!active)
            return;

        float max = GetMaxIndex();
        currentIndex = Mathf.Clamp(currentIndex - SPEED * GetGuestNeedsCount() * Time.fixedDeltaTime, 0f, max);
        
        indicator.localPosition = new Vector3((currentIndex / max - .5f) * size, 0f, 0f);

        if (currentIndex <= 0f && OnFunExpired != null)
            OnFunExpired(this, EventArgs.Empty);
    }

    public void Setup(BarGuest[] barGuests)
    {
        guests = barGuests;

        for (int i = 0; i < guests.Length; i++)
        {
            guests[i].OnNeedsComplete += OnNeedsComplete;
        }
        
        currentIndex = GetMaxIndex();
    }

    private void OnNeedsComplete(object sender, BarGuest.NeedCompleteEventArgs eventArgs)
    {
        int delta;

        switch (eventArgs.consomable.GetType())
        {
            case BarConsumable.Type.Beer:
                delta = UNIT_BEER_SATISFIED;
                break;
            case BarConsumable.Type.Cake:
                delta = UNIT_CAKE_SATISFIED;
                break;
            default:
                delta = UNIT_TALK_SATISFIED;
                break;
        }
        
        currentIndex = Mathf.Clamp(currentIndex + delta, 0f, GetMaxIndex());
    }

    private int GetMaxIndex()
    {
        return UNIT_BY_GUEST * guests.Length;
    }

    private int GetGuestNeedsCount()
    {
        int needs = 0;

        for (int i = 0; i < guests.Length; i++)
        {
            needs += guests[i].GetNeeds().Count();
        }
        
        return needs;
    }

    public void SetActive(bool act)
    {
        gameObject.SetActive(act);
        active = act;
    }
}
