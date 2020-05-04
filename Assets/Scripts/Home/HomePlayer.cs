using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HomePlayer : MonoBehaviour
{
    public event EventHandler OnShoot;
    
    private const float Speed = 10f;
    private const float MinXPosition = -26f;
    private const float MaxXPosition = -14f;
    private const float MinYPosition = -12.3f;
    private const float MaxYPosition = 12.3f;

    [SerializeField] private UserInput.Player playerId;
    [SerializeField] private Transform bullet;
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private Sprite bulletLaunchSprite;

    private Transform gunPosition;
    
    private void Awake()
    {
        gunPosition = transform.Find("GunPosition");
    }

    private void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        if (UserInput.isKey(playerId, UserInput.Key.Up))
        {
            Move(Vector3.up);
        } else if (UserInput.isKey(playerId, UserInput.Key.Down))
        {
            Move(Vector3.down);
        } else if (UserInput.isKey(playerId, UserInput.Key.Right))
        {
            Move(Vector3.right);
        } else if (UserInput.isKey(playerId, UserInput.Key.Left))
        {
            Move(Vector3.left);
        }
        
        if (UserInput.isKeyDown(playerId, UserInput.Key.Action))
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
        projectile.GetComponent<HomeBullet>().Setup(playerId);
        projectile.GetComponent<SpriteRenderer>().sprite = bulletSprite;

        GameObject launch = new GameObject("LaunchEffect", typeof(SpriteRenderer), typeof(HomeBulletLaunch));
        launch.GetComponent<SpriteRenderer>().sprite = bulletLaunchSprite;
        launch.transform.position = position;
        
        if (OnShoot != null)
            OnShoot(this, new HomePlayerShootEventArgs()
            {
                shootPosition = position
            });
    }
    
    public UserInput.Player GetPlayerId()
    {
        return playerId;
    }

    public class HomePlayerShootEventArgs : EventArgs
    {
        public Vector3 shootPosition;
    }
}
