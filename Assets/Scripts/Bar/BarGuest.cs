using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarGuest : MonoBehaviour
{
    public event EventHandler<NeedCompleteEventArgs> OnNeedsComplete;
    
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
    private BarConsumableList needs;
    private BubbleSystem bubbleSystem;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        animator = transform.Find("Player").GetComponent<AnimatorRPGPlayer>();
        active = false;
        currentState = State.Idle;
        InitializeRandomTimer();
        needs = new BarConsumableList(2, false);
        bubbleSystem = transform.Find("BubbleSystem").GetComponent<BubbleSystem>();
        bubbleSystem.Setup(needs);
        boxCollider = GetComponent<BoxCollider2D>();
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
        currentState = State.Idle;
        InitializeRandomTimer();
        animator.SetSpeed(0f);
    }

    private void HandleWalking()
    {
        timer -= Time.fixedDeltaTime;

        float distance = SPEED * Time.fixedDeltaTime;
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, walkingDirection, distance);

        if (raycastHit2D.collider != null || timer < 0f)
        {
            StartIdleState();
            return;
        }
        
        transform.position += walkingDirection * distance;  
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

    public void Deactivate()
    {
        active = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        StartIdleState();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BarPlayer player = other.GetComponent<BarPlayer>();

        if (player != null && CanPlayerSatisfyNeeds(player))
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

    private void OnTriggerStay2D(Collider2D other)
    {
        BarPlayer player = other.GetComponent<BarPlayer>();

        if (player != null && UserInput.IsActionKeyDown(player.GetPlayerId()))
        {
            for (int i = 0; i < needs.Count(); i++)
            {
                BarConsumable consumable = player.GetConsumableList().TryConsume(needs.Get(i).GetType());

                if (consumable != null || needs.Get(i).GetType() == BarConsumable.Type.Talk)
                {
                    animator.SetDirection(player.GetPosition() - transform.position);
                    needs.TryConsume(needs.Get(i).GetType());

                    if (OnNeedsComplete != null)
                        OnNeedsComplete(this, new NeedCompleteEventArgs()
                        {
                            consomable = consumable
                        });

                    if (!CanPlayerSatisfyNeeds(player))
                        player.HideAdviceButton();
                    
                    break;
                }
            }
        }
    }

    public BarConsumableList GetNeeds()
    {
        return needs;
    }

    private bool CanPlayerSatisfyNeeds(BarPlayer player)
    {
        for (int i = 0; i < needs.Count(); i++)
        {
            if (
                needs.Get(i).GetType() == BarConsumable.Type.Talk ||
                player.GetConsumableList().ContainsType(needs.Get(i).GetType()) 
            )
            {
                return true;
            }
        }

        return false;
    }

    public class NeedCompleteEventArgs : EventArgs
    {
        public BarConsumable consomable;
    }
}
