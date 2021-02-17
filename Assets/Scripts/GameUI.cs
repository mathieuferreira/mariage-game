using System;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public event EventHandler<DisplayChangedEventArgs> onDisplayChanged;
    
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
            onDisplayChanged?.Invoke(this, new DisplayChangedEventArgs(isActive));
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

    public class DisplayChangedEventArgs : EventArgs
    {
        private readonly bool isActive;
        public DisplayChangedEventArgs(bool active)
        {
            isActive = active;
        }

        public bool IsActive()
        {
            return isActive;
        }
    }
}
