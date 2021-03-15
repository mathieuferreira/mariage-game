using System;
using UnityEngine;

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
    
    public static int GetAngleFromVector(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        int angle = Mathf.RoundToInt(n);

        return angle;
    }
    
    public static int Hex_to_Dec(string hex) {
        return Convert.ToInt32(hex, 16);
    }
    
    public static float Hex_to_Dec01(string hex) {
        return Hex_to_Dec(hex)/255f;
    }
    
    public static Color GetColorFromString(string color) {
        float red = Hex_to_Dec01(color.Substring(0,2));
        float green = Hex_to_Dec01(color.Substring(2,2));
        float blue = Hex_to_Dec01(color.Substring(4,2));
        float alpha = 1f;
        if (color.Length >= 8) {
            // Color string contains alpha
            alpha = Hex_to_Dec01(color.Substring(6,2));
        }
        return new Color(red, green, blue, alpha);
    }
    
    public static Vector3 GetVectorFromAngle(int angle) {
        // angle = 0 -> 360
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}

