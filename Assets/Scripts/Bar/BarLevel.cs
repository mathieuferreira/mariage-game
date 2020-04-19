using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarLevel : MonoBehaviour
{
    [SerializeField] private BarPlayer[] players;
    [SerializeField] private BarGuest[] guests;
    [SerializeField] private BarConsumableWorker[] workers;
    [SerializeField] private BarConsumableCounter[] counters;
    
    private ModalLevel modalLevel;

    private void Awake()
    {
        modalLevel = GetComponent<ModalLevel>();
        modalLevel.gameStart += StartGame;
    }

    private void StartGame(object sender, EventArgs e)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].UnlockMove();
        }
        
        for (int i = 0; i < workers.Length; i++)
        {
            workers[i].StartWork();
        }
        
        for (int i = 0; i < counters.Length; i++)
        {
            counters[i].Show();
        }
    }
}
