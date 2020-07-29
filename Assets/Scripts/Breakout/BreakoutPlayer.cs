using System;
using UnityEngine;

namespace Breakout
{
    public class BreakoutPlayer : MonoBehaviour
    {
        private const float Speed = 15f;
        
        [SerializeField] private PlayerID playerId;
        
        private Rigidbody2D rb;
        private bool canMove;
        private Vector3 moveDirection;
        private UserInput.Direction lastDirection;
        private float acceleration;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            canMove = false;
        }
        
        private void Start()
        {
            moveDirection = Vector2.zero;
            lastDirection = UserInput.Direction.None;
        }

        private void Update()
        {
            HandleMoves();
        }

        private void HandleMoves()
        {
            if (!canMove)
                return;
            
            UserInput.Direction direction = UserInput.FindBestDirection(playerId);

            switch (direction)
            {
                case UserInput.Direction.Left:
                    moveDirection = Vector2.left;
                    break;
                case UserInput.Direction.Right:
                    moveDirection = Vector2.right;
                    break;
                case UserInput.Direction.None:
                    moveDirection = Vector2.zero;
                    break;
            }

            if (direction != UserInput.Direction.None && direction == lastDirection)
            {
                acceleration = Mathf.Clamp(acceleration + Time.deltaTime, 1f, 3f);
            }
            else
            {
                acceleration = 1;
            }

            lastDirection = direction;
        }

        private void FixedUpdate()
        {
            rb.velocity = moveDirection * (Speed * acceleration);
        }

        public void UnlockMove()
        {
            canMove = true;
        }

        public void LockMove()
        {
            canMove = false;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public Vector3 GetVelocity()
        {
            return rb.velocity;
        }
        
        public PlayerID GetPlayerId()
        {
            return playerId;
        }
    }
}
