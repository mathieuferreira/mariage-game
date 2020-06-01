using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBullet : MonoBehaviour
{
    private const float Speed = 20f;
    private const float DisappearXPosition = 40f;

    [SerializeField] private Explosion explosion;
    
    private UserInput.Player playerId;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = Vector2.right * Speed;
    }

    private void FixedUpdate()
    {
        if (transform.position.x > DisappearXPosition)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(UserInput.Player player, Sprite sprite)
    {
        playerId = player;
        GetComponent<SpriteRenderer>().sprite = sprite;

        Debug.Log("Setup");
        Debug.Log(sprite);
        Debug.Log(GetComponent<SpriteRenderer>().sprite);
    }

    public UserInput.Player GetPlayerId()
    {
        return playerId;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
