using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CubicBezierCurve
{
    [SerializeField] private float p0 = default;
    [SerializeField] private float p1 = default;
    [SerializeField] private float p2 = default;
    [SerializeField] private float p3 = default;
    
    public CubicBezierCurve()
    {
        p0 = 0f;
        p1 = 0.5f;
        p2 = 0.5f;
        p3 = 1f;
    }

    public CubicBezierCurve(float p0, float p1, float p2, float p3)
    {
        this.p0 = p0;
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
    }
    
    public float GetPosition(float t)
    {
        var it = 1f - t;
        return (it * it * it * p0)
               + (3f * it * it * t * p1)
               + (3f * it * t * t * p2)
               + (t * t * t * p3);
    }

    /// <summary>
    /// Represents an eased interpolation w/ respect to time.
    /// 
    /// float t, float b, float c, float d
    /// </summary>
    /// <param name="current">how long into the ease are we</param>
    /// <param name="initialValue">starting value if current were 0</param>
    /// <param name="totalChange">total change in the value (not the end value, but the end - start)</param>
    /// <param name="duration">the total amount of time (when current == duration, the returned value will == initial + totalChange)</param>
    /// <returns></returns>
    public float Ease(float current, float initialValue, float totalChange, float duration)
    {
        var t = current / duration;
        var r = GetPosition(t);
        return initialValue + totalChange * r;
    }
}
