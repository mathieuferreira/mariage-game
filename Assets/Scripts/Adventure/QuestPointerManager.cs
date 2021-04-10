using UnityEngine;

namespace Adventure
{
    public class QuestPointerManager : MonoBehaviour
    {
        private static float BORDER_SIZE = 60f;
    
        [SerializeField] private Camera uiCamera = default;
        [SerializeField] private Camera mainCamera = default;
        [SerializeField] private QuestPointer arrowPointer = default;
        [SerializeField] private QuestPointer finalPointer = default;

        private Vector3 targetPosition;
        private QuestPointer currentPointer;

        private void Awake()
        {
            gameObject.SetActive(false);
            arrowPointer.Hide();
            finalPointer.Hide();
        }

        private void FixedUpdate()
        {
            Vector3 targetPositionScreenPoint = mainCamera.WorldToScreenPoint(targetPosition);
            bool isOffScreen = targetPositionScreenPoint.x <= BORDER_SIZE || targetPositionScreenPoint.x >= Screen.width - BORDER_SIZE ||
                               targetPositionScreenPoint.y <= BORDER_SIZE || targetPositionScreenPoint.y >= Screen.height - BORDER_SIZE;

            QuestPointer pointer;
            if (isOffScreen)
            {
                pointer = arrowPointer;
                
                if (targetPositionScreenPoint.x <= BORDER_SIZE) targetPositionScreenPoint.x = BORDER_SIZE;
                if (targetPositionScreenPoint.x >= Screen.width - BORDER_SIZE) targetPositionScreenPoint.x = Screen.width - BORDER_SIZE;
                if (targetPositionScreenPoint.y <= BORDER_SIZE) targetPositionScreenPoint.y = BORDER_SIZE;
                if (targetPositionScreenPoint.y >= Screen.height - BORDER_SIZE) targetPositionScreenPoint.y = Screen.height - BORDER_SIZE;
            }
            else
            {
                pointer = finalPointer;
            }

            if (pointer != currentPointer)
            {
                arrowPointer.Hide();
                finalPointer.Hide();
                pointer.Show();
            }

            currentPointer = pointer;
            RotatePointer();
            SetPointerPosition(targetPositionScreenPoint);
        }

        private void RotatePointer()
        {
            Vector3 fromPosition = mainCamera.transform.position;
            fromPosition.z = 0f;
            Vector3 direction = (targetPosition - fromPosition).normalized;
            currentPointer.Rotate(Utils.GetAngleFromVector(direction));
        }

        private void SetPointerPosition(Vector3 position)
        {
            Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(position);
            //pointerWorldPosition.z = 0f;
            currentPointer.SetPosition(pointerWorldPosition);
        }

        public void Show(Vector3 target)
        {
            targetPosition = target;
            Show();
        }

        public void Show(QuestPosition quest)
        {
            targetPosition = quest.GetPosition();
            Show();
        }

        public void Show()
        {
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
}
