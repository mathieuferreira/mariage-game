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

        private Vector3 initialVelocity;
        private bool checkVelocity = false;
        private bool freezed = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Setup(PlayerID lastPlayer, Vector3 initVelocity)
        {
            lastPlayerBounce = lastPlayer;
            initialVelocity = initVelocity;
            rb.velocity = initVelocity;
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
                SoundManager.GetInstance().Play("PlayerImpact");
            }

            BreakoutBrick brick = other.transform.GetComponent<BreakoutBrick>();

            if (brick != null)
            {
                brick.Damage();
                UtilsClass.ShakeCamera(.05f, .1f);
                ScoreManager.IncrementScore(lastPlayerBounce);
                SoundManager.GetInstance().Play("BrickHit");
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            checkVelocity = true;
        }

        private void LateUpdate()
        {
            if (!freezed && checkVelocity)
            {
                Vector3 velocity = rb.velocity;
                if (velocity.sqrMagnitude < InitialSpeed * InitialSpeed)
                {
                    float magnitude = velocity.magnitude;

                    if (magnitude == 0)
                    {
                        velocity = initialVelocity;
                    }
                    else
                    {
                        velocity = velocity * InitialSpeed / magnitude;
                    }
                }
                
                float angle = Vector3.Angle(Vector3.right, velocity);

                if (angle < Mathf.PI * 15 / 180)
                {
                    velocity.y = velocity.y / Mathf.Abs(velocity.y) * Mathf.Abs(velocity.x) * Mathf.Tan(Mathf.PI  * 15 / 180);
                }

                rb.velocity = velocity;
                checkVelocity = false;
            }
        }

        public void Stop()
        {
            freezed = true;
            rb.velocity = Vector2.zero;
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
