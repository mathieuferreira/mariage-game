using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HomeEnemy : MonoBehaviour
{
    private float maxYPosition;
    private float minYPosition;
    private float speed;

    private Rigidbody2D rigidBody;
    private State state;
    private HealthSystem healthSystem;
    private Animator animator;

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
        healthSystem = new HealthSystem(GetMaxHealth());
        healthSystem.OnDied += OnHealthSystemDied;
    }

    private void OnHealthSystemDied(object sender, EventArgs e)
    {
        animator.SetTrigger("Dead");
    }

    public void Setup(Vector3 initialVelocity, float minPosition, float maxPosition)
    {
        initialVelocity.y = speed;
        rigidBody.velocity = initialVelocity;
        minYPosition = minPosition;
        maxYPosition = maxPosition;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (state == State.Dead)
            return;

        if (state == State.Up && transform.position.y > maxYPosition)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, -speed, 0f);
            state = State.Down;
        }
        
        if (state == State.Down && transform.position.y < minYPosition)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, speed, 0f);
            state = State.Up;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        HomeBullet bullet = other.gameObject.GetComponent<HomeBullet>();

        if (bullet != null)
        {
            healthSystem.Damage(10);
        }
    }

    protected virtual int GetMaxHealth()
    {
        return 30;
    }
}
