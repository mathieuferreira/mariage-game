﻿using System;
using Adventure;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bar
{
    public class BarGuest : InteractableArea<BarPlayer>
    {
        public event EventHandler<NeedCompleteEventArgs> OnNeedsComplete;
    
        private const float Speed = 1f;
    
        private enum State
        {
            Walking,
            Idle
        }

        private Vector3 walkingDirection;
        private State currentState;
        private bool active;
        private float timer;
        private RPGPlayerAnimator animator;
        private BarConsumableList needs;
        private BubbleSystem bubbleSystem;
        private BoxCollider2D boxCollider;

        private void Awake()
        {
            animator = transform.GetComponent<RPGPlayerAnimator>();
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
            animator.SetSpeedAndDirection(Speed, walkingDirection);
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

            float distance = Speed * Time.fixedDeltaTime;
            var bounds = boxCollider.bounds;
            RaycastHit2D raycastHit2D = Physics2D.BoxCast(bounds.center, bounds.size, 0f, walkingDirection, distance);

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

        protected override bool IsPlayerAccepted(BarPlayer player)
        {
            if (!CanPlayerSatisfyNeeds(player))
                return false;
            
            return base.IsPlayerAccepted(player);
        }

        protected override void DoAreaAction(BarPlayer player)
        {
            for (int i = 0; i < needs.Count(); i++)
            {
                if (needs.Get(i).GetKind() == BarConsumable.Kind.Talk || player.GetConsumableList().TryConsume(needs.Get(i).GetKind()) != null)
                {
                    animator.SetDirection(player.GetPosition() - transform.position);
                    BarConsumable need = needs.TryConsume(needs.Get(i).GetKind());

                    if (need != null)
                    {
                        SoundManager.GetInstance().Play("Give");
                        ScoreManager.IncrementScore(player.GetPlayerId());
                        OnNeedsComplete?.Invoke(this, new NeedCompleteEventArgs()
                        {
                            Consumable = need
                        });
                    }

                    if (!CanPlayerSatisfyNeeds(player))
                        player.HideAdviceButton();

                    StartIdleState();
                    
                    return;
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
                    needs.Get(i).GetKind() == BarConsumable.Kind.Talk ||
                    player.GetConsumableList().ContainsType(needs.Get(i).GetKind()) 
                )
                {
                    return true;
                }
            }

            return false;
        }

        public class NeedCompleteEventArgs : EventArgs
        {
            public BarConsumable Consumable;
        }
    }
}
