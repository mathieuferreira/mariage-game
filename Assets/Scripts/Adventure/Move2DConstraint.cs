using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Move2DConstraint
{
    [SerializeField] private Transform constrainPivot;
    [SerializeField] private float maxXDistance;
    [SerializeField] private float maxYDistance;

    public bool IsPositionAllowed(Vector3 candidatePosition)
    {
        Vector3 distance = constrainPivot.position - candidatePosition;

        Debug.Log(distance);
        return Math.Abs(distance.x) < maxXDistance && Math.Abs(distance.y) < maxYDistance;
    }
}
