using System;
using System.Collections;
using System.Collections.Generic;
using Breakout;
using UnityEngine;
using Random = UnityEngine.Random;

public class BreakoutBonus : MonoBehaviour
{
    public event EventHandler OnBallMultiply;
    
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 10f;
    [SerializeField] private Type type;
    
    private float speed;

    public enum Type
    {
        BallMultiplier,
        Gun,
        ScaleIncrease
    }

    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += Vector3.down * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BreakoutPlayer player = other.GetComponent<BreakoutPlayer>();

        if (player != null)
        {
            ActivateBonus(player);
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }

    private void ActivateBonus(BreakoutPlayer player)
    {
        switch (type)
        {
            case Type.Gun:
                player.GetComponent<BreakoutBonusEffectGun>().StartShooting();
                break;
            case Type.BallMultiplier:
                OnBallMultiply?.Invoke(this, EventArgs.Empty);
                break;
            case Type.ScaleIncrease:
                player.gameObject.AddComponent<BreakoutBonusEffectScale>();
                break;
        }
    }
}
