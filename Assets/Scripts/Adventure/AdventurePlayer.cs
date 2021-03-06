﻿using UnityEngine;

public class AdventurePlayer : BaseRPGPlayer
{
    [SerializeField] private Move2DConstraint moveConstraint = default;

    protected override void Move(Vector3 direction)
    {
        Vector3 candidatePosition = transform.position + direction * MOVE_SPEED * Time.deltaTime;

        if (!moveConstraint.IsPositionAllowed(candidatePosition))
            return;
        
        base.Move(direction);
    }
}
