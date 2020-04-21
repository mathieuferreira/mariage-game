using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarGuest : MonoBehaviour
{
    private const float SPEED = 1f;
    
    private enum State
    {
        Walking,
        Idle
    }

    private Vector3 walkingDirection;
    private State currentState;
    private bool active;
    private float timer;
    private AnimatorRPGPlayer animator;

    private void Awake()
    {
        animator = transform.Find("Player").GetComponent<AnimatorRPGPlayer>();
        active = false;
        currentState = State.Idle;
        InitializeRandomTimer();
    }

    private void FixedUpdate()
    {
        if (!active)
            return;

        switch (currentState)
        {
            case State.Walking:
                HandleWalking();
                break;
            default:
                HandleIdle();
                break;
        }
    }
    
    private void StartWalkingState()
    {
        //CMDebug.TextPopup("Start Walking", transform.position);
        walkingDirection = new [] {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right
        }[Random.Range(0, 4)];
        currentState = State.Walking;
        InitializeRandomTimer();
        animator.SetSpeedAndDirection(SPEED, walkingDirection);
    }
    
    private void StartIdleState()
    {
        //CMDebug.TextPopup("Start Idle", transform.position);
        currentState = State.Idle;
        InitializeRandomTimer();
        animator.SetSpeed(0f);
    }

    private void HandleWalking()
    {
        timer -= Time.fixedDeltaTime;
        transform.position += walkingDirection * SPEED * Time.fixedDeltaTime;

        if (timer < 0f)
            StartIdleState();
    }
    
    private void HandleIdle()
    {
        timer -= Time.fixedDeltaTime;

        if (timer < 0f)
            StartWalkingState();
    }

    private void InitializeRandomTimer()
    {
        timer = Random.Range(1f, currentState == State.Idle ? 8f : 3f);
    }

    public void Activate()
    {
        active = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        StartIdleState();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BarPlayer player = other.GetComponent<BarPlayer>();

        if (player != null)
        {
            player.ShowAdviceButton();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        BarPlayer player = other.GetComponent<BarPlayer>();

        if (player != null)
        {
            player.HideAdviceButton();
        }
    }
}
