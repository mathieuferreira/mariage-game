using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public static class Utils
{
    public static float ConvertAngle360(float angle)
    {
        while (angle > 360)
            angle -= 360;

        while (angle < 0)
            angle += 360;

        return angle;
    }

    public static float ConvertAngle180(float angle)
    {
        while (angle > 180)
            angle -= 360;

        while (angle < -180)
            angle += 360;

        return angle;
    }
}

