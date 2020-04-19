using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRPGPlayer : MonoBehaviour
{
    protected const float MOVE_SPEED = 4f;
    
    [SerializeField] private UserInput.Player player;

    private Animator animator;
    private bool active;
    private GameObject adviceButton;

    protected virtual void Awake()
    {
        active = true;
        animator = GetComponent<Animator>();
        animator.SetFloat("speed", 0f);
        animator.SetFloat("vertical", 0f);
        animator.SetFloat("horizontal", -1f);
        adviceButton = transform.Find("AdviceButton").gameObject;
    }

    protected void FixedUpdate()
    {
        if (!active)
            return;
        
        if (UserInput.isKey(player, UserInput.Key.Up))
        {
            Move(Vector3.up);
        } 
        else if (UserInput.isKey(player, UserInput.Key.Down))
        {
            Move(Vector3.down);
        }
        else if (UserInput.isKey(player, UserInput.Key.Left))
        {
            Move(Vector3.left);
        }
        else if (UserInput.isKey(player, UserInput.Key.Right))
        {
            Move(Vector3.right);
        }
        else
        {
            Idle();
        }
    }

    protected virtual void Move(Vector3 direction)
    {
        transform.position = transform.position + direction * MOVE_SPEED * Time.deltaTime;
        animator.SetFloat("speed", MOVE_SPEED);
        animator.SetFloat("vertical", direction.x);
        animator.SetFloat("horizontal", direction.y);
    }

    private void Idle()
    {
        animator.SetFloat("speed", 0f);
    }

    public UserInput.Player GetPlayerId()
    {
        return player;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
        LockMove();
    }

    public void LockMove()
    {
        active = false;
    }

    public void UnlockMove()
    {
        active = true;
    }

    public void ShowAdviceButton()
    {
        adviceButton.SetActive(true);
    }

    public void HideAdviceButton()
    {
        adviceButton.SetActive(false);
    }
}
