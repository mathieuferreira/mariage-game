using System.Collections.Generic;
using UnityEngine;

namespace Home
{
    public class HomeDamageableEnemy : MonoBehaviour
    {
        private const float MaxColorTimer = .2f;
        private static readonly Color DamageColor = GameColor.RED;
    
        [SerializeField] private int maxHealth = default;
    
        private HealthSystem healthSystem;
        private float colorTimer;
        private List<SpriteRenderer> spriteRenderers;
        private bool active = true;

        private void Awake()
        {
            healthSystem = new HealthSystem(maxHealth);

            spriteRenderers = new List<SpriteRenderer>();
            FindSpriteRenderersRecursively(transform);
        }

        private void Update()
        {
            if (!healthSystem.IsAlive() || colorTimer < 0f)
                return;

            colorTimer -= Time.deltaTime;
            if (colorTimer < 0f)
                colorTimer = 0f;

            float r = DamageColor.r + (1f - DamageColor.r) * (MaxColorTimer - colorTimer) / MaxColorTimer;
            float g = DamageColor.g + (1f - DamageColor.g) * (MaxColorTimer - colorTimer) / MaxColorTimer;
            float b = DamageColor.b + (1f - DamageColor.b) * (MaxColorTimer - colorTimer) / MaxColorTimer;

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = new Color(r, g, b, 1f);
            }
        }

        private void FindSpriteRenderersRecursively(Transform trans)
        {
            SpriteRenderer spriteRenderer = trans.GetComponent<SpriteRenderer>();
            if (trans.gameObject.name != "Web" && spriteRenderer != null)
                spriteRenderers.Add(spriteRenderer);
        
            foreach (Transform child in trans)
            {
                FindSpriteRenderersRecursively(child);
            }
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!active)
                return;
        
            HomeBullet bullet = other.gameObject.GetComponent<HomeBullet>();

            if (bullet != null)
            {
                colorTimer = MaxColorTimer;
                healthSystem.Damage(10);
            }
        }

        public HealthSystem GetHealthSystem()
        {
            return healthSystem;
        }

        public void SetActive(bool isActive)
        {
            active = isActive;
        }
    }
}
