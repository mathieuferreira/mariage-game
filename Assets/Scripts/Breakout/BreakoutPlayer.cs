using UnityEngine;

namespace Breakout
{
    public class BreakoutPlayer : MonoBehaviour
    {
        private const float Speed = 15f;
        
        [SerializeField] private PlayerID playerId;
        
        private Rigidbody2D rb;
        private bool canMove;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            canMove = false;
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
                    rb.velocity = Vector2.left * Speed;
                    break;
                case UserInput.Direction.Right:
                    rb.velocity = Vector2.right * Speed;
                    break;
                default:
                    rb.velocity = Vector2.zero;
                    break;
            }
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
