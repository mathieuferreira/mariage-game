using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

namespace School
{
    public class SchoolThunderController : MonoBehaviour
    {
        private const float FadeOutMaxTimer = .3f;
        
        [SerializeField] private float minTimer = default;
        [SerializeField] private float maxTimer = default;
        [SerializeField] private Color color = default;
        [SerializeField] private float intensity = default;
        [SerializeField] private AnimationCurve curve = default;
        
        private Light2D globalLight;
        private Color originalColor;
        private float originalIntensity;
        private float timer;
        private float fadeOutTimer = 0f;
        private float isThunderActive;
        
        private void Awake()
        {
            globalLight = GetComponent<Light2D>();
            originalColor = globalLight.color;
            originalIntensity = globalLight.intensity;
            InitializeTimer();
        }

        private void Update()
        {
            if (fadeOutTimer <= 0f)
                return;

            fadeOutTimer -= Time.deltaTime;
            fadeOutTimer = Mathf.Max(fadeOutTimer, 0f);
            ManageLightIntensity();

            if (fadeOutTimer <= 0f)
            {
                SoundManager.GetInstance().Play("Thunder");
                Debug.Log("Play Thunder");
            }
        }

        private void FixedUpdate()
        {
            timer -= Time.fixedDeltaTime;

            if (timer < 0f)
            {
                fadeOutTimer = FadeOutMaxTimer;
                ManageLightIntensity();
                InitializeTimer();
            }
        }

        private void InitializeTimer()
        {
            timer = Random.Range(minTimer, maxTimer);
        }

        private void ManageLightIntensity()
        {
            if (fadeOutTimer <= 0f)
                return;

            float current = (FadeOutMaxTimer - fadeOutTimer) / FadeOutMaxTimer;
            float normalizedPosition = curve.Evaluate(current);
            globalLight.intensity = originalIntensity + normalizedPosition * (intensity - originalIntensity);
            Color flashColor = globalLight.color;
            flashColor.r = originalColor.r + normalizedPosition * (color.r - originalColor.r);
            flashColor.g = originalColor.g + normalizedPosition * (color.g - originalColor.g);
            flashColor.b = originalColor.b + normalizedPosition * (color.b - originalColor.b);
            globalLight.color = flashColor;
        }
    }
}
