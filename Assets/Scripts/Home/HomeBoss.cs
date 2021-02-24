using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Home
{
    [RequireComponent(typeof(HomeDamageableEnemy))]
    public class HomeBoss : MonoBehaviour
    {
        public event EventHandler BattleStart;
        public event EventHandler OnDisappear;
    
        private const float MaxXPosition = 20f;
        private const float MinXPosition = 10f;
        private const float MoveSpeed = 10f;
        private const float ProjectileSpeed = 20f;
        private const float ProjectileAngle = 20f;
    
        [SerializeField] private Transform shootPosition = default;
        [SerializeField] private LayerMask playerMask = default;
        [SerializeField] private HomeBossProjectile projectile = default;

        private enum State
        {
            WaitingToStart,
            Idle,
            Moving,
            Attacking,
            Dead
        }

        private Animator animator;
        private State currentState;
        private float environmentSpeed;
        private HomePlayer[] targets;
        private HomePlayer currentTarget;
        private float targetPositionX;
        private HomeDamageableEnemy homeDamageableEnemy;
        private static readonly int AnimationSpeed = Animator.StringToHash("Speed");
        private static readonly int AnimationDead = Animator.StringToHash("Dead");
        private static readonly int AnimationAttack = Animator.StringToHash("Attack");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            currentState = State.WaitingToStart;
            homeDamageableEnemy = GetComponent<HomeDamageableEnemy>();
            homeDamageableEnemy.SetActive(false);
        }

        private void Start()
        {
            homeDamageableEnemy.GetHealthSystem().OnDied += OnOnDied;
        }

        private void OnOnDied(object sender, EventArgs e)
        {
            animator.SetFloat(AnimationSpeed, 0f);
            animator.SetTrigger(AnimationDead);
            currentState = State.Dead;
            Debug.Log("Boss Died");
        }

        // Update is called once per frame
        private void Update()
        {
            switch (currentState)
            {
                case State.WaitingToStart :
                    transform.position += Vector3.left * (environmentSpeed * Time.deltaTime);

                    if (transform.position.x < (MaxXPosition + MinXPosition) / 2)
                    {
                        currentState = State.Idle;
                        homeDamageableEnemy.SetActive(true);
                        BattleStart?.Invoke(this, EventArgs.Empty);
                    }

                    break;
                case State.Moving:
                    HandleMoving();
                    break;
                case State.Attacking:
                    break;
                case State.Dead:
                    break;
                default:
                    HandleIdle();
                    break;
            }
        }

        private void HandleIdle()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 50, playerMask);

            if (hit.collider != null)
            {
                StartAttacking();
            }
            else
            {
                StartMoving();
            }
        }

        private void HandleMoving()
        {
            Vector3 targetPosition = new Vector3(targetPositionX, currentTarget.GetPosition().y, 0);
            Transform trans = transform;
            Vector3 position = trans.position;
            Vector3 dir = (targetPosition - position).normalized;

            position += dir * (MoveSpeed * Time.deltaTime);
            trans.position = position;

            if (Math.Abs(transform.position.y - targetPosition.y) < 0.2f)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, 50, playerMask);

                if (hit.collider != null)
                {
                    StartAttacking();
                }
            }
        }

        private void StartMoving()
        {
            currentTarget = targets[Random.Range(0, targets.Length)];
            targetPositionX = Random.Range(MinXPosition, MaxXPosition);
            currentState = State.Moving;
            animator.SetFloat(AnimationSpeed, MoveSpeed);
        }

        private void StartAttacking()
        {
            if (currentState == State.Dead)
                return;

            animator.SetFloat(AnimationSpeed, 0f);
            animator.SetTrigger(AnimationAttack);
            currentState = State.Attacking;
        }

        public void LaunchProjectile()
        {
            Vector3 shootPos = shootPosition.position;
            HomeBossProjectile projectileMiddle = Instantiate(projectile, shootPos, Quaternion.Euler(0f, 0f, 180f));
            projectileMiddle.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.left * ProjectileSpeed;

            Quaternion quaternionProjectileUp = Quaternion.Euler(0, 0, 180f - ProjectileAngle);
            HomeBossProjectile projectileUp = Instantiate(projectile, shootPos, quaternionProjectileUp);
            projectileUp.gameObject.GetComponent<Rigidbody2D>().velocity = quaternionProjectileUp * Vector3.right * ProjectileSpeed;
        
            Quaternion quaternionProjectileDown = Quaternion.Euler(0, 0, 180f + ProjectileAngle);
            HomeBossProjectile projectileDown = Instantiate(projectile, shootPos, quaternionProjectileDown);
            projectileDown.gameObject.GetComponent<Rigidbody2D>().velocity = quaternionProjectileDown * Vector3.right * ProjectileSpeed;

            SoundManager.GetInstance().Play("BossShoot");
        }

        public void StartIdle()
        {
            animator.SetFloat(AnimationSpeed, 0f);
            currentState = State.Idle;
        }

        public void Disappear()
        {
            Debug.Log("Boss Disappear");
            OnDisappear?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }

        public void Setup(HomePlayer[] players, float envSpeed)
        {
            targets = players;
            environmentSpeed = envSpeed;
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
