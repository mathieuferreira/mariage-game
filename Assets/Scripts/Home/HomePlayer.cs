﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class HomePlayer : MonoBehaviour
{
    public event EventHandler OnShoot;
    public event EventHandler OnDamaged;
    
    private const float Speed = 10f;
    private const float MinXPosition = -26f;
    private const float MaxXPosition = -14f;
    private const float MinYPosition = HomeLevel.MinYPosition;
    private const float MaxYPosition = HomeLevel.MaxYPosition;

    [SerializeField] private PlayerID playerId;
    [SerializeField] private Transform bullet;
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private HomeBulletFlash shootFlash;

    private Transform gunPosition;
    private bool moveLocked;
    
    private void Awake()
    {
        gunPosition = transform.Find("GunPosition");
        moveLocked = true;
    }

    private void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        if (moveLocked)
            return;

        UserInput.Direction direction = UserInput.FindBestDirection(playerId);

        switch (direction)
        {
            case UserInput.Direction.Up:
                Move(Vector3.up);
                break;
            case UserInput.Direction.Down:
                Move(Vector3.down);
                break;
            case UserInput.Direction.Left:
                Move(Vector3.left);
                break;
            case UserInput.Direction.Right:
                Move(Vector3.right);
                break;
        }
        
        if (UserInput.IsActionKeyDown(playerId))
        {
            Shoot();
        }
    }

    private void Move(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction * (Speed * Time.deltaTime);
        
        if (
            newPosition.x > MinXPosition && newPosition.x < MaxXPosition
            && newPosition.y > MinYPosition && newPosition.y < MaxYPosition
        )
            transform.position = newPosition;
    }

    private void Shoot()
    {
        Vector3 position = gunPosition.position;
        Transform projectile = Instantiate(bullet, position, bullet.rotation);
        projectile.GetComponent<HomeBullet>().Setup(playerId, bulletSprite);

        shootFlash.Start();
        
        if (OnShoot != null)
            OnShoot(this, EventArgs.Empty);
    }
    
    public PlayerID GetPlayerId()
    {
        return playerId;
    }

    public void Damage()
    {
        if (OnDamaged != null)
            OnDamaged(this, EventArgs.Empty);
    }

    public void LockMove()
    {
        moveLocked = true;
    }

    public void UnlockMove()
    {
        moveLocked = false;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
