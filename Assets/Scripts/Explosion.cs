﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Explosion : MonoBehaviour
{
    [SerializeField] private float duration = default;
    [SerializeField] private Sprite[] sprites = default;

    private SpriteRenderer spriteRenderer;
    private float timer;
    private float timeByFrame;
    private int currentFrame;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetCurrentFrame(0);
        timer = 0f;
        timeByFrame = duration / sprites.Length;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        SetCurrentFrame(Mathf.FloorToInt(timer / timeByFrame));

        if (timer > duration)
        {
            Destroy(gameObject);
        }
    }

    private void SetCurrentFrame(int newCurrentFrame)
    {
        if (newCurrentFrame <= currentFrame || newCurrentFrame >= sprites.Length)
            return;
        
        currentFrame = newCurrentFrame;
        spriteRenderer.sprite = sprites[currentFrame];
    }
}
