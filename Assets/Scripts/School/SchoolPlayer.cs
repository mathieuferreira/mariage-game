using System;
using UnityEngine;

namespace School
{
    public class SchoolPlayer : MonoBehaviour
    {
        public event EventHandler<ShurikenLaunchEventArgs> OnShurikenLaunch;
    
        //private static float ACCELERATION = 18f;
        //private static float DECCELERATION = 10f;
        private static float SPEED = 8f;
    
        [SerializeField] private PlayerID player = default;
        [SerializeField] private GameObject shieldImpact = default;
        private bool moveLocked;
        private Rigidbody2D rigidBody;
        private GameObject shuriken;
        private Vector2 shurikenPosition;
        private bool hasShuriken;
        private Animator animator;

        private void Awake()
        {
            moveLocked = true;
            rigidBody = GetComponent<Rigidbody2D>();
            Transform shurikenTransform = transform.Find("Shuriken");
            shuriken = shurikenTransform.gameObject;
            shurikenPosition = shurikenTransform.localPosition;
            shuriken.SetActive(false);
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (moveLocked)
                return;

            Deccelerate();

            UserInput.Direction direction = UserInput.FindBestDirection(player);

            switch (direction)
            {
                case UserInput.Direction.Up:
                    Move(Vector2.up);
                    break;
                case UserInput.Direction.Down:
                    Move(Vector2.down);
                    break;
            }

            if (UserInput.IsActionKeyDown(player))
                LaunchShuriken();
        }

        private void Deccelerate()
        {
            /*float currentVelocity = rigidBody.velocity.y;
            float potentialDeccelerationAmount = DECCELERATION * Time.deltaTime;
            float deccelerationAmount = Math.Min(potentialDeccelerationAmount, Math.Abs(currentVelocity));
            rigidBody.velocity += (currentVelocity > 0 ? Vector2.down : Vector2.up) * deccelerationAmount;*/
            rigidBody.velocity = Vector2.zero;
        }

        private void Move(Vector2 move)
        {
            //rigidBody.velocity += move * (ACCELERATION * Time.deltaTime);
            rigidBody.velocity = move * SPEED;
        }

        private void LaunchShuriken()
        {
            if (!HasShuriken())
                return;
        
            hasShuriken = false;
            shuriken.SetActive(false);
            OnShurikenLaunch?.Invoke(this, new ShurikenLaunchEventArgs(transform.rotation.z == 0f ? rigidBody.position + shurikenPosition : rigidBody.position - shurikenPosition));
        }

        public void LockMove()
        {
            moveLocked = true;
        }

        public void UnlockMove()
        {
            moveLocked = false;
        }

        public bool HasShuriken()
        {
            return hasShuriken;
        }

        public void StartShuriken()
        {
            hasShuriken = true;
            shuriken.SetActive(true);
        }

        public PlayerID GetPlayerId()
        {
            return player;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void PlayBounceAnimation(Vector2 contactPoint)
        {
            animator.SetTrigger("Bounce");
            GameObject impactEffect = Instantiate(shieldImpact, contactPoint, Quaternion.Euler(0, 0, Utils.GetAngleFromVector(-1 * transform.position)));
            Destroy(impactEffect, 1f);
        }
    }

    public class ShurikenLaunchEventArgs : EventArgs
    {
        private readonly Vector2 position;

        public ShurikenLaunchEventArgs(Vector2 position)
        {
            this.position = position;
        }

        public Vector2 GetPosition()
        {
            return position;
        }
    }
}