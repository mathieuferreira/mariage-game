using UnityEngine;

public class BaseRPGPlayer : MonoBehaviour
{
    protected const float MOVE_SPEED = 4f;
    
    [SerializeField] private PlayerID player = default;

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

        UserInput.Direction direction = UserInput.FindBestDirection(player);

        switch (direction)
        {
            case UserInput.Direction.Up:
                Move(Vector3.up);
                break;
            case UserInput.Direction.Down:
                Move(Vector3.down);
                break;
            case UserInput.Direction.Left:
                Move(Vector3.left);
                break;
            case UserInput.Direction.Right:
                Move(Vector3.right);
                break;
            default:
                Idle();
                break;
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

    public PlayerID GetPlayerId()
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
