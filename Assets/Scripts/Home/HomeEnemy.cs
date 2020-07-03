using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Home
{
    [RequireComponent(typeof(HomeDamageableEnemy))]
    public class HomeEnemy : MonoBehaviour
    {
        public event EventHandler OnDisappear;
    
        private float maxYPosition;
        private float minYPosition;
        private float speed;

        private Rigidbody2D rigidBody;
        private State state;
        private Animator animator;
        private float nextPosition;
        private HomeDamageableEnemy homeDamageableEnemy;
        private static readonly int AnimationDead = Animator.StringToHash("Dead");

        private enum State
        {
            Up,
            Down,
            Dead
        }

        private void Awake()
        {
            state = State.Up;
            speed = Random.Range(2f, 3f);
            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            homeDamageableEnemy = GetComponent<HomeDamageableEnemy>();
        }

        private void Start()
        {
            homeDamageableEnemy.GetHealthSystem().OnDied += OnHealthSystemDied;
        }

        private void OnHealthSystemDied(object sender, EventArgs e)
        {
            Disappear();
            state = State.Dead;
        }

        public void Setup(Vector3 initialVelocity, float minPosition, float maxPosition)
        {
            minYPosition = minPosition;
            maxYPosition = maxPosition;
            rigidBody.velocity = initialVelocity;

            FindNextPosition();
        }

        private void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (state == State.Dead)
                return;

            if (
                state == State.Up && transform.position.y > nextPosition
                || state == State.Down && transform.position.y < nextPosition
            )
            {
                FindNextPosition();
            }
        }

        private void FindNextPosition()
        {
            nextPosition = Random.Range(minYPosition, maxYPosition);
            bool isDown = nextPosition < transform.position.y;
            state = isDown ? State.Down : State.Up;
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, isDown ? -speed : speed, 0f);
        }

        public void Disappear()
        {
            animator.SetTrigger(AnimationDead);
            OnDisappear?.Invoke(this, EventArgs.Empty);
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void OnCollisionEnter2D(Collision2D other)
        {
            HomePlayer player = other.gameObject.GetComponent<HomePlayer>();

            if (player != null)
            {
                player.Damage();
                Disappear();
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public HealthSystem GetHealthSystem()
        {
            return homeDamageableEnemy.GetHealthSystem();
        }
    }
}
