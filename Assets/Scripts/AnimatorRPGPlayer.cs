using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorRPGPlayer : MonoBehaviour
{
    [SerializeField] private Sprite[] idleUpSprites;
    [SerializeField] private Sprite[] idleDownSprites;
    [SerializeField] private Sprite[] idleRightSprites;
    [SerializeField] private Sprite[] idleLeftSprites;
    [SerializeField] private Sprite[] walkUpSprites;
    [SerializeField] private Sprite[] walkDownSprites;
    [SerializeField] private Sprite[] walkLeftSprites;
    [SerializeField] private Sprite[] walkRightSprites;
    [SerializeField] private int frameSample;

    private enum Direction
    {
        Top,
        Down,
        Left,
        Right
    }
    private enum State
    {
        Idle,
        Walk
    }
    
    private Sprite[] activeSprites;
    private int currentFrame;
    private SpriteRenderer spriteRenderer;
    private Vector2 direction;
    private float speed;
    private int frameCount;
    private Direction currentDirection;
    private State currentState;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = Vector3.down;
        speed = 0f;
        UpdateSpritesToRender();
    }

    private void Update()
    {
        int activeFrameCount = activeSprites.Length;
        
        if (activeFrameCount <= 1)
            return;

        frameCount++;

        if (frameCount >= frameSample)
        {
            currentFrame = (currentFrame + 1) % activeFrameCount;
            UpdateSprite();
            frameCount = 0;
        }
    }

    private void SetActiveSprites(Sprite[] sprites)
    {
        activeSprites = sprites;
        currentFrame = 0;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        spriteRenderer.sprite = activeSprites[currentFrame];
    }

    public void SetSpeed(float s)
    {
        speed = s;
        UpdateSpritesToRender();
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
        UpdateSpritesToRender();
    }

    public void SetSpeedAndDirection(float s, Vector3 dir)
    {
        speed = s;
        direction = dir;
        UpdateSpritesToRender();
    }

    private void UpdateSpritesToRender()
    {
        Direction dir = GuessBestDirection();
        State state = GuessBestState();

        if (currentDirection == dir && currentState == state)
            return;

        currentDirection = dir;
        currentState = state;

        Sprite[] sprites;
        if (state == State.Idle)
        {
            switch (dir)
            {
                case Direction.Left:
                    sprites = idleLeftSprites;
                    break;
                case Direction.Right:
                    sprites = idleRightSprites;
                    break;
                case Direction.Top:
                    sprites = idleUpSprites;
                    break;
                default:
                    sprites = idleDownSprites;
                    break;
            }
        }
        else
        {
            switch (dir)
            {
                case Direction.Left:
                    sprites = walkLeftSprites;
                    break;
                case Direction.Right:
                    sprites = walkRightSprites;
                    break;
                case Direction.Top:
                    sprites = walkUpSprites;
                    break;
                default:
                    sprites = walkDownSprites;
                    break;
            }
        }
        
        SetActiveSprites(sprites);
    }

    private Direction GuessBestDirection()
    {
        Direction[] directions = new Direction[]
        {
            Direction.Top,
            Direction.Down,
            Direction.Left,
            Direction.Right
        };

        int position = 0;
        float distance = -1f;

        for (int i = 0; i < 4; i++)
        {
            float magnitude = (DirectionToVector2(directions[i]) - direction).sqrMagnitude;
            if (distance < 0 || magnitude < distance)
            {
                position = i;
                distance = magnitude;
            }
        }
        
        return directions[position];
    }

    private State GuessBestState()
    {
        return Math.Abs(speed) > 0.1 ? State.Walk : State.Idle;
    }

    private Vector2 DirectionToVector2(Direction dir)
    {
        switch (dir)
        {
            case Direction.Down:
                return Vector2.down;
            case Direction.Top:
                return Vector2.up;
            case Direction.Left:
                return Vector2.left;
            default:
                return Vector2.right;
        }
    }
}
