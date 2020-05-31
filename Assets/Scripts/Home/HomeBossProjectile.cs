using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBossProjectile : MonoBehaviour
{
    private const float DisappearXPosition = -40f;

    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (transform.position.x < DisappearXPosition)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HomePlayer player = other.gameObject.GetComponent<HomePlayer>();

        if (player != null)
        {
            player.Damage();
            animator.SetTrigger("Explode");
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
