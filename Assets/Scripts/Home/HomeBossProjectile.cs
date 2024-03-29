﻿using UnityEngine;

namespace Home
{
    public class HomeBossProjectile : MonoBehaviour
    {
        private const float DisappearXPosition = -40f;

        [SerializeField] private Explosion explosion = default;

        private void FixedUpdate()
        {
            if (transform.position.x < DisappearXPosition)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            HomePlayer player = other.gameObject.GetComponent<HomePlayer>();

            if (player != null)
            {
                player.Damage();
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
