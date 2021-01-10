using System;
using JetBrains.Annotations;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const float DestroyTimerMax = 1f;
    
    [SerializeField] private Sound[] sounds = default;

    private static SoundManager _instance;

    private float destroyTimer;

    public static SoundManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        foreach(Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        if (_instance != null)
        {
            _instance.SoftDestroy();
        }

        _instance = this;
    }

    private void Start()
    {
        Play("Theme");
    }

    private void FixedUpdate()
    {
        if (destroyTimer <= 0f)
            return;

        destroyTimer -= Time.fixedDeltaTime;

        if (destroyTimer <= 0f)
        {
            Destroy(gameObject);
            return;
        }
        
        float volumeMax = destroyTimer / DestroyTimerMax;
        
        foreach(Sound sound in sounds)
        {
            sound.source.volume = Mathf.Min(sound.source.volume, volumeMax);
        }
    }

    private void SoftDestroy()
    {
        destroyTimer = DestroyTimerMax;
    }

    public void Play(string soundName)
    {
        Sound sound = Array.Find(sounds, s => s.name == soundName);

        if (sound == null)
        {
            Debug.LogWarning("Sound " + soundName + " not found!");
            return;
        }
        
        sound.source.Play();
    }
}
