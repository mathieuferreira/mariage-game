using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements.Experimental;

public class Modal : MonoBehaviour
{
    private static float CLOSE_Y_POSITION = -754f;
    
    public event EventHandler afterClose;
    public event EventHandler beforeClose;
    public event EventHandler afterOpen;
    public event EventHandler beforeOpen;

    [SerializeField] private bool activeByDefault = true;
    [SerializeField] private CubicBezierCurve openEasing = new CubicBezierCurve(0f, .3f, 1.1f, 1f);
    [SerializeField] private CubicBezierCurve closeEasing = new CubicBezierCurve(0f, -.1f, .3f, 1f);
    [SerializeField] private float openDuration = 1f;
    [SerializeField] private float closeDuration = 1f;
    
    private PlayerReadyButton[] playersReady;
    private bool active;

    private RectTransform rectTransform;

    private bool openAnimation;
    private float animationTimer;

    private void Awake()
    {
        playersReady = new PlayerReadyButton[2];
        playersReady[0] = transform.Find("Player1ReadyButton").GetComponent<PlayerReadyButton>();
        playersReady[1] = transform.Find("Player2ReadyButton").GetComponent<PlayerReadyButton>();

        active = activeByDefault;
        gameObject.SetActive(active);
        animationTimer = 0f;
        
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0f, active ? 0f : CLOSE_Y_POSITION);
    }

    private void FixedUpdate()
    {
        HandleAnimation();
        
        if (!active)
            return;

        HandleInputs();
    }

    private void HandleAnimation()
    {
        if (animationTimer <= 0f)
            return;
        
        animationTimer -= Time.fixedDeltaTime;

        if (animationTimer < 0)
        {
            animationTimer = 0f;
            if (openAnimation)
            {
                Opened();
            } 
            else
            {
                Closed();                
            }
        }

        float position;
        if (openAnimation)
        {
            position = openEasing.Ease(openDuration - animationTimer, CLOSE_Y_POSITION, 0f - CLOSE_Y_POSITION, openDuration);
        }
        else
        {
            position = closeEasing.Ease(closeDuration - animationTimer, 0f, CLOSE_Y_POSITION - 0f, closeDuration);
        }
        
        rectTransform.anchoredPosition = new Vector2(0f, position);
    }

    private void HandleInputs()
    {
        for (int i = 0; i < playersReady.Length; i++)
        {
            if (UserInput.isKeyDown(i, UserInput.Key.Action))
            {
                playersReady[i].SetPlayerReady();
                if (areAllPlayersReady())
                {
                    Close();
                }
            }
        }
    }

    private bool areAllPlayersReady()
    {
        for (int i = 0; i < playersReady.Length; i++)
        {
            if (!playersReady[i].isPlayerReady())
                return false;
        }

        return true;
    }

    public void Open()
    {
        if (beforeOpen != null)
        {
            beforeOpen(this, EventArgs.Empty);
        }
        gameObject.SetActive(true);
        openAnimation = true;
        animationTimer = openDuration;
    }

    public void Close()
    {
        active = false;
        if (beforeClose != null)
        {
            beforeClose(this, EventArgs.Empty);
        }
        openAnimation = false;
        animationTimer = closeDuration;
    }

    private void Closed()
    {
        if (afterClose != null)
        {
            afterClose(this, EventArgs.Empty);
        }
        
        gameObject.SetActive(false);
    }

    private void Opened()
    {
        if (afterOpen != null)
        {
            afterOpen(this, EventArgs.Empty);
        }

        active = true;
    }
}
