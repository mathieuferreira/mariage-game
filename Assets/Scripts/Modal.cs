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

    [SerializeField] private State defaultState;
    [SerializeField] private CubicBezierCurve openEasing = new CubicBezierCurve(0f, .3f, 1.1f, 1f);
    [SerializeField] private CubicBezierCurve closeEasing = new CubicBezierCurve(0f, -.1f, .3f, 1f);
    [SerializeField] private float openDuration = 1f;
    [SerializeField] private float closeDuration = 1f;
    
    public enum State
    {
        Open,
        Closing,
        Closed,
        Opening
    }
    
    private PlayerReadyButton[] playersReady;
    private State currentState;
    private RectTransform rectTransform;
    private float animationTimer;

    private void Awake()
    {
        playersReady = new PlayerReadyButton[2];
        playersReady[0] = transform.Find("Player1ReadyButton").GetComponent<PlayerReadyButton>();
        playersReady[1] = transform.Find("Player2ReadyButton").GetComponent<PlayerReadyButton>();

        for (int i = 0; i < playersReady.Length; i++)
        {
            playersReady[i].OnPlayerReady += OnOnPlayerReady;
        }

        currentState = defaultState;
        animationTimer = 0f;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0f, defaultState == State.Open ? 0f : CLOSE_Y_POSITION);
    }

    private void OnOnPlayerReady(object sender, EventArgs e)
    {
        for (int i = 0; i < playersReady.Length; i++)
        {
            if (!playersReady[i].IsPlayerReady())
                return;
        }

        Close();
    }

    private void FixedUpdate()
    {
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        if (animationTimer <= 0f || (currentState != State.Closing && currentState != State.Opening))
            return;
        
        animationTimer -= Time.fixedDeltaTime;

        if (animationTimer < 0)
        {
            animationTimer = 0f;
            if (currentState == State.Opening)
            {
                Opened();
            } 
            else
            {
                Closed();                
            }
            return;
        }

        float position;
        if (currentState == State.Opening)
        {
            position = openEasing.Ease(openDuration - animationTimer, CLOSE_Y_POSITION, 0f - CLOSE_Y_POSITION, openDuration);
        }
        else
        {
            position = closeEasing.Ease(closeDuration - animationTimer, 0f, CLOSE_Y_POSITION - 0f, closeDuration);
        }
        
        rectTransform.anchoredPosition = new Vector2(0f, position);
    }

    public void Open()
    {
        beforeOpen?.Invoke(this, EventArgs.Empty);
        gameObject.SetActive(true);
        currentState = State.Opening;
        animationTimer = openDuration;
    }

    public void Close()
    {
        beforeClose?.Invoke(this, EventArgs.Empty);
        currentState = State.Closing;
        animationTimer = closeDuration;
    }

    private void Closed()
    {
        afterClose?.Invoke(this, EventArgs.Empty);
        currentState = State.Closed;
        gameObject.SetActive(false);
    }

    private void Opened()
    {
        afterOpen?.Invoke(this, EventArgs.Empty);
        currentState = State.Open;
    }
}
