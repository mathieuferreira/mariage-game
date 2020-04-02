using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchoolLevel : MonoBehaviour
{
    private bool gameStarted;
    private SchoolPlayer[] players;
    private List<SchoolEnemy> zombies;
    
    private void Awake()
    {
        gameStarted = false;
        SchoolExplanationWindow.GetInstance().onClosed += OnExplanationClosed;
        players = new SchoolPlayer[2];
        players[0] = transform.Find("Player1").GetComponent<SchoolPlayer>();
        players[1] = transform.Find("Player2").GetComponent<SchoolPlayer>();
    }

    private void Update()
    {
        if (!gameStarted)
            return;
    }

    private void OnExplanationClosed(object sender, EventArgs e)
    {
        CountDown.GetInstance().StartCounter(3, () =>
        {
            StartGame();
            Debug.Log("Game start !");
        });
    }

    private void StartGame()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].UnlockMove();
        }

        gameStarted = true;
    }
}
