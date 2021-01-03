using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Move2DConstraint
{
    [SerializeField] private Transform constrainPivot = default;
    [SerializeField] private float maxXDistance = default;
    [SerializeField] private float maxYDistance = default;

    public bool IsPositionAllowed(Vector3 candidatePosition)
    {
        Vector3 distance = constrainPivot.position - candidatePosition;

        return Math.Abs(distance.x) < maxXDistance && Math.Abs(distance.y) < maxYDistance;
    }
}
