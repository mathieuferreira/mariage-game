using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class SchoolEnemy : MonoBehaviour
{
    public event EventHandler OnDisappear;
    public event EventHandler OnHitPlayer;

    private static int MAX_POSITION_RETRY = 20;
    private static float POSITION_X_MAX = 7f;
    private static float POSITION_X_MIN = -7f;
    private static float POSITION_Y_MAX = 7f;
    private static float POSITION_Y_MIN = -7f;
    private static float MOVE_SPEED = 1f;
    private static float ROTATION_SPEED = 2f;
    
    private Vector3 nextPosition;
    private int nextAngle;
    private Vector3 currentDirection;
    private Animator animator;
    private HealthSystem healthSystem;
    private SchoolPlayer player;
    private State currentState;
    private int randomMove;

    private enum State
    {
        MoveToRandom,
        MoveToPlayer
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        randomMove = 1;
        FindNextPosition();
        healthSystem = new HealthSystem(GetMaxHealth());
        healthSystem.OnDied += HealthSystemOnDied;
        currentState = State.MoveToRandom;
    }

    private void HealthSystemOnDied(object sender, EventArgs e)
    {
        animator.SetTrigger("Dead");
    }

    protected virtual int GetMaxHealth()
    {
        return 75;
    }

    private void Update()
    {
        if (!healthSystem.IsAlive())
            return;

        TargetInfo target = GetTargetPosition();
        
        float currentAngle = Utils.ConvertAngle360(transform.eulerAngles.z);
        float deltaAngle = Utils.ConvertAngle180(target.angle - currentAngle);
        if (Math.Abs(deltaAngle) > 1f)
        {
            float frameDeltaAngle = (deltaAngle / Math.Abs(deltaAngle)) * 360 * ROTATION_SPEED * Time.deltaTime;
            if (Math.Abs(frameDeltaAngle) > Math.Abs(deltaAngle))
                frameDeltaAngle = deltaAngle;
            
            transform.eulerAngles += new Vector3(0, 0, frameDeltaAngle);
        }
        
        transform.position += target.direction * MOVE_SPEED * Time.deltaTime;

        Vector3 heading = target.position - transform.position;
        float range = currentState == State.MoveToPlayer ? 1f : .2f;

        if (heading.sqrMagnitude < range * range)
        {
            GoToNextState();
        }
    }

    public void Setup(SchoolPlayer player)
    {
        this.player = player;
    }

    private TargetInfo GetTargetPosition()
    {
        if (currentState == State.MoveToPlayer)
        {
            Vector3 targetPosition = player.GetPosition();
            Vector3 heading = targetPosition - transform.position;

            return new TargetInfo
            {
                angle = UtilsClass.GetAngleFromVector(heading),
                direction = Vector3.Normalize(heading),
                position = targetPosition
            };
        }
        
        return new TargetInfo
        {
            angle = nextAngle,
            direction = currentDirection,
            position = nextPosition
        };
    }

    protected virtual int GetMaxRandomMove()
    {
        return 5;
    }

    protected void GoToNextState()
    {
        switch (currentState)
        {
            case State.MoveToRandom:
                FindNextPosition();
                randomMove++;

                if (GetMaxRandomMove() > 0 && randomMove > GetMaxRandomMove())
                {
                    currentState = State.MoveToPlayer;
                }
                
                break;
            case State.MoveToPlayer:
                healthSystem.Damage(GetMaxHealth());
                if (OnHitPlayer != null)
                    OnHitPlayer(this, EventArgs.Empty);
                break;
        }
    }

    protected void FindNextPosition()
    {
        Vector3 candidatePosition = Vector3.right;
        Vector3 currentPosition = transform.position;
        Vector3 heading = Vector3.right;
        for (int i = 0; i < MAX_POSITION_RETRY; i++)
        {
            candidatePosition = new Vector3(Random.Range(POSITION_X_MIN, POSITION_X_MAX), Random.Range(POSITION_Y_MIN, POSITION_Y_MAX), 0);
            heading = candidatePosition - currentPosition;
            
            if (heading.sqrMagnitude > 16f)
            {
                break;
            }
        }

        nextPosition = candidatePosition;
        currentDirection = Vector3.Normalize(heading);
        nextAngle = UtilsClass.GetAngleFromVector(heading);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
        if (OnDisappear != null)
            OnDisappear(this, EventArgs.Empty);
    }

    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

    private class TargetInfo
    {
        public Vector3 position;
        public Vector3 direction;
        public float angle;
    }
}
