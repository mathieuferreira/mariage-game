using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class AdventurePlayer : MonoBehaviour
{
    private static float MOVE_SPEED = 4f;
    
    [SerializeField] private UserInput.Player player;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (UserInput.isKey(player, UserInput.Key.Up))
        {
            Move(Vector3.up);
        } 
        else if (UserInput.isKey(player, UserInput.Key.Down))
        {
            Move(Vector3.down);
        }
        else if (UserInput.isKey(player, UserInput.Key.Left))
        {
            Move(Vector3.left);
        }
        else if (UserInput.isKey(player, UserInput.Key.Right))
        {
            Move(Vector3.right);
        }
        else
        {
            Idle();
        }
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction * MOVE_SPEED * Time.deltaTime;
        animator.SetFloat("speed", MOVE_SPEED);
        animator.SetFloat("vertical", direction.x);
        animator.SetFloat("horizontal", direction.y);
    }

    private void Idle()
    {
        animator.SetFloat("speed", 0f);
    }
}
