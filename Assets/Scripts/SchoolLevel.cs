using System;
using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.Util;
using UnityEngine;
using Random = UnityEngine.Random;

public class SchoolLevel : MonoBehaviour
{
    private static float ENEMY_SPAWN_MAX_TIME = 8f;
    private static int ENEMY_MAX_COUNT = 10;
    
    [SerializeField] private SchoolExplanationWindow explanationWindow;
    [SerializeField] private SchoolPlayer[] players;
    [SerializeField] private SchoolShuriken shuriken;
    [SerializeField] private SchoolEnemy enemy;
    [SerializeField] private SchoolBonus bonus;
    
    private bool gameStarted;
    private List<SchoolEnemy> enemies;
    private List<SchoolShuriken> shurikens;
    private List<Vector3> spawnPositions;
    private float enemySpawnTimer;

    private void Awake()
    {
        gameStarted = false;
        explanationWindow.onClosed += OnExplanationClosed;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].OnShurikenLaunch += OnOnShurikenLaunch;
        }
        shurikens = new List<SchoolShuriken>();

        spawnPositions = new List<Vector3>();
        Transform spawnPositionWrapper = transform.Find("SpawnPlaces");
        for (int i = 0; i < spawnPositionWrapper.childCount; i++)
        {
            spawnPositions.Add(spawnPositionWrapper.GetChild(i).position);
        }
        
        enemies = new List<SchoolEnemy>();
    }

    private void OnOnShurikenLaunch(object sender, ShurikenLaunchEventArgs e)
    {
        SchoolShuriken newShuriken = CreateShuriken(e.getPosition());
        newShuriken.StartMoving();
    }

    private SchoolShuriken CreateShuriken(Vector3 position)
    {
        SchoolShuriken newShuriken = Instantiate(shuriken, position, Quaternion.identity);
        newShuriken.OnDestroyed += OnShurikenDestroyed;
        shurikens.Add(newShuriken);
        return newShuriken;
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

    private void FixedUpdate()
    {
        if (!gameStarted)
            return;

        if (enemies.Count < ENEMY_MAX_COUNT)
        {
            enemySpawnTimer -= Time.fixedDeltaTime;
            
            if (enemySpawnTimer < 0)
            {
                SpawnEnemy();
            }
        }
    }

    private void OnExplanationClosed(object sender, EventArgs e)
    {
        CountDown.GetInstance().StartCounter(3, () =>
        {
            StartGame();
        });
    }

    private void StartGame()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].UnlockMove();
        }

        InitShuriken();
        SpawnEnemy();
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

    private void SpawnEnemy()
    {
        Vector3 position = spawnPositions[Random.Range(0, spawnPositions.Count)];
        SchoolEnemy newEnemy = Instantiate(enemy, position, Quaternion.identity);
        newEnemy.OnDied += NewEnemyOnOnDied;
        enemySpawnTimer = ENEMY_SPAWN_MAX_TIME;
        enemies.Add(newEnemy);
    }

    private void NewEnemyOnOnDied(object sender, EventArgs e)
    {
        if (enemies.Count < ENEMY_MAX_COUNT)
        {
            SpawnEnemy();
        }

        if (Random.Range(0, 1) == 1)
        {
            SpawnBonus(((SchoolEnemy)sender).GetCurrentPosition());
        }
    }

    private void SpawnBonus(Vector3 position)
    {
        SchoolBonus bonus = Instantiate(this.bonus, position, Quaternion.identity);
        bonus.OnConsume += BonusOnOnConsume;
    }

    private void BonusOnOnConsume(object sender, EventArgs e)
    {
        for (int i = 0; i < shurikens.Count; i++)
        {
            SchoolShuriken actualShiruken = shurikens[i];
            if (!actualShiruken.IsDestroyed())
            {
                SchoolShuriken newShuriken = CreateShuriken(actualShiruken.GetPosition());
                newShuriken.StartMoving(new Vector3(-1 * actualShiruken.GetVelocity().x * .75f, actualShiruken.GetVelocity().y * .75f, 0));
            }
        }
    }
}
