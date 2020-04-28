using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolShuriken : MonoBehaviour
{
    public event EventHandler OnDestroyed;
    public event EventHandler OnHit;
    
    private static float ROTATION_SPEED = 2f;
    private static float INITIAL_SPEED = 4f;
    private static float DESTROY_MIN_X = -14f;
    private static float DESTROY_MAX_X = 14f;

    private Vector3 velocity;
    private bool isDestroyed;
    private UserInput.Player lastPlayerTouched;

    private void Awake()
    {
        velocity = Vector3.zero;
        isDestroyed = false;
    }

    private void Update()
    {
        var transform1 = transform;
        transform1.localEulerAngles += new Vector3(0, 0, 360 * ROTATION_SPEED * Time.deltaTime);
        transform1.position += velocity * Time.deltaTime;

        if (transform1.position.x > DESTROY_MAX_X || transform1.position.x < DESTROY_MIN_X)
        {
            DestroySelf();
        }
    }

    public void StartMoving(UserInput.Player player)
    {
        lastPlayerTouched = player;
        velocity = (transform.position.x > 0 ? Vector3.left : Vector3.right) * INITIAL_SPEED;
    }

    public void StartMoving(UserInput.Player player, Vector3 velocity)
    {
        this.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        SchoolPlayer player = other.gameObject.GetComponent<SchoolPlayer>();

        if (player != null)
        {
            lastPlayerTouched = player.GetPlayerId();
            Rigidbody2D rigidbody2D = other.transform.GetComponent<Rigidbody2D>();
            Vector3 playerPosition = rigidbody2D.position;
            Vector3 playerVelocity = rigidbody2D.velocity;
            velocity = new Vector3( - velocity.x * 1.1f, (transform.position.y - playerPosition.y) * 2f + playerVelocity.y * .75f , 0f);
        }
        else if(other.gameObject.CompareTag("Border"))
        {
            velocity = new Vector3(velocity.x, -velocity.y, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SchoolEnemy enemy = other.GetComponent<SchoolEnemy>();

        if (enemy != null && enemy.GetHealthSystem().IsAlive())
        {
            enemy.GetHealthSystem().Damage(100);
            if (OnHit != null)
                OnHit(this, EventArgs.Empty);
        }
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void DestroySelf()
    {
        isDestroyed = true;
        if (OnDestroyed != null)
            OnDestroyed(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public UserInput.Player GetLastPlayerTouched()
    {
        return lastPlayerTouched;
    }
}
