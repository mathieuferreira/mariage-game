using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Bar
{
    public class BarJukeBox : MonoBehaviour
    {
        [SerializeField] private JukeBoxMusic[] musics = default;
        [SerializeField] private Light2D ambianceLight = default;

        private JukeBoxMusic musicPlaying;
        private Color originalColor;
        private float remainingMusicTimer;

        private void Start()
        {
            originalColor = ambianceLight.color;
        }

        void Update()
        {
            if (musicPlaying == null || remainingMusicTimer <= 0f)
                return;

            remainingMusicTimer -= Time.deltaTime;

            if (remainingMusicTimer <= 0f)
            {
                SoundManager.GetInstance().StopPlaying(musicPlaying.GetLabel());
                SoundManager.GetInstance().SetVolume("Theme", 0.5f);
                ambianceLight.color = originalColor;
                remainingMusicTimer = 0f;
                musicPlaying = null;
                return;
            }

            float x = remainingMusicTimer % 2f;
            float normalizedX = Mathf.Abs((float)((x - 1) * 1.2));
            float intensity = - Mathf.Pow(normalizedX, 3) + normalizedX + 0.6f;
            ambianceLight.intensity = intensity * .6f;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            BarPlayer player = other.GetComponent<BarPlayer>();

            if (player != null)
            {
                player.ShowAdviceButton();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            BarPlayer player = other.GetComponent<BarPlayer>();

            if (player != null)
            {
                player.HideAdviceButton();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            BarPlayer player = other.GetComponent<BarPlayer>();

            if (player != null && UserInput.IsActionKeyDown(player.GetPlayerId()))
            {
                PlayMusic();
            }
        }

        private void PlayMusic()
        {
            SoundManager.GetInstance().SetVolume("Theme", 0.1f);
            
            if (musicPlaying != null)
                SoundManager.GetInstance().StopPlaying(musicPlaying.GetLabel());
            
            musicPlaying = musics[Random.Range(0, musics.Length)];
            SoundManager.GetInstance().Play(musicPlaying.GetLabel());
            ambianceLight.color = musicPlaying.GetColor();
            remainingMusicTimer = musicPlaying.GetDuration();
        }
    }

    [Serializable]
    public class JukeBoxMusic
    {
        [SerializeField] private String label;
        [SerializeField] private Color color = default;
        [SerializeField] private float duration;

        public String GetLabel()
        {
            return label;
        }

        public Color GetColor()
        {
            return color;
        }

        public float GetDuration()
        {
            return duration;
        }
    }
}
