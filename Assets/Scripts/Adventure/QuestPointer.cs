using UnityEngine;

namespace Adventure
{
    public class QuestPointer : MonoBehaviour
    {
        private RectTransform rectTransform;

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void Rotate(int rotation)
        {
            
        }

        public void SetPosition(Vector3 position)
        {
            rectTransform.position = position;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
