using System;
using CodeMonkey.Utils;
using UnityEngine;

namespace Breakout
{
    public class BreakoutBall : MonoBehaviour
    {
        public event EventHandler OnDisappear;

        public static float InitialSpeed = 12f;
        private const float LateralBouncyFactor = 2f;
        private const float PlayerVelocityBouncyFactor = .2f;
        
        private Rigidbody2D rb;
        private PlayerID lastPlayerBounce;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Setup(PlayerID lastPlayer, Vector3 initialVelocity)
        {
            lastPlayerBounce = lastPlayer;
            rb.velocity = initialVelocity;
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

                Vector3 v = velocity + delta * LateralBouncyFactor +
                            player.GetVelocity().normalized * PlayerVelocityBouncyFactor;

                if (v.sqrMagnitude < InitialSpeed * InitialSpeed)
                {
                    v = v * InitialSpeed / v.magnitude;
                }
                
                rb.velocity = v;
            }

            BreakoutBrick brick = other.transform.GetComponent<BreakoutBrick>();

            if (brick != null)
            {
                brick.Damage();
                UtilsClass.ShakeCamera(.05f, .1f);
            }
        }

        public Vector3 GetVelocity()
        {
            return rb.velocity;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public PlayerID GetLastPlayerBounce()
        {
            return lastPlayerBounce;
        }
    }
}
