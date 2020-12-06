using System;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Breakout
{
    public class BreakoutLevel : MonoBehaviour
    {
        [SerializeField] private BreakoutBall ballPrefab;
        [SerializeField] private BreakoutPlayer[] players;
        [SerializeField] private BreakoutRing ring;
        [SerializeField] private BreakoutFinalClip finalClip;

        private ModalLevel modalLevel;
        private List<BreakoutBall> balls;
        private BreakoutBonusManager bonusManager;

        private void Awake()
        {
            bonusManager = GetComponent<BreakoutBonusManager>();
            modalLevel = GetComponent<ModalLevel>();
            modalLevel.gameStart += StartGame;
            ring.onCollision += RingOnCollision;
        }

        private void RingOnCollision(object sender, EventArgs e)
        {
            Debug.Log("Ring collision");
            StopGameUI();
            StopBalls();
            bonusManager.DestroyAllBonuses();
            
            foreach (BreakoutPlayer player in players)
            {
                player.LockMove();
            }
            
            Win();
        }

        private void Start()
        {
            balls = new List<BreakoutBall>();
        }

        private void StartGame(object sender, EventArgs e)
        {
            SpawnInitialBall();
            StartGameUI();
        
            foreach (BreakoutPlayer player in players)
            {
                player.UnlockMove();
            }
        }

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

        private void Win()
        {
            finalClip.StartAnimation();
        }

        public void MultiplyBalls()
        {
            int ballCount = balls.Count;

            for (int i = 0; i < ballCount; i++)
            {
                BreakoutBall ball = balls[i];
                int newAngle = Random.Range(45, 135);
                
                Vector3 velocity = ball.GetVelocity();
                Vector3 newVelocity = UtilsClass.GetVectorFromAngle(newAngle);

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
            }
        }

        private void StopBalls()
        {
            foreach (var ball in balls)
            {
                ball.Stop();
            }
        }

        private void StartGameUI()
        {
            
        }

        private void StopGameUI()
        {
            
        }
    }
}
