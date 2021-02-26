using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static float CAMERA_MOVE_SPEED = 3f;
    private static float CAMERA_ZOOM_SPEED = 4f;
    private static float CAMERA_ZOOM_MIN = 7f;

    [SerializeField] private BaseRPGPlayer player1 = default;
    [SerializeField] private BaseRPGPlayer player2 = default;

    private Camera mainCamera;
    private float ratio;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        ratio = Screen.width * 1f / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraFollowPosition = FindFollowedPosition();
        Vector3 currentPosition = transform.position;
        Vector3 travel = cameraFollowPosition - currentPosition;
        float orthographicSize = mainCamera.orthographicSize;
        
        float deltaOrthoSize = FindOrthographicSize() - orthographicSize;
        float orthoSize = Math.Max(orthographicSize + deltaOrthoSize * CAMERA_ZOOM_SPEED * Time.deltaTime, CAMERA_ZOOM_MIN);
        
        mainCamera.orthographicSize = orthoSize;
        transform.position = currentPosition + travel * Math.Min(CAMERA_MOVE_SPEED * Time.deltaTime, 1f);
    }

    private float FindOrthographicSize()
    {
        Vector3 distanceBetweenPlayers = player2.GetPosition() - player1.GetPosition();
        float minSizeX = Math.Abs(distanceBetweenPlayers.x);
        float minSizeY = Math.Abs(distanceBetweenPlayers.y);

        float targetOrthoSize;
        if (minSizeX / minSizeY > ratio)
        {
            targetOrthoSize = minSizeX / ratio;
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
