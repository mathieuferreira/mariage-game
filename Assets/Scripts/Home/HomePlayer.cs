using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Home
{
    public class HomePlayer : MonoBehaviour
    {
        public event EventHandler OnShoot;
        public event EventHandler OnDamaged;
    
        private const float Speed = 10f;
        private const float MinXPosition = -26f;
        private const float MaxXPosition = -14f;
        private const float MinYPosition = HomeLevel.MinYPosition;
        private const float MaxYPosition = HomeLevel.MaxYPosition;

        [SerializeField] private PlayerID playerId = default;
        [SerializeField] private HomeBullet bullet = default;
        [SerializeField] private Sprite bulletSprite = default;
        [SerializeField] private HomeBulletFlash shootFlash = default;
        [SerializeField] private Light2D playerLight;
        [SerializeField] private Tutorial moveTutorial;
        [SerializeField] private Tutorial shootTutorial;

        private Transform gunPosition;
        private bool moveLocked;
        private Color lightColor;
    
        private void Awake()
        {
            gunPosition = transform.Find("GunPosition");
            moveLocked = true;
            lightColor = playerLight.color;
            moveTutorial.Hide();
            shootTutorial.Hide();
        }

        private void Update()
        {
            HandleInputs();
        }

        private void HandleInputs()
        {
            if (moveLocked)
                return;

            UserInput.Direction direction = UserInput.FindBestDirection(playerId);

            switch (direction)
            {
                case UserInput.Direction.Up:
                    Move(Vector3.up);
                    break;
                case UserInput.Direction.Down:
                    Move(Vector3.down);
                    break;
                case UserInput.Direction.Left:
                    Move(Vector3.left);
                    break;
                case UserInput.Direction.Right:
                    Move(Vector3.right);
                    break;
            }
        
            if (UserInput.IsActionKeyDown(playerId))
            {
                Shoot();
            }
        }

        private void Move(Vector3 direction)
        {
            Vector3 newPosition = transform.position + direction * (Speed * Time.deltaTime);

            if (
                newPosition.x > MinXPosition 
                && newPosition.x < MaxXPosition
                && newPosition.y > MinYPosition 
                && newPosition.y < MaxYPosition
            )
            {
                transform.position = newPosition;
                moveTutorial.Complete();
            }
        }

        private void Shoot()
        {
            Vector3 position = gunPosition.position;
            HomeBullet projectile = Instantiate(bullet, position, Quaternion.Euler(0f, 0f, -90f));
            projectile.Setup(playerId, bulletSprite, lightColor);

            shootFlash.Start();
            SoundManager.GetInstance().Play("Laser");
            shootTutorial.Complete();

            OnShoot?.Invoke(this, EventArgs.Empty);
        }
    
        public PlayerID GetPlayerId()
        {
            return playerId;
        }

        public void Damage()
        {
            SoundManager.GetInstance().Play("PlayerHurt");
            OnDamaged?.Invoke(this, EventArgs.Empty);
        }

        public void LockMove()
        {
            moveLocked = true;
        }

        public void UnlockMove()
        {
            moveLocked = false;
            moveTutorial.Show();
            shootTutorial.Show();
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}
