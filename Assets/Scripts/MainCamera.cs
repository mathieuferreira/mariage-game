using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        float height = camera.orthographicSize;

        float expectedRatio = 16f / 9;
        float expectedWidth = height * expectedRatio;
        
        float currentRatio = Screen.width * 1f / Screen.height;
        camera.orthographicSize = expectedWidth / currentRatio;
    }
}
