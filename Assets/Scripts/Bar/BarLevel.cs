using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Bar
{
    public class BarLevel : MonoBehaviour
    {
        private const float GameTimerMax = 90f;
        //private const float GameTimerMax = 10f;
    
        private const float TalkTimerMax = 3f;
        private const float BeerTimerMax = 5f;
        private const float CakeTimerMax = 8f;
    
        [SerializeField] private BarPlayer[] players = default;
        [SerializeField] private BarGuest[] guests = default;
        [SerializeField] private BarConsumableWorker[] workers = default;
        [SerializeField] private BarConsumableCounter[] counters = default;
        [SerializeField] private Text timerText = default;
        [SerializeField] private GameUI[] gameUis = default;
        [SerializeField] private FunIndicator funIndicator = default;
    
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
            ScoreManager.StartSession();
        }

        private void FunIndicatorOnOnFunExpired(object sender, EventArgs e)
        {
            StopUI();

            SoundManager.GetInstance().Play("Loose");
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
                SoundManager.GetInstance().Play("Win");
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
                guests[Random.Range(0, guests.Length)].GetNeeds().TryAdd(new BarConsumable(BarConsumable.Kind.Talk));
                talkTimer += TalkTimerMax;
            }

            if (beerTimer < 0f)
            {
                guests[Random.Range(0, guests.Length)].GetNeeds().TryAdd(new BarConsumable(BarConsumable.Kind.Beer));
                beerTimer += BeerTimerMax;
            }

            if (cakeTimer < 0f)
            {
                guests[Random.Range(0, guests.Length)].GetNeeds().TryAdd(new BarConsumable(BarConsumable.Kind.Cake));
                cakeTimer += CakeTimerMax;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            foreach (BarPlayer player in players)
            {
                player.UnlockMove();
            }
        
            foreach (BarConsumableWorker worker in workers)
            {
                worker.StartWork();
            }
        
            foreach (BarGuest guest in guests)
            {
                guest.Activate();
            }
        
            foreach (GameUI ui in gameUis)
            {
                ui.Show();
            }
            
            timerText.gameObject.SetActive(true);
            started = true;
            talkTimer = TalkTimerMax;
            beerTimer = BeerTimerMax;
            cakeTimer = CakeTimerMax;
        }

        private void StopUI()
        {
            foreach (BarPlayer player in players)
            {
                player.LockMove();
            }
        
            foreach (BarConsumableWorker worker in workers)
            {
                worker.StopWork();
            }
        
            foreach (BarGuest guest in guests)
            {
                guest.Deactivate();
            }
            
            foreach (GameUI ui in gameUis)
            {
                ui.Hide();
            }
        
            timerText.gameObject.SetActive(false);
            started = false;
        }
    }
}
