using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSystemUI : MonoBehaviour
{
    private const float heartWidth = 43f;
    private const float heartHeight = 36f;
    private const float heartGap = 15f;

    [SerializeField] private Transform pfHeartUI;

    private HeartSystem heartSystem;
    private HeartUI[] heartUIs;

    public void Setup(HeartSystem system)
    {
        heartSystem = system;
        heartSystem.OnDamaged += OnDamaged;
        heartUIs = new HeartUI[heartSystem.GetMaxHearts()];
        
        float totalSize = GetTotalSize();
        for (int i = 0; i < heartSystem.GetMaxHearts(); i++)
        {
            float position = -totalSize * .5f + i * (heartWidth + heartGap) + heartWidth * .5f;
            Transform heartUI = Instantiate(pfHeartUI, transform);
            heartUI.localPosition = new Vector3(position, heartHeight * .5f, 0f);
            heartUIs[i] = heartUI.GetComponent<HeartUI>();
        }
    }

    private float GetTotalSize()
    {
        return heartWidth * heartSystem.GetMaxHearts() + heartGap * (heartSystem.GetMaxHearts() - 1);
    }

    private void OnDamaged(object sender, EventArgs e)
    {
        for (int i = 0; i < heartSystem.GetMaxHearts(); i++)
        {
            if (i > heartSystem.GetHearts() - 1 && heartUIs[i].IsVisible())
            {
                heartUIs[i].Hide();
            }
        }
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
