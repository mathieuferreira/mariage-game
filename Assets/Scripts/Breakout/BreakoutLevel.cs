using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Breakout
{
    public class BreakoutLevel : MonoBehaviour
    {
        [SerializeField] private BreakoutBall ballPrefab = default;
        [SerializeField] private BreakoutPlayer[] players = default;
        [SerializeField] private BreakoutRing ring = default;
        [SerializeField] private BreakoutFinalClip finalClip = default;
        [SerializeField] private HeartSystemUI heartSystemUI = default;
        [SerializeField] private Avatar[] avatars = default;

        private ModalLevel modalLevel;
        private List<BreakoutBall> balls;
        private BreakoutBonusManager bonusManager;
        private HeartSystem heartSystem;

        private void Awake()
        {
            bonusManager = GetComponent<BreakoutBonusManager>();
            modalLevel = GetComponent<ModalLevel>();
            modalLevel.gameStart += StartGame;
            ring.onCollision += RingOnCollision;
            heartSystem = new HeartSystem(5);
            heartSystem.OnDied += HeartSystemOnDied;
            heartSystemUI.Setup(heartSystem);
            ScoreManager.StartSession();
        }

        private void Start()
        {
            balls = new List<BreakoutBall>();
        }
        
        #region Game mechanisms
        
        private void StartGame(object sender, EventArgs e)
        {
            SpawnInitialBall();
        
            foreach (BreakoutPlayer player in players)
            {
                player.UnlockMove();
            }
        }

        private void Win()
        {
            ScoreManager.CloseSession();
            finalClip.StartAnimation();
            heartSystemUI.Hide();
            foreach (Avatar avatar in avatars)
            {
                avatar.Hide();
            }
        }

        private void HeartSystemOnDied(object sender, EventArgs e)
        {
            StopGame();
            ScoreManager.RevertSession();
            modalLevel.OpenLooseWindow(() =>
            {
                Loader.Load(Loader.Scene.Breakout);
            });
        }

        private void RingOnCollision(object sender, EventArgs e)
        {
            StopGame();
            Win();
        }

        private void StopGame()
        {
            StopBalls();
            bonusManager.DestroyAllBonuses();
            
            foreach (BreakoutPlayer player in players)
            {
                player.LockMove();
            }
        }
        
        #endregion
        
        #region Ball management
        
        private void SpawnBall(Vector3 position, Vector3 velocity, PlayerID player)
        {
            if (balls.Count >= 20)
                return;
            
            BreakoutBall newBall = Instantiate(ballPrefab, position, Quaternion.identity);
            newBall.Setup(player, velocity);
            newBall.OnDisappear += BallOnDisappear;
            balls.Add(newBall);
        }
        
        private void SpawnInitialBall()
        {
            SpawnBall(Vector3.zero, Vector3.down * BreakoutBall.InitialSpeed, PlayerID.Player1);
        }

        public void MultiplyBalls()
        {
            int ballCount = balls.Count;

            for (int i = 0; i < ballCount; i++)
            {
                BreakoutBall ball = balls[i];
                int newAngle = Random.Range(45, 135);
                
                Vector3 velocity = ball.GetVelocity();
                Vector3 newVelocity = Utils.GetVectorFromAngle(newAngle);

                if (Math.Abs(Vector3.Angle(velocity, newVelocity)) < 5)
                {
                    newVelocity = Quaternion.Euler(0, 0, 5) * newVelocity;
                }

                float magnitudeMultiplier = Random.Range(.6f, .8f);
                newVelocity = newVelocity * (velocity.magnitude * magnitudeMultiplier);

                SpawnBall(ball.GetPosition(), newVelocity, ball.GetLastPlayerBounce());
            }
        }

        private void BallOnDisappear(object sender, EventArgs e)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i].Equals(sender))
                {
                    balls.RemoveAt(i);
                    i--;
                }
            }

            if (balls.Count <= 0)
            {
                SpawnInitialBall();
                heartSystem.Damage();
            }
        }

        private void StopBalls()
        {
            foreach (var ball in balls)
            {
                ball.Stop();
            }
        }

        public int GetBallCount()
        {
            return balls.Count;
        }
        
        #endregion
    }
}
