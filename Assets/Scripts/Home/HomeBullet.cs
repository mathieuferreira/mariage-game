using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Home
{
    public class HomeBullet : MonoBehaviour
    {
        private const float Speed = 20f;
        private const float DisappearXPosition = 40f;

        [SerializeField] private Explosion explosion = default;
        [SerializeField] private Light2D bulletLight = default;
    
        private PlayerID playerId;
        private Rigidbody2D rigidBody;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            rigidBody.velocity = Vector2.right * Speed;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            if (transform.position.x > DisappearXPosition)
            {
                Destroy(gameObject);
            }
        }

        public void Setup(PlayerID player, Sprite sprite, Color lightColor)
        {
            playerId = player;
            spriteRenderer.sprite = sprite;
            bulletLight.color = lightColor;
        }

        public PlayerID GetPlayerId()
        {
            return playerId;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                ScoreManager.IncrementScore(playerId);
                SoundManager.GetInstance().Play("Explosion");
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
