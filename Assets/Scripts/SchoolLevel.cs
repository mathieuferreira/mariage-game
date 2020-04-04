using System;
using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.Util;
using UnityEngine;
using Random = UnityEngine.Random;

public class SchoolLevel : MonoBehaviour
{
    [SerializeField] private SchoolExplanationWindow explanationWindow;
    [SerializeField] private SchoolPlayer[] players;
    [SerializeField] private SchoolShuriken shuriken;
    
    private bool gameStarted;
    private List<SchoolEnemy> zombies;
    private List<SchoolShuriken> shurikens;

    private void Awake()
    {
        gameStarted = false;
        explanationWindow.onClosed += OnExplanationClosed;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].OnShurikenLaunch += OnOnShurikenLaunch;
        }
        shurikens = new List<SchoolShuriken>();
    }

    private void OnOnShurikenLaunch(object sender, ShurikenLaunchEventArgs e)
    {
        SchoolShuriken newShuriken = Instantiate(shuriken, e.getPosition(), Quaternion.identity);
        newShuriken.OnDestroyed += OnShurikenDestroyed;
        newShuriken.StartMoving();
        shurikens.Add(newShuriken);
    }

    private void OnShurikenDestroyed(object sender, EventArgs args)
    {
        bool remainsShurikens = false;
        for (int i = 0; i < shurikens.Count; i++)
        {
            if (!shurikens[i].IsDestroyed())
            {
                remainsShurikens = true;
            }
        }
        
        if (!remainsShurikens)
            InitShuriken();
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

        InitShuriken();
        gameStarted = true;
    }

    private void InitShuriken()
    {
        List<SchoolPlayer> listPlayers = new List<SchoolPlayer>();
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].HasShuriken())
            {
                listPlayers.Add(players[i]);
            }
        }
        
        if (listPlayers.Count == 0)
            return;

        listPlayers[Random.Range(0, listPlayers.Count)].StartShuriken();
    }
}
