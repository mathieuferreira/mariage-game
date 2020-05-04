using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBullet : MonoBehaviour
{
    private const float Speed = 20f;
    private const float DisappearXPosition = 40f;
    
    private UserInput.Player playerId;

    private void Awake()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.right * Speed;
    }

    private void FixedUpdate()
    {
        if (transform.position.x > DisappearXPosition)
        {
            Destroy(gameObject);
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
}
