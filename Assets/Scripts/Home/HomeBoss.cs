using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Random = UnityEngine.Random;

public class HomeBoss : MonoBehaviour
{
    public event EventHandler battleStart; 
    
    private const float MaxXPosition = 20f;
    private const float MinXPosition = 10f;
    private const float MoveSpeed = 10f;
    private const float ProjectileSpeed = 20f;
    private const float ProjectileAngle = 20f;
    
    [SerializeField] private Transform shootPosition;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private HomeBossProjectile projectile;

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
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentState = State.WaitingToStart;
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
                    battleStart?.Invoke(this, EventArgs.Empty);
                }

                break;
            case State.Moving:
                HandleMoving();
                break;
            case State.Attacking:
                break;
            default:
            case State.Idle :
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
        Vector3 dir = (targetPosition - transform.position).normalized;

        transform.position += dir * MoveSpeed * Time.deltaTime;

        Debug.Log(transform.position.y + " - " + targetPosition.y + " = " + Math.Abs(transform.position.y - targetPosition.y));
        
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
        animator.SetFloat("Speed", MoveSpeed);
    }

    private void StartAttacking()
    {
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Attack");
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
    }

    public void StartIdle()
    {
        animator.SetFloat("Speed", 0f);
        currentState = State.Idle;
    }

    public void Setup(HomePlayer[] targets, float environmentSpeed)
    {
        this.targets = targets;
        this.environmentSpeed = environmentSpeed;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
