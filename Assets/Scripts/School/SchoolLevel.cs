using System;
using System.Collections.Generic;
using Adventure;
using UnityEngine;
using Random = UnityEngine.Random;

namespace School
{
    public class SchoolLevel : MonoBehaviour
    {
        private static float ENEMY_SPAWN_MAX_TIME = 8f;
        private static int ENEMY_MAX_COUNT = 10;
        private static float BOSS_APPEAR_TIMEOUT = 100f;
    
        [SerializeField] private SchoolPlayer[] players = default;
        [SerializeField] private SchoolShuriken shuriken = default;
        [SerializeField] private SchoolEnemy[] enemyPrefabs = default;
        [SerializeField] private SchoolBonus bonus = default;
        [SerializeField] private SchoolBoss boss = default;
        [SerializeField] private HealthBar bossHealthBar = default;
        [SerializeField] private Avatar[] avatars = default;
        [SerializeField] private HeartSystemUI heartSystemUI = default;
        [SerializeField] private ProgressBar progressBar = default;
        [SerializeField] private ParticleSystem bloodParticleSystem = default;

        private enum Stage
        {
            WaitingForStart,
            Stage1,
            Boss,
            Victory,
            Defeat
        }
    
        private List<SchoolEnemy> enemies;
        private List<SchoolShuriken> shurikens;
        private List<Vector3> spawnPositions;
        private float enemySpawnTimer;
        private Stage currentStage;
        private float bossSpawnTimer;
        private ModalLevel modalLevel;
        private HeartSystem heartSystem;

        private void Awake()
        {
            currentStage = Stage.WaitingForStart;
            modalLevel = GetComponent<ModalLevel>();
            modalLevel.gameStart += StartGame;
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
            heartSystem = new HeartSystem(5);
            heartSystem.OnDied += HeartSystemOnOnDied;
        
            heartSystemUI.Setup(heartSystem);
            
            ScoreManager.StartSession();
        }

        private void HeartSystemOnOnDied(object sender, EventArgs e)
        {
            currentStage = Stage.Defeat;
        
            StopGameUI();
            
            ScoreManager.RevertSession();
            
            SoundManager.GetInstance().StopPlaying("Rain");
            SoundManager.GetInstance().StopPlaying("Zombi1");
            SoundManager.GetInstance().StopPlaying("Zombi2");

            modalLevel.OpenLooseWindow(() =>
                {
                    Loader.Load(Loader.Scene.School);
                }
            );
        }

        private void FixedUpdate()
        {
            if (currentStage == Stage.WaitingForStart || currentStage == Stage.Victory)
                return;

            if (enemies.Count < ENEMY_MAX_COUNT)
            {
                enemySpawnTimer -= Time.fixedDeltaTime;
            
                if (enemySpawnTimer < 0)
                {
                    SpawnEnemy();
                }
            }

            if (currentStage == Stage.Stage1)
            {
                bossSpawnTimer -= Time.fixedDeltaTime;

                if (bossSpawnTimer < 0)
                {
                    StartBossStage();
                }
            }

            for (int i = 0; i < avatars.Length; i++)
            {
                avatars[i].SetAlpha(players[i].GetPosition().y > -2 ? 1f : .3f);
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            InitShuriken();
            SpawnEnemy();
            bossSpawnTimer = BOSS_APPEAR_TIMEOUT;
            currentStage = Stage.Stage1;
            StartGameUI();
            SoundManager.GetInstance().Play("Rain");
        }

        private void StartGameUI()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].UnlockMove();
            }

            for (int i = 0; i < avatars.Length; i++)
            {
                avatars[i].Show();
            }

            heartSystemUI.Show();
            progressBar.GetComponent<GameUI>().Show();
        }

        private void StopGameUI()
        {
            bossHealthBar.Hide();
            heartSystemUI.Hide();
            progressBar.GetComponent<GameUI>().Hide();

            for (int i = 0; i < players.Length; i++)
            {
                players[i].LockMove();
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].GetHealthSystem().IsAlive())
                    enemies[i].DestroySelf();
            }

            for (int i = 0; i < shurikens.Count; i++)
            {
                if (!shurikens[i].IsDestroyed())
                    shurikens[i].DestroySelf();
            }

            for (int i = 0; i < avatars.Length; i++)
            {
                avatars[i].Hide();
            }
        }

        private void StartBossStage()
        {
            currentStage = Stage.Boss;
            SchoolBoss newBoss = Instantiate(boss, new Vector3(0f, 0f, 0f), Quaternion.identity);
            newBoss.GetHealthSystem().OnDied += BossOnDied;
            progressBar.GetComponent<GameUI>().Hide();
            bossHealthBar.Show();
            bossHealthBar.Setup(newBoss.GetHealthSystem());
            SoundManager.GetInstance().Play("Zombi2");
        }

        private void BossOnDied(object sender, EventArgs e)
        {
            currentStage = Stage.Victory;
        
            SoundManager.GetInstance().StopPlaying("Rain");
            SoundManager.GetInstance().StopPlaying("Zombi2");
            SoundManager.GetInstance().StopPlaying("Zombi1");
            StopGameUI();
            ScoreManager.CloseSession();

            modalLevel.OpenWinWindow(() =>
                {
                    AdventureLevel.SetStage(AdventureLevel.Stage.Bar);
                    Loader.Load(Loader.Scene.Adventure);
                }
            );
        }

        #region Shurikens
    
        private void OnOnShurikenLaunch(object sender, ShurikenLaunchEventArgs e)
        {
            SchoolPlayer player = sender as SchoolPlayer;
            SchoolShuriken newShuriken = CreateShuriken(e.GetPosition());
            newShuriken.StartMoving(player.GetPlayerId());
        }

        private SchoolShuriken CreateShuriken(Vector3 position)
        {
            SchoolShuriken newShuriken = Instantiate(shuriken, position, Quaternion.identity);
            newShuriken.OnDestroyed += OnShurikenDestroyed;
            newShuriken.OnHit += NewShurikenOnHit;
            shurikens.Add(newShuriken);
            return newShuriken;
        }

        private void NewShurikenOnHit(object sender, SchoolShuriken.HitEventArgs e)
        {
            SchoolShuriken shuriken = sender as SchoolShuriken;
            ScoreManager.IncrementScore(shuriken.GetLastPlayerTouched());
            ParticleSystem blood = Instantiate(bloodParticleSystem, e.GetHitPosition(), Quaternion.identity);
            Destroy(blood.gameObject, 4f);
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
            {
                InitShuriken();
            }
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
    
        #endregion

        #region Enemies

        private void SpawnEnemy()
        {
            Vector3 position = spawnPositions[Random.Range(0, spawnPositions.Count)];
            SchoolEnemy newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], position, Quaternion.identity);
            newEnemy.Setup(players[Random.Range(0, players.Length)]);
            newEnemy.GetHealthSystem().OnDied += NewEnemyOnOnDied;
            newEnemy.OnDisappear += NewEnemyOnOnDisappear;
            newEnemy.OnHitPlayer += NewEnemyOnOnHitPlayer;
            enemySpawnTimer = ENEMY_SPAWN_MAX_TIME;
            enemies.Add(newEnemy);
            
            if (CountEnemyAlive() == 1)
            {
                SoundManager.GetInstance().Play("Zombi1");
            } 
            else if (CountEnemyAlive() == 2)
            {
                SoundManager.GetInstance().Play("Zombi2");
            }
        }

        private void NewEnemyOnOnHitPlayer(object sender, EventArgs e)
        {
            heartSystem.Damage();
        }

        private void NewEnemyOnOnDied(object sender, EventArgs e)
        {
            int enemiesKilled = 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i].GetHealthSystem().IsAlive())
                {
                    enemiesKilled++;
                }
            }

            progressBar.SetFillAmount((float)enemiesKilled / ENEMY_MAX_COUNT);
        
            if (enemies.Count < ENEMY_MAX_COUNT)
            {
                SpawnEnemy();
                return;
            }

            if (currentStage == Stage.Stage1 && !AreEnemyRemaining())
            {
                StartBossStage();
            }
        }

        private bool AreEnemyRemaining()
        {
            if (enemies.Count < ENEMY_MAX_COUNT)
                return true;

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].GetHealthSystem().IsAlive())
                    return true;
            }

            return false;
        }
        
        private int CountEnemyAlive()
        {
            int count = 0;
            
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].GetHealthSystem().IsAlive())
                    count++;
            }

            return count;
        }

        private void NewEnemyOnOnDisappear(object sender, EventArgs e)
        {
            if (Random.Range(0, 2) == 0)
            {
                SpawnBonus(((SchoolEnemy)sender).GetCurrentPosition());
            }

            if (CountEnemyAlive() < 2)
            {
                SoundManager.GetInstance().StopPlaying("Zombi2");
            }
            else if (CountEnemyAlive() < 1)
            {
                SoundManager.GetInstance().StopPlaying("Zombi1");
            }
        }

        #endregion

        #region Bonus

        private void SpawnBonus(Vector3 position)
        {
            SchoolBonus bonus = Instantiate(this.bonus, position, Quaternion.identity);
            bonus.OnConsume += BonusOnOnConsume;
        }
    
        private void BonusOnOnConsume(object sender, SchoolBonus.BonusConsumedeventArgs e)
        {
            SchoolShuriken newShuriken = CreateShuriken(e.shuriken.GetPosition());
            newShuriken.StartMoving(e.shuriken.GetLastPlayerTouched(), new Vector3(-1 * e.shuriken.GetVelocity().x * .75f, e.shuriken.GetVelocity().y * .75f, 0));
        }

        #endregion
    
    }
}
