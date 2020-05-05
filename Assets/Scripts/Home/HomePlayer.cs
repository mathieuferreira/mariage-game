using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HomePlayer : MonoBehaviour
{
    public event EventHandler<HomePlayerShootEventArgs> OnShoot;
    public event EventHandler<HomePlayerDamagedEventArgs> OnDamaged;
    
    private const float Speed = 10f;
    private const float MinXPosition = -26f;
    private const float MaxXPosition = -14f;
    private const float MinYPosition = HomeLevel.MinYPosition;
    private const float MaxYPosition = HomeLevel.MaxYPosition;

    [SerializeField] private UserInput.Player playerId;
    [SerializeField] private Transform bullet;
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private Sprite bulletLaunchSprite;

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
        SpriteRenderer launchSpriteRenderer = launch.GetComponent<SpriteRenderer>();
        launchSpriteRenderer.sprite = bulletLaunchSprite;
        launchSpriteRenderer.sortingOrder = 110;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        HomeEnemy spider = other.gameObject.GetComponent<HomeEnemy>();

        if (spider != null)
        {
            spider.DestroySelf();

            if (OnDamaged != null)
                OnDamaged(this, new HomePlayerDamagedEventArgs() { spider = spider });
        }
    }

    public void LockMove()
    {
        moveLocked = true;
    }

    public void UnlockMove()
    {
        moveLocked = false;
    }

    public class HomePlayerShootEventArgs : EventArgs
    {
        public Vector3 shootPosition;
    }

    public class HomePlayerDamagedEventArgs : EventArgs
    {
        public HomeEnemy spider;
    }
}
