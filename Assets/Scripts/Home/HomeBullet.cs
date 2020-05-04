using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBullet : MonoBehaviour
{
    private const float Speed = 20f;
    private const float DisappearXPosition = 40f;
    
    private UserInput.Player playerId;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private bool exploding;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = Vector2.right * Speed;
        animator = GetComponent<Animator>();
        exploding = false;
    }

    private void FixedUpdate()
    {
        if (transform.position.x > DisappearXPosition)
        {
            DestroySelf();
        }
    }

    public void Setup(UserInput.Player player)
    {
        playerId = player;
    }

    public UserInput.Player GetPlayerId()
    {
        return playerId;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void Explode()
    {
        rigidBody.velocity = Vector2.zero;
        Destroy(GetComponent<BoxCollider2D>());
        animator.SetTrigger("Explode");
        exploding = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        HomeEnemy enemy = other.gameObject.GetComponent<HomeEnemy>();
        if (enemy != null)
        {
            Explode();
        }
    }
}
