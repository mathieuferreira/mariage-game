using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolPlayer : MonoBehaviour
{
    public event EventHandler<ShurikenLaunchEventArgs> OnShurikenLaunch;
    
    private static float ACCELERATION = 18f;
    private static float DECCELERATION = 10f;
    
    [SerializeField] private PlayerID player;
    private bool moveLocked;
    private Rigidbody2D rigidBody;
    private GameObject shuriken;
    private Vector2 shurikenPosition;
    private bool hasShuriken;

    private void Awake()
    {
        moveLocked = true;
        rigidBody = GetComponent<Rigidbody2D>();
        Transform shurikenTransform = transform.Find("Shuriken");
        shuriken = shurikenTransform.gameObject;
        shurikenPosition = shurikenTransform.localPosition;
        shuriken.SetActive(false);
    }

    private void Update()
    {
        if (moveLocked)
            return;

        Deccelerate();

        UserInput.Direction direction = UserInput.FindBestDirection(player);

        switch (direction)
        {
            case UserInput.Direction.Up:
                MoveUp();
                break;
            case UserInput.Direction.Down:
                MoveDown();
                break;
        }

        if (UserInput.IsActionKeyDown(player))
            LaunchShuriken();
    }

    private void Deccelerate()
    {
        float currentVelocity = rigidBody.velocity.y;
        float potentialDeccelerationAmount = DECCELERATION * Time.deltaTime;
        float deccelerationAmount = Math.Min(potentialDeccelerationAmount, Math.Abs(currentVelocity));
        rigidBody.velocity += (currentVelocity > 0 ? Vector2.down : Vector2.up) * deccelerationAmount;
    }

    private void MoveUp()
    {
        rigidBody.velocity += Vector2.up * ACCELERATION * Time.deltaTime;
    }

    private void MoveDown()
    {
        rigidBody.velocity += Vector2.down * ACCELERATION * Time.deltaTime;
    }

    private void LaunchShuriken()
    {
        if (!HasShuriken())
            return;
        
        hasShuriken = false;
        shuriken.SetActive(false);
        if (OnShurikenLaunch != null)
            OnShurikenLaunch(this, new ShurikenLaunchEventArgs(transform.rotation.z == 0f ? rigidBody.position + shurikenPosition : rigidBody.position - shurikenPosition));
    }

    public void LockMove()
    {
        moveLocked = true;
    }

    public void UnlockMove()
    {
        moveLocked = false;
    }

    public bool HasShuriken()
    {
        return hasShuriken;
    }

    public void StartShuriken()
    {
        hasShuriken = true;
        shuriken.SetActive(true);
    }

    public PlayerID GetPlayerId()
    {
        return player;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}

public class ShurikenLaunchEventArgs : EventArgs
{
    private Vector2 position;

    public ShurikenLaunchEventArgs(Vector2 position)
    {
        this.position = position;
    }

    public Vector2 getPosition()
    {
        return position;
    }
}
