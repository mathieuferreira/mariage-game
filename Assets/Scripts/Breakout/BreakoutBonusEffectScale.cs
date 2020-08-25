using System.Collections.Generic;
using UnityEngine;

namespace Breakout
{
    [RequireComponent(typeof(BreakoutPlayer))]
    public class BreakoutBonusEffectScale : MonoBehaviour
    {
        [SerializeField] private const float Duration = 10f;
        [SerializeField] private const float Scale = .3f;
        [SerializeField] private const float MaxScale = 3f;

        private List<float> durationTimers;
        private Vector3 initialScale;

        // Start is called before the first frame update
        private void Awake()
        {
            durationTimers = new List<float>();
            initialScale = transform.localScale;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (durationTimers.Count == 0)
                return;
            
            bool changed = false;
            for (int i = 0; i < durationTimers.Count; i++)
            {
                float timer = durationTimers[i];
                timer -= Time.deltaTime;

                if (timer < 0f)
                {
                    durationTimers.RemoveAt(i);
                    i--;
                    changed = true;
                }
                else
                {
                    durationTimers[i] = timer;
                }
            }
            
            if (changed)
                UpdateScale();
        }

        private void UpdateScale()
        {
            transform.localScale = Vector3.Scale(
                initialScale,
                new Vector3(Mathf.Clamp(1f + Scale * durationTimers.Count, 1f, MaxScale), 1f, 1f)
            );
        }

        public void StartScale()
        {
            durationTimers.Add(Duration);
            UpdateScale();
        }
    }
}
