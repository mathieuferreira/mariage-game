using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolShuriken : MonoBehaviour
{
    public event EventHandler OnDestroyed;
    
    private static float ROTATION_SPEED = 2f;
    private static float INITIAL_SPEED = 4f;
    private static float DESTROY_MIN_X = -14f;
    private static float DESTROY_MAX_X = 14f;

    private Vector3 velocity;
    private bool isDestroyed;

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
            isDestroyed = true;
            if (OnDestroyed != null)
                OnDestroyed(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }

    public void StartMoving()
    {
        velocity = (transform.position.x > 0 ? Vector3.left : Vector3.right) * INITIAL_SPEED;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rigidbody2D = other.transform.GetComponent<Rigidbody2D>();
            Vector3 playerPosition = rigidbody2D.position;
            Vector3 playerVelocity = rigidbody2D.velocity;
            velocity = new Vector3( - velocity.x, (transform.position.y - playerPosition.y) * 2f + playerVelocity.y * .75f , 0f);
        }
        else if(other.gameObject.CompareTag("Border"))
        {
            velocity = new Vector3(velocity.x, -velocity.y, 0f);
        }
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }
}
