using UnityEngine;
using Random = UnityEngine.Random;
using EditorExtension;

namespace Adventure
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class MovableGuest : MonoBehaviour
    {
        private const float Speed = 1f;
        
        [SerializeField] private RPGPlayerAnimator animator;
        public Vector2 topRightCorner;
        public Vector2 lowerLeftCorner;
    
        private enum State
        {
            Walking,
            Idle
        }

        private Vector3 walkingDirection;
        private State currentState;
        private float timer;
        //private BoxCollider2D boxCollider;
        private Vector3[] bounds;
        private bool locked;

        private void Awake()
        {
            currentState = State.Idle;
            InitializeRandomTimer();
            //boxCollider = GetComponent<BoxCollider2D>();
            Vector2 currentPosition = transform.position;
            bounds = new []
            {
                new Vector3(currentPosition.x, currentPosition.x),
                new Vector3(currentPosition.y, currentPosition.y)
            };
            
            bounds[0].x = Mathf.Min(bounds[0].x, topRightCorner.x);
            bounds[0].y = Mathf.Max(bounds[0].y, topRightCorner.x);
            bounds[1].x = Mathf.Min(bounds[1].x, topRightCorner.y);
            bounds[1].y = Mathf.Max(bounds[1].y, topRightCorner.y);
            
            bounds[0].x = Mathf.Min(bounds[0].x, lowerLeftCorner.x);
            bounds[0].y = Mathf.Max(bounds[0].y, lowerLeftCorner.x);
            bounds[1].x = Mathf.Min(bounds[1].x, lowerLeftCorner.y);
            bounds[1].y = Mathf.Max(bounds[1].y, lowerLeftCorner.y);
        }
        
        private void InitializeRandomTimer()
        {
            timer = Random.Range(1f, currentState == State.Idle ? 8f : 3f);
        }
        
        private void FixedUpdate()
        {
            if (locked)
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

            if (timer < 0f)
            {
                StartIdleState();
                return;
            }
            
            Vector3 candidatePosition = transform.position + walkingDirection * distance;
            
            if (
                candidatePosition.x < bounds[0].x
                || candidatePosition.x > bounds[0].y
                || candidatePosition.y < bounds[1].x
                || candidatePosition.y > bounds[1].y
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

        public void SetDirection(Vector3 direction)
        {
            walkingDirection = direction;
            animator.SetSpeedAndDirection(Speed, walkingDirection);
        }

        public void LockMove()
        {
            locked = true;
            StartIdleState();
        }

        public void UnlockMove()
        {
            locked = false;
        }
    }
}