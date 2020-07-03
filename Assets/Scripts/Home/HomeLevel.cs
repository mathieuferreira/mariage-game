using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Home
{
    public class HomeLevel : MonoBehaviour
    {
        public const float MaxYPosition = 12f;
        public const float MinYPosition = -12f;
        private const float EnemyXSpawn = 30f;
        private const float EnemyXDisappear = -30f;
        private const float SpiderMinTimer = 1f;
        private const float SpiderMaxTimer = 2f;
        private const float BossMaxTimer = 45f;
        //private const float BossMaxTimer = 10f;
        private const float Speed = 5f;
    
        [SerializeField] private HomePlayer[] players;
        [SerializeField] private Transform spider;
        [SerializeField] private HeartSystemUI heartSystemUI;
        [SerializeField] private Avatar[] avatars;
        [SerializeField] private HomeBoss boss;
        [SerializeField] private Transform web;
        [SerializeField] private GameUI[] UIElements;
        [SerializeField] private ProgressBar progressBar;
        [SerializeField] private HealthBar bossHealthBar;

        private enum State
        {
            WaitingToStart,
            Stage1,
            WaitingForBoss,
            WaitingForBossBattleStart,
            Boss,
            Win,
            Loose
        }
    
        private HomeEnvironment environment;
        private List<HomeEnemy> spiders;
        private float spiderTimer;
        private float bossTimer;
        private State state;
        private ModalLevel modalLevel;
        private HeartSystem heartSystem;
        private HomeBoss bossInstance;
        private Transform webInstance;

        private void Awake()
        {
            spiders = new List<HomeEnemy>();
            environment = GetComponent<HomeEnvironment>();
            bossTimer = BossMaxTimer;
            state = State.WaitingToStart;
            heartSystem = new HeartSystem(5);
            heartSystem.OnDied += OnHeartSystemDied;
            heartSystemUI.Setup(heartSystem);
        
            modalLevel = GetComponent<ModalLevel>();
            modalLevel.gameStart += StartGame;

            foreach (HomePlayer player in players)
            {
                player.LockMove();
                player.OnDamaged += OnPlayerDamaged;
            }
        
            foreach (Avatar avatar in avatars)
            {
                avatar.Hide();
            }
        }
    
        private void StartGame(object sender, EventArgs e)
        {
            state = State.Stage1;
            environment.SetSpeed(Speed);
            SpawnSpider();
            spiderTimer = SpiderMaxTimer;
            bossTimer = BossMaxTimer;
            StartGameUI();
        
            foreach (HomePlayer player in players)
            {
                player.UnlockMove();
            }
        }

        private void StartGameUI()
        {
            heartSystemUI.Show();
        
            foreach (Avatar avatar in avatars)
            {
                avatar.Show();
            }
        
            foreach (GameUI element in UIElements)
            {
                element.Show();
            }
        
            bossHealthBar.Hide();
        }

        private void StopGameUI()
        {
            heartSystemUI.Hide();
        
            foreach (Avatar avatar in avatars)
            {
                avatar.Hide();
            }
        
            foreach (GameUI element in UIElements)
            {
                element.Hide();
            }
        
            bossHealthBar.Hide();
            progressBar.transform.GetComponent<GameUI>().Hide();
        }

        private void OnHeartSystemDied(object sender, EventArgs e)
        {
            if (state == State.Win)
                return;

            EndBattle();
        
            modalLevel.OpenLooseWindow(() =>
            {
                Loader.Load(Loader.Scene.Home);
            });
            state = State.Loose;
        }

        private void EndBattle()
        {
            environment.SetSpeed(0);
            StopGameUI();
            foreach (HomePlayer player in players)
            {
                player.LockMove();
            }
        
            if (webInstance != null)
                Destroy(webInstance.gameObject);

            if (bossInstance != null)
                Destroy(bossInstance.gameObject);

            foreach (HomeEnemy enemy in spiders)
            {
                Destroy(enemy.gameObject);
            }
        }

        private void OnPlayerDamaged(object sender, EventArgs e)
        {
            heartSystem.Damage();
        }

        private void Update()
        {
            switch (state)
            {
                case State.Stage1:
                    HandleSpiderDisappear();
                    HandleSpiderSpawn();
                
                    bossTimer -= Time.deltaTime;
                    progressBar.SetFillAmount((BossMaxTimer - bossTimer) / BossMaxTimer);
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
                        float webSize = web.GetComponent<Renderer>().bounds.size.x;
                        webInstance = Instantiate(web, new Vector3(EnemyXSpawn + webSize / 2, 0f, 0f), Quaternion.identity);
                    
                        bossInstance = Instantiate(boss, new Vector3(EnemyXSpawn + webSize / 2, 0f, 0f), Quaternion.Euler(0f, 0f, -90f));
                        bossInstance.Setup(players, Speed);
                        bossInstance.BattleStart += BossInstanceOnBattleStart;
                        bossInstance.OnDisappear += BossInstanceOnDisappear;
                    
                        bossHealthBar.Setup(bossInstance.GetHealthSystem());
                    
                        state = State.WaitingForBossBattleStart;
                    }

                    break;
                case State.WaitingForBossBattleStart:
                    webInstance.position = bossInstance.GetPosition();
                    break;
                case State.Boss:
                    break;
                case State.Win:
                    break;
                case State.Loose:
                    break;
                case State.WaitingToStart:
                    break;
            }
        }

        private void BossInstanceOnDisappear(object sender, EventArgs e)
        {
            if (state != State.Boss)
                return;
        
            StopGameUI();
            modalLevel.OpenWinWindow(() =>
            {
                AdventureLevel.SetStage(AdventureLevel.Stage.Breakout);
                Loader.Load(Loader.Scene.Adventure);
            });
            state = State.Win;
        }

        private void BossInstanceOnBattleStart(object sender, EventArgs e)
        {
            progressBar.transform.GetComponent<GameUI>().Hide();
            bossHealthBar.Show();
            environment.SetSpeed(0f);
            state = State.Boss;
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
                    spiders[i].Disappear();
                    spiders.RemoveAt(i);
                    i--;
                } 
            }
        }

        private void SpawnSpider()
        {
            float spiderPosition = Random.Range(MinYPosition, MaxYPosition);
            Transform spiderTransform = Instantiate(spider, new Vector3(EnemyXSpawn, spiderPosition, 0f), Quaternion.identity);
            HomeEnemy newSpider = spiderTransform.GetComponent<HomeEnemy>();
            newSpider.Setup(Vector2.left * Speed, MinYPosition, MaxYPosition);
            newSpider.GetHealthSystem().OnDied += OnSpiderDied;
            newSpider.OnDisappear += OnSpiderDisappear;
            spiders.Add(newSpider);
        }

        private void OnSpiderDisappear(object sender, EventArgs e)
        {
            for (int i = 0; i < spiders.Count; i++)
            {
                if (spiders[i] == (HomeEnemy) sender)
                {
                    spiders.RemoveAt(i);
                    i--;
                } 
            }
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
}
