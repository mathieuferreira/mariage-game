using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Adventure
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class MovableGuest : MonoBehaviour
    {
        private const float Speed = 1f;
        
        [SerializeField] private RPGPlayerAnimator animator;
        [SerializeField] private GameObject[] walkingBounds;
    
        private enum State
        {
            Walking,
            Idle
        }

        private Vector3 walkingDirection;
        private State currentState;
        private float timer;
        private BoxCollider2D boxCollider;
        private Vector3[] bounds;

        private void Awake()
        {
            currentState = State.Idle;
            InitializeRandomTimer();
            boxCollider = GetComponent<BoxCollider2D>();
            bounds = new Vector3[2]
            {
                new Vector3(999999999, -999999999),
                new Vector3(999999999, -999999999)
            };

            for (int i = 0; i < walkingBounds.Length; i++)
            {
                Vector3 boundPosition = walkingBounds[i].transform.position;
                bounds[0].x = Mathf.Min(bounds[0].x, boundPosition.x);
                bounds[0].y = Mathf.Max(bounds[0].y, boundPosition.x);
                bounds[1].x = Mathf.Min(bounds[1].x, boundPosition.y);
                bounds[1].y = Mathf.Max(bounds[1].y, boundPosition.y);
            }
        }
        
        private void InitializeRandomTimer()
        {
            timer = Random.Range(1f, currentState == State.Idle ? 8f : 3f);
        }
        
        private void FixedUpdate()
        {
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

            if (timer < 0f)
            {
                StartIdleState();
                return;
            }
            
            Vector3 candidatePosition = transform.position + walkingDirection * distance;
            
            if (
                walkingBounds.Length > 0 && 
                (
                    candidatePosition.x < bounds[0].x
                    || candidatePosition.x > bounds[0].y
                    || candidatePosition.y < bounds[1].x
                    || candidatePosition.y > bounds[1].y
                )
            )
            {
                candidatePosition.x = Mathf.Max(bounds[0].x, Mathf.Min(bounds[0].y, candidatePosition.x));
                candidatePosition.y = Mathf.Max(bounds[1].x, Mathf.Min(bounds[1].y, candidatePosition.y));
                transform.position = candidatePosition;
                StartIdleState();
                return;
            }
            /*var bounds = boxCollider.bounds;
            RaycastHit2D raycastHit2D = Physics2D.BoxCast(bounds.center, bounds.size, 0f, walkingDirection, distance);

            if (raycastHit2D.collider != null || timer < 0f)
            {
                StartIdleState();
                return;
            }*/
        
            transform.position = candidatePosition;
        }
    
        private void HandleIdle()
        {
            timer -= Time.fixedDeltaTime;

            if (timer < 0f)
                StartWalkingState();
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            StartIdleState();
        }
    }
}