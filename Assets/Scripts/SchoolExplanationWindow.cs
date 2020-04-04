using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolExplanationWindow : MonoBehaviour
{
    public event EventHandler onClosed;
    private PlayerReadyButton[] playersReady;
    private Animator anim;

    private void Awake()
    {
        playersReady = new PlayerReadyButton[2];
        playersReady[0] = transform.Find("Player1ReadyButton").GetComponent<PlayerReadyButton>();
        playersReady[1] = transform.Find("Player2ReadyButton").GetComponent<PlayerReadyButton>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        for (int i = 0; i < playersReady.Length; i++)
        {
            if (UserInput.isKeyDown(i, UserInput.Key.Action))
            {
                playersReady[i].SetPlayerReady();
                if (areAllPlayersReady())
                {
                    anim.SetTrigger("Close");
                }
            }
        }
    }

    private bool areAllPlayersReady()
    {
        for (int i = 0; i < playersReady.Length; i++)
        {
            if (!playersReady[i].isPlayerReady())
                return false;
        }

        return true;
    }

    private void Closed()
    {
        if (onClosed != null)
        {
            onClosed(this, EventArgs.Empty);
        }

        gameObject.GetComponent<RectTransform>().position = Vector3.down * 1000;
        Destroy(gameObject, 3);
    }
}
