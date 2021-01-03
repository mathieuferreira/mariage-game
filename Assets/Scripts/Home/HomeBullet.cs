using UnityEngine;

namespace Home
{
    public class HomeBullet : MonoBehaviour
    {
        private const float Speed = 20f;
        private const float DisappearXPosition = 40f;

        [SerializeField] private Explosion explosion = default;
    
        private PlayerID playerId;
        private Rigidbody2D rigidBody;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            rigidBody.velocity = Vector2.right * Speed;
        }

        private void FixedUpdate()
        {
            if (transform.position.x > DisappearXPosition)
            {
                Destroy(gameObject);
            }
        }

        public void Setup(PlayerID player, Sprite sprite)
        {
            playerId = player;
            GetComponent<SpriteRenderer>().sprite = sprite;
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
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
