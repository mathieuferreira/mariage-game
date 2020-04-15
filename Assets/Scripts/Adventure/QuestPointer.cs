using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using CodeMonkey.Utils;

public class QuestPointer : MonoBehaviour
{
    private static float BORDER_SIZE = 60f;
    
    [SerializeField] private Camera uiCamera;
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;

    private Vector3 targetPosition;
    private RectTransform pointerRectTransform;
    private SpriteRenderer pointerSpriteRenderer;

    private void Awake()
    {
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        pointerSpriteRenderer = transform.Find("Pointer").GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= BORDER_SIZE || targetPositionScreenPoint.x >= Screen.width - BORDER_SIZE ||
                           targetPositionScreenPoint.y <= BORDER_SIZE || targetPositionScreenPoint.y >= Screen.height - BORDER_SIZE;

        if (isOffScreen)
        {
            pointerSpriteRenderer.sprite = arrowSprite;
            
            RotatePointer();
            
            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= BORDER_SIZE) cappedTargetScreenPosition.x = BORDER_SIZE;
            if (cappedTargetScreenPosition.x >= Screen.width - BORDER_SIZE) cappedTargetScreenPosition.x = Screen.width - BORDER_SIZE;
            if (cappedTargetScreenPosition.y <= BORDER_SIZE) cappedTargetScreenPosition.y = BORDER_SIZE;
            if (cappedTargetScreenPosition.y >= Screen.height - BORDER_SIZE) cappedTargetScreenPosition.y = Screen.height - BORDER_SIZE;

            SetPointerPosition(cappedTargetScreenPosition);
        }
        else
        {
            pointerSpriteRenderer.sprite = crossSprite;
            pointerRectTransform.localEulerAngles = new Vector3(0f, 0f, 0f);
            SetPointerPosition(targetPositionScreenPoint);
        }
    }

    private void RotatePointer()
    {
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 direction = (targetPosition - fromPosition).normalized;
        pointerRectTransform.localEulerAngles = new Vector3(0f, 0f, UtilsClass.GetAngleFromVector(direction));
    }

    private void SetPointerPosition(Vector3 position)
    {
        Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(position);
        pointerRectTransform.position = pointerWorldPosition;
        pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
    }

    public void Show(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        gameObject.SetActive(true);
    }

    public void Show(QuestPosition quest)
    {
        targetPosition = quest.GetPosition();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public Vector3 GetTarget()
    {
        return targetPosition;
    }
}
