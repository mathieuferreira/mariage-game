using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReadyButton : MonoBehaviour
{
    [SerializeField] private string readyText = "JOUEUR PRET !";

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
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetPlayerReady()
    {
        text.text = readyText;
        button.SetActive(false);
        animator.SetTrigger("PlayerEnterGame");
        playerReady = true;
    }

    public bool isPlayerReady()
    {
        return playerReady;
    }
}
