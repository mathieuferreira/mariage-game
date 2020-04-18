using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static float CAMERA_MOVE_SPEED = 3f;
    private static float CAMERA_ZOOM_SPEED = 4f;
    private static float CAMERA_ZOOM_MIN = 7f;

    [SerializeField] private BaseRPGPlayer player1;
    [SerializeField] private BaseRPGPlayer player2;

    private Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraFollowPosition = FindFollowedPosition();
        Vector3 travel = cameraFollowPosition - transform.position;
        
        float deltaOrthoSize = FindOrthographicSize() - camera.orthographicSize;
        float orthoSize = Math.Max(camera.orthographicSize + deltaOrthoSize * CAMERA_ZOOM_SPEED * Time.deltaTime, CAMERA_ZOOM_MIN);
        
        camera.orthographicSize = orthoSize;
        transform.position += travel * Math.Min(CAMERA_MOVE_SPEED * Time.deltaTime, 1f);
    }

    private float FindOrthographicSize()
    {
        Vector3 distanceBeetweenPlayers = player2.GetPosition() - player1.GetPosition();
        float minSizeX = Math.Abs(distanceBeetweenPlayers.x);
        float minSizeY = Math.Abs(distanceBeetweenPlayers.y);

        float targetOrthoSize;
        if (minSizeX / minSizeY > camera.aspect)
        {
            targetOrthoSize = minSizeX / camera.aspect;
        }
        else
        {
            targetOrthoSize = minSizeY;
        }

        targetOrthoSize += 5f;

        return targetOrthoSize / 2f;
    }

    private Vector3 FindFollowedPosition()
    {
        Vector3 cameraFollowPosition = player1.GetPosition() + (player2.GetPosition() - player1.GetPosition()) / 2 ;
        cameraFollowPosition.z = transform.position.z;
        return cameraFollowPosition;
    }

    public void InitPosition()
    {
        camera.orthographicSize = FindOrthographicSize();
        transform.position = FindFollowedPosition();
    }
}
