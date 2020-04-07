using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class SchoolEnemy : MonoBehaviour
{
    public event EventHandler OnDied;
    public event EventHandler OnDisappear;
    
    private static int MAX_POSITION_RETRY = 20;
    private static float POSITION_X_MAX = 7f;
    private static float POSITION_X_MIN = -7f;
    private static float POSITION_Y_MAX = 7f;
    private static float POSITION_Y_MIN = -7f;
    private static float MOVE_SPEED = 1.5f;
    private static float ROTATION_SPEED = 2f;
    
    private Vector3 nextPosition;
    private int nextAngle;
    private Vector3 currentDirection;
    private bool alive;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        alive = true;
        FindNextPosition();
    }

    private void Update()
    {
        if (!alive)
            return;
        
        float currentAngle = Utils.ConvertAngle360(transform.eulerAngles.z);
        float deltaAngle = Utils.ConvertAngle180(nextAngle - currentAngle);
        if (Math.Abs(deltaAngle) > 1f)
        {
            float frameDeltaAngle = (deltaAngle / Math.Abs(deltaAngle)) * 360 * ROTATION_SPEED * Time.deltaTime;
            if (Math.Abs(frameDeltaAngle) > Math.Abs(deltaAngle))
                frameDeltaAngle = deltaAngle;
            
            transform.eulerAngles += new Vector3(0, 0, frameDeltaAngle);
            return;
        }
        
        transform.position += currentDirection * MOVE_SPEED * Time.deltaTime;

        Vector3 heading = nextPosition - transform.position;

        if (heading.sqrMagnitude < .2f * .2f)
        {
            FindNextPosition();
        }
    }

    private void FindNextPosition()
    {
        Vector3 candidatePosition = Vector3.right;
        Vector3 currentPosition = transform.position;
        Vector3 heading = Vector3.right;
        for (int i = 0; i < MAX_POSITION_RETRY; i++)
        {
            candidatePosition = new Vector3(Random.Range(POSITION_X_MIN, POSITION_X_MAX), Random.Range(POSITION_Y_MIN, POSITION_Y_MAX), 0);
            heading = candidatePosition - currentPosition;
            
            if (heading.sqrMagnitude > 4f)
            {
                break;
            }
        }

        nextPosition = candidatePosition;
        currentDirection = Vector3.Normalize(heading);
        nextAngle = UtilsClass.GetAngleFromVector(heading);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("EnemyDead");
        if (other.gameObject.CompareTag("Projectile"))
        {
            alive = false;
            if (OnDied != null)
                OnDied(this, EventArgs.Empty);
            animator.SetTrigger("Dead");
        }
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
}
