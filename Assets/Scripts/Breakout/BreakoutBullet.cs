using System;
using UnityEngine;

namespace Breakout
{
    public class BreakoutBullet : MonoBehaviour
    {
        [SerializeField] private float velocity = 20f;
        
        private PlayerID playerId;
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            rb.velocity = Vector3.up * velocity;
        }

        public void Setup(PlayerID player)
        {
            playerId = player;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            BreakoutBrick brick = other.GetComponent<BreakoutBrick>();

            if (brick != null)
            {
                brick.Damage();
                Destroy(gameObject);
            }

            if (other.CompareTag("Border"))
            {
                Destroy(gameObject);
            }
        }
    }
}
