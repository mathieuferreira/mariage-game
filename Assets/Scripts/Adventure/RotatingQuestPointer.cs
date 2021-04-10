using UnityEngine;

namespace Adventure
{
    public class RotatingQuestPointer : QuestPointer
    {
        [SerializeField] private Transform arrow = default;
        [SerializeField] private Transform icon = default;

        private RectTransform arrowRectTransform;
        private RectTransform iconRectTransform;

        protected override void Awake()
        {
            arrowRectTransform = arrow.GetComponent<RectTransform>();
            iconRectTransform = icon.GetComponent<RectTransform>();
            base.Awake();
        }

        public override void Rotate(int rotation)
        {
            arrowRectTransform.localEulerAngles = new Vector3(0f, 0f, rotation);
            Vector3 position = Utils.GetVectorFromAngle(rotation);
            iconRectTransform.localPosition = position * -50;
            base.Rotate(rotation);
        }
    }
}
