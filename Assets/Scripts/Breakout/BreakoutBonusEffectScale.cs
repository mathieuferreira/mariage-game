using UnityEngine;

namespace Breakout
{
    [RequireComponent(typeof(BreakoutPlayer))]
    public class BreakoutBonusEffectScale : MonoBehaviour
    {
        private const float Duration = 10f;
        private const float Scale = 1.4f;

        private float durationTimer;

        // Start is called before the first frame update
        private void Start()
        {
            durationTimer = Duration;
        
            UpdateScale(Scale);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            durationTimer -= Time.deltaTime;

            if (durationTimer < 0f)
            {
                UpdateScale(1f / Scale);
                Destroy(this);
            }
        }

        private void UpdateScale(float factor)
        {
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1f, factor, 1f));
        }
    }
}
