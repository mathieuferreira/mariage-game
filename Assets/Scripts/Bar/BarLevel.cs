using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BarLevel : MonoBehaviour
{
    private const float GameTimerMax = 90f;
    //private const float GameTimerMax = 10f;
    
    private const float TalkTimerMax = 3f;
    private const float BeerTimerMax = 5f;
    private const float CakeTimerMax = 8f;
    
    [SerializeField] private BarPlayer[] players;
    [SerializeField] private BarGuest[] guests;
    [SerializeField] private BarConsumableWorker[] workers;
    [SerializeField] private BarConsumableCounter[] counters;
    [SerializeField] private Avatar[] avatars;
    [SerializeField] private FunIndicator funIndicator;
    [SerializeField] private Text timerText;
    
    private ModalLevel modalLevel;
    private float talkTimer;
    private float beerTimer;
    private float cakeTimer;
    private bool started;
    private float gameTimer;

    private void Awake()
    {
        modalLevel = GetComponent<ModalLevel>();
        modalLevel.gameStart += StartGame;
        funIndicator.Setup(guests);
        funIndicator.OnFunExpired += FunIndicatorOnOnFunExpired;
        started = false;
        gameTimer = GameTimerMax;
        timerText.gameObject.SetActive(false);
    }

    private void FunIndicatorOnOnFunExpired(object sender, EventArgs e)
    {
        StopUI();

        modalLevel.OpenLooseWindow(() =>
            {
                ScoreManager.RevertSession();
                Loader.Load(Loader.Scene.Bar);
            }
        );
    }

    private void FixedUpdate()
    {
        if (!started)
            return;

        gameTimer -= Time.fixedDeltaTime;

        if (gameTimer < 0f)
        {
            StopUI();
            ScoreManager.CloseSession();
            modalLevel.OpenWinWindow(() =>
            {
                AdventureLevel.SetStage(AdventureLevel.Stage.Home);
                Loader.Load(Loader.Scene.Adventure);
            });
            return;
        }

        timerText.text = String.Format("{0:00}", Math.Floor(gameTimer / 60f)) + ":" + String.Format("{0:00}", Math.Ceiling(gameTimer % 60f));
        
        talkTimer -= Time.fixedDeltaTime;
        beerTimer -= Time.fixedDeltaTime;
        cakeTimer -= Time.fixedDeltaTime;

        if (talkTimer < 0f)
        {
            guests[Random.Range(0, guests.Length)].GetNeeds().TryAdd(new BarConsumable(BarConsumable.Type.Talk));
            talkTimer += TalkTimerMax;
        }

        if (beerTimer < 0f)
        {
            guests[Random.Range(0, guests.Length)].GetNeeds().TryAdd(new BarConsumable(BarConsumable.Type.Beer));
            beerTimer += BeerTimerMax;
        }

        if (cakeTimer < 0f)
        {
            guests[Random.Range(0, guests.Length)].GetNeeds().TryAdd(new BarConsumable(BarConsumable.Type.Cake));
            cakeTimer += CakeTimerMax;
        }
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
        
        for (int i = 0; i < guests.Length; i++)
        {
            guests[i].Activate();
        }
        
        for (int i = 0; i < avatars.Length; i++)
        {
            avatars[i].Show();
        }
        
        funIndicator.SetActive(true);
        timerText.gameObject.SetActive(true);
        started = true;
        talkTimer = TalkTimerMax;
        beerTimer = BeerTimerMax;
        cakeTimer = CakeTimerMax;
    }

    private void StopUI()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].LockMove();
        }
        
        for (int i = 0; i < workers.Length; i++)
        {
            workers[i].StopWork();
        }
        
        for (int i = 0; i < counters.Length; i++)
        {
            counters[i].Hide();
        }
        
        for (int i = 0; i < guests.Length; i++)
        {
            guests[i].Deactivate();
        }
        
        for (int i = 0; i < avatars.Length; i++)
        {
            avatars[i].Hide();
        }
        
        funIndicator.SetActive(false);
        timerText.gameObject.SetActive(false);
        started = false;
    }
}
