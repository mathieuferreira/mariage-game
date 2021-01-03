using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public event EventHandler OnAnimationComplete;
    
    [SerializeField] private float speed = 1f;
    [SerializeField] private Sprite barSprite = default;
    [SerializeField] private Sprite borderSprite = default;
    [SerializeField] private Vector2 size = new Vector2(320, 32);
    [SerializeField] private Vector2 borderSize = new Vector2(4, 4);
    [SerializeField] private bool visibleWhenEmpty = true;
    [SerializeField] private float fillAmount = 1f;

    private RectTransform bar;
    private bool animated;

    private void Awake()
    {
        GameObject background = new GameObject("Background", typeof(Image));
        background.transform.SetParent(transform);
        background.transform.SetAsFirstSibling();
        RectTransform backgroundRectTransform = background.transform.GetComponent<RectTransform>();
        backgroundRectTransform.sizeDelta = size;
        backgroundRectTransform.localPosition = Vector3.zero;
        backgroundRectTransform.localScale = new Vector3(1f, 1f);
        Image backgroundImage = background.transform.GetComponent<Image>();
        backgroundImage.type = Image.Type.Sliced;
        backgroundImage.sprite = borderSprite;
        
        GameObject barGameObject = new GameObject("Bar", typeof(Image));
        barGameObject.transform.SetParent(transform);
        barGameObject.transform.SetSiblingIndex(1);
        bar = barGameObject.transform.GetComponent<RectTransform>();
        bar.sizeDelta = new Vector2(GetMinBarSize() + (GetMaxBarSize() - GetMinBarSize()) * fillAmount, size.y - borderSize.y * 2);
        bar.pivot = new Vector2(0, .5f);
        bar.localPosition = new Vector3(-size.x * .5f + borderSize.x, 0f, 0f);
        bar.localScale = new Vector3(1f, 1f);
        Image barImage = barGameObject.transform.GetComponent<Image>();
        barImage.type = Image.Type.Sliced;
        barImage.sprite = barSprite;

        animated = false;
    }

    private void FixedUpdate()
    {
        if (!animated)
            return;

        float minBarSize = GetMinBarSize();
        float maxBarSize = GetMaxBarSize();
        float currentFilledAmount = (bar.sizeDelta.x - minBarSize) / (maxBarSize - minBarSize);
        float newFillAmount = currentFilledAmount;
        if (currentFilledAmount < fillAmount)
        {
            newFillAmount += speed * Time.fixedDeltaTime;

            if (newFillAmount > fillAmount)
            {
                newFillAmount = fillAmount;
                animated = false;
            }
        }
        else if(currentFilledAmount > fillAmount)
        {
            newFillAmount -= speed * Time.fixedDeltaTime;

            if (newFillAmount < fillAmount)
            {
                newFillAmount = fillAmount;
                animated = false;
            }
        }
        else
        {
            animated = false;
        }
        
        bar.sizeDelta = new Vector2(minBarSize + (maxBarSize - minBarSize) * newFillAmount, bar.sizeDelta.y);
        
        if (!animated && OnAnimationComplete != null)
            OnAnimationComplete(this, EventArgs.Empty);
    }

    private float GetMaxBarSize()
    {
        return size.x - borderSize.x * 2;
    }

    private float GetMinBarSize()
    {
        return visibleWhenEmpty ? size.y - borderSize.y * 2 : 0f;
    }

    public void SetFillAmount(float amount)
    {
        fillAmount = Mathf.Clamp(amount, 0f, 1f);
        animated = true;
    }

    public float GetFillAmount()
    {
        return fillAmount;
    }

    public bool IsFull()
    {
        return fillAmount >= 1f;
    }
}
