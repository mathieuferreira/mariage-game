using System;
using Home;
using UnityEngine;

namespace Breakout
{
    public class BreakoutBrick : MonoBehaviour
    {
        [SerializeField] private Transform explosionEffect;
        [SerializeField] private Color color;

        private void Awake()
        {
            GetComponent<SpriteRenderer>().color = color;
        }

        public void Damage()
        {
            Destroy(gameObject);
            Transform explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            var main = explosion.GetComponent<ParticleSystem>().main;
            main.startColor = color;
            Destroy(explosion.gameObject, 2f);
        }
    }
}
