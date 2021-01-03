using UnityEngine;

namespace Breakout
{
    [RequireComponent(typeof(BreakoutPlayer))]
    public class BreakoutBonusEffectGun : MonoBehaviour
    {
        private const float Duration = 8f;
        private const float MaxShootTimer = .3f;

        [SerializeField] private BreakoutBullet bulletPrefab = default;

        private float durationTimer;
        private float shootTimer;
        private BreakoutPlayer player;

        private void Awake()
        {
            player = GetComponent<BreakoutPlayer>();
        }

        private void Start()
        {
            shootTimer = MaxShootTimer;
            durationTimer = 0f;
        }

        private void Update()
        {
            if (durationTimer <= 0f)
                return;

            durationTimer -= Time.deltaTime;
            shootTimer -= Time.deltaTime;

            if (shootTimer < 0f)
            {
                BreakoutBullet bullet = Instantiate(bulletPrefab, player.GetPosition(), Quaternion.identity);
                bullet.Setup(player.GetPlayerId());
                shootTimer += MaxShootTimer;
            }
        }

        public void StartShooting()
        {
            durationTimer = Duration;
        }
    }
}
