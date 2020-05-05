using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HomeLevel : MonoBehaviour
{
    public const float MaxYPosition = 12f;
    public const float MinYPosition = -12f;
    private const float EnemyXSpawn = 30f;
    private const float EnemyXDisappear = -30f;
    private const float SpiderMinTimer = 1f;
    private const float SpiderMaxTimer = 6f;
    private const float BossMaxTimer = 90f;
    private const float Speed = 5f;
    
    [SerializeField] private HomePlayer[] players;
    [SerializeField] private Transform spider;
    [SerializeField] private HeartSystemUI heartSystemUI;
    [SerializeField] private Avatar[] avatars;

    private enum State
    {
        WaitingToStart,
        Stage1,
        WaitingForBoss,
        Boss,
        Win
    }
    
    private HomeEnvironment environment;
    private List<HomeEnemy> spiders;
    private float spiderTimer;
    private float bossTimer;
    private State state;
    private ModalLevel modalLevel;
    private HeartSystem heartSystem;

    private void Awake()
    {
        spiders = new List<HomeEnemy>();
        environment = GetComponent<HomeEnvironment>();
        bossTimer = BossMaxTimer;
        state = State.Stage1;
        heartSystem = new HeartSystem(3);
        heartSystem.OnDied += OnHeartSystemDied;
        heartSystemUI.Setup(heartSystem);
        
        //modalLevel = GetComponent<ModalLevel>();
        //modalLevel.gameStart += StartGame;

        for (int i = 0; i < players.Length; i++)
        {
            players[i].LockMove();
            players[i].OnDamaged += OnPlayerDamaged;
        }
        
        for (int i = 0; i < avatars.Length; i++)
        {
            avatars[i].Hide();
        }
    }

    private void Start()
    {
        StartGame(null, null);
    }
    
    private void StartGame(object sender, EventArgs e)
    {
        environment.SetSpeed(Speed);
        SpawnSpider();
        spiderTimer = SpiderMaxTimer;
        bossTimer = BossMaxTimer;
        StartGameUI();
        
        for (int i = 0; i < players.Length; i++)
        {
            players[i].UnlockMove();
        }
    }

    private void StartGameUI()
    {
        heartSystemUI.Show();
        
        for (int i = 0; i < avatars.Length; i++)
        {
            avatars[i].Show();
        }
    }

    private void OnHeartSystemDied(object sender, EventArgs e)
    {
        Debug.Log("Game Over !");
    }

    private void OnPlayerDamaged(object sender, HomePlayer.HomePlayerDamagedEventArgs e)
    {
        heartSystem.Damage();
        
        for (int i = 0; i < spiders.Count; i++)
        {
            if (spiders[i] == e.spider)
            {
                spiders.RemoveAt(i);
                i--;
            } 
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Stage1:
                HandleSpiderDisappear();
                HandleSpiderSpawn();
                
                bossTimer -= Time.deltaTime;
                if (bossTimer < 0f)
                {
                    state = State.WaitingForBoss;
                    bossTimer = 2f;
                }
                break;
            case State.WaitingForBoss:
                bossTimer -= Time.deltaTime;
                if (bossTimer < 0f)
                {
                    state = State.Boss;
                }

                break;
            case State.Boss:
                Debug.Log("Boss appears");
                break;
        }
    }

    private void HandleSpiderSpawn()
    {
        spiderTimer -= Time.deltaTime;

        if (spiderTimer <= 0f)
        {
            spiderTimer = Random.Range(SpiderMinTimer,  SpiderMinTimer + (SpiderMaxTimer - SpiderMinTimer) * bossTimer / BossMaxTimer );
            SpawnSpider();
        }
    }

    private void HandleSpiderDisappear()
    {
        for (int i = 0; i < spiders.Count; i++)
        {
            if (spiders[i].GetPosition().x < EnemyXDisappear)
            {
                spiders[i].DestroySelf();
                spiders.RemoveAt(i);
                i--;
            } 
        }
    }

    private HomeEnemy SpawnSpider()
    {
        float spiderPosition = Random.Range(MinYPosition, MaxYPosition);
        Transform spiderTransform = Instantiate(spider, new Vector3(EnemyXSpawn, spiderPosition, 0f), Quaternion.identity);
        HomeEnemy newSpider = spiderTransform.GetComponent<HomeEnemy>();
        newSpider.Setup(Vector2.left * Speed, MinYPosition, MaxYPosition);
        newSpider.GetHealthSystem().OnDied += OnSpiderDied;
        spiders.Add(newSpider);
        return newSpider;
    }

    private void OnSpiderDied(object sender, EventArgs e)
    {
        for (int i = 0; i < spiders.Count; i++)
        {
            if (!spiders[i].GetHealthSystem().IsAlive())
            {
                spiders.RemoveAt(i);
                i--;
            } 
        }
    }
}
