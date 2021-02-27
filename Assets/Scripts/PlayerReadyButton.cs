using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReadyButton : MonoBehaviour
{
    public event EventHandler<BeforePlayerReadyEventArgs> BeforePlayerReady;
    public event EventHandler OnPlayerReady;
    
    [SerializeField] private string readyText = "JOUEUR PRET !";
    [SerializeField] private PlayerID player = default;

    private bool playerReady;
    private Text text;
    private GameObject button;
    private Animator animator;
    private bool locked = false;

    private void Awake()
    {
        playerReady = false;
        text = GetComponent<Text>();
        button = transform.Find("Button").gameObject;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerReady || locked)
            return;

        if (UserInput.IsActionKeyDown(player))
        {
            BeforePlayerReadyEventArgs eventArgs = new BeforePlayerReadyEventArgs();
            
            BeforePlayerReady?.Invoke(this, eventArgs);
            
            SoundManager.GetInstance().Play("Confirmation");
            
            if (!eventArgs.IsCancelled())
                SetPlayerReady();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void SetPlayerReady()
    {
        text.text = readyText;
        button.SetActive(false);
        animator.SetTrigger("PlayerEnterGame");
        playerReady = true;
        
        OnPlayerReady?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReady()
    {
        return playerReady;
    }
    
    public class BeforePlayerReadyEventArgs : EventArgs
    {
        private bool cancel;

        public BeforePlayerReadyEventArgs()
        {
            cancel = false;
        }

        public bool IsCancelled()
        {
            return cancel;
        }

        public void Cancel()
        {
            cancel = true;
        }
    }

    public PlayerID GetPlayerId()
    {
        return player;
    }

    public void Lock()
    {
        this.locked = true;
    }

    public void Unlock()
    {
        this.locked = false;
    }
}
