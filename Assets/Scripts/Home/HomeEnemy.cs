using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class HomeEnemy : MonoBehaviour
{
    private const float MaxColorTimer = .2f;
    private static Color DamageColor = GameColor.RED;
    
    private float maxYPosition;
    private float minYPosition;
    private float speed;

    private Rigidbody2D rigidBody;
    private State state;
    private HealthSystem healthSystem;
    private Animator animator;
    private float nextPosition;
    private float colorTimer;
    private SpriteRenderer[] sprites;

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
        colorTimer = 0f;
        
        sprites = new SpriteRenderer[9];
        sprites[0] = transform.Find("Spider").GetComponent<SpriteRenderer>();
        sprites[1] = transform.Find("Spider").Find("Paw1").GetComponent<SpriteRenderer>();
        sprites[2] = transform.Find("Spider").Find("Paw2").GetComponent<SpriteRenderer>();
        sprites[3] = transform.Find("Spider").Find("Paw3").GetComponent<SpriteRenderer>();
        sprites[4] = transform.Find("Spider").Find("Paw4").GetComponent<SpriteRenderer>();
        sprites[5] = transform.Find("Spider").Find("Paw5").GetComponent<SpriteRenderer>();
        sprites[6] = transform.Find("Spider").Find("Paw6").GetComponent<SpriteRenderer>();
        sprites[7] = transform.Find("Spider").Find("Paw7").GetComponent<SpriteRenderer>();
        sprites[8] = transform.Find("Spider").Find("Paw8").GetComponent<SpriteRenderer>();
    }

    private void OnHealthSystemDied(object sender, EventArgs e)
    {
        animator.SetTrigger("Dead");
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
        HandleColor();
        HandleMovement();
    }

    private void HandleColor()
    {
        if (state == State.Dead || colorTimer < 0f)
            return;

        colorTimer -= Time.deltaTime;
        if (colorTimer < 0f)
            colorTimer = 0f;

        float r = DamageColor.r + (1f - DamageColor.r) * (MaxColorTimer - colorTimer) / MaxColorTimer;
        float g = DamageColor.g + (1f - DamageColor.g) * (MaxColorTimer - colorTimer) / MaxColorTimer;
        float b = DamageColor.b + (1f - DamageColor.b) * (MaxColorTimer - colorTimer) / MaxColorTimer;

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].color = new Color(r, g, b, 1f);
        }
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

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        HomeBullet bullet = other.gameObject.GetComponent<HomeBullet>();

        if (bullet != null)
        {
            colorTimer = MaxColorTimer;
            healthSystem.Damage(10);
        }
    }

    protected virtual int GetMaxHealth()
    {
        return 30;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}
