using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static float CAMERA_MOVE_SPEED = 3f;
    private static float CAMERA_ZOOM_SPEED = 4f;
    private static float CAMERA_ZOOM_MIN = 7f;

    [SerializeField] private BaseRPGPlayer player1 = default;
    [SerializeField] private BaseRPGPlayer player2 = default;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraFollowPosition = FindFollowedPosition();
        Vector3 travel = cameraFollowPosition - transform.position;
        
        float deltaOrthoSize = FindOrthographicSize() - mainCamera.orthographicSize;
        float orthoSize = Math.Max(mainCamera.orthographicSize + deltaOrthoSize * CAMERA_ZOOM_SPEED * Time.deltaTime, CAMERA_ZOOM_MIN);
        
        mainCamera.orthographicSize = orthoSize;
        transform.position += travel * Math.Min(CAMERA_MOVE_SPEED * Time.deltaTime, 1f);
    }

    private float FindOrthographicSize()
    {
        Vector3 distanceBeetweenPlayers = player2.GetPosition() - player1.GetPosition();
        float minSizeX = Math.Abs(distanceBeetweenPlayers.x);
        float minSizeY = Math.Abs(distanceBeetweenPlayers.y);

        float targetOrthoSize;
        if (minSizeX / minSizeY > mainCamera.aspect)
        {
            targetOrthoSize = minSizeX / mainCamera.aspect;
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
        mainCamera.orthographicSize = FindOrthographicSize();
        transform.position = FindFollowedPosition();
    }
}
