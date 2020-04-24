using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReadyButton : MonoBehaviour
{
    public event EventHandler<BeforePlayerReadyEventArgs> BeforePlayerReady;
    public event EventHandler OnPlayerReady;
    
    [SerializeField] private string readyText = "JOUEUR PRET !";
    [SerializeField] private UserInput.Player player;

    private bool playerReady;
    private Text text;
    private GameObject button;
    private Animator animator;

    private void Awake()
    {
        playerReady = false;
        text = GetComponent<Text>();
        button = transform.Find("Button").gameObject;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerReady)
            return;

        if (UserInput.isKeyDown(player, UserInput.Key.Action))
        {
            BeforePlayerReadyEventArgs eventArgs = new BeforePlayerReadyEventArgs();
            
            if (BeforePlayerReady != null)
                BeforePlayerReady(this, eventArgs);
            
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
        
        if (OnPlayerReady != null)
            OnPlayerReady(this, EventArgs.Empty);
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

    public UserInput.Player GetPlayerId()
    {
        return player;
    }
}
