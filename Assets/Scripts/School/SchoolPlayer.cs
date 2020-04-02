using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolPlayer : MonoBehaviour
{
    [SerializeField] private int playerId;
    private bool moveLocked;

    private void Awake()
    {
        moveLocked = true;
    }

    private void Update()
    {
        if (moveLocked)
            return;

        if (UserInput.isKeyDown(playerId, UserInput.Key.Up))
            MoveUp();

        if (UserInput.isKeyDown(playerId, UserInput.Key.Down))
            MoveDown();

        if (UserInput.isKeyDown(playerId, UserInput.Key.Action))
            MoveAction();
    }

    private void MoveUp()
    {
        
    }

    private void MoveDown()
    {
        
    }

    private void MoveAction()
    {
        
    }

    public void LockMove()
    {
        moveLocked = true;
    }

    public void UnlockMove()
    {
        moveLocked = false;
    }
}
