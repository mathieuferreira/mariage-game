using System;
using CodeMonkey.Utils;
using UnityEngine;

namespace Breakout
{
    public class BreakoutBall : MonoBehaviour
    {
        public event EventHandler OnDisappear;

        private const float Speed = 12f;
        private const float LateralBouncyFactor = 4f;
        private const float PlayerVelocityBouncyFactor = .5f;
        
        private Rigidbody2D rb;
        private PlayerID lastPlayerBounce;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            rb.velocity = Vector2.down * Speed;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Floor"))
            {
                OnDisappear?.Invoke(this, EventArgs.Empty);
                
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            BreakoutPlayer player = other.transform.GetComponent<BreakoutPlayer>();

            if (player != null)
            {
                lastPlayerBounce = player.GetPlayerId();
                
                Vector3 velocity = rb.velocity;

                Vector3 delta = transform.position - player.GetPosition();
                delta.y = 0f;
                delta.z = 0f;
                
                rb.velocity = velocity * 1.05f + delta * LateralBouncyFactor + player.GetVelocity() * PlayerVelocityBouncyFactor;
            }

            BreakoutBrick brick = other.transform.GetComponent<BreakoutBrick>();

            if (brick != null)
            {
                brick.Damage();
                UtilsClass.ShakeCamera(.05f, .1f);
            }
        }
    }
}
