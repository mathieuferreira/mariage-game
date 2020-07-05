using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Breakout
{
    public class BreakoutLevel : MonoBehaviour
    {
        [SerializeField] private BreakoutBall ballPrefab;
        [SerializeField] private BreakoutPlayer[] players;

        private ModalLevel modalLevel;
        private int ballCounter;

        private void Awake()
        {
            modalLevel = GetComponent<ModalLevel>();
            modalLevel.gameStart += StartGame;
        }

        private void Start()
        {
            ballCounter = 0;
        }

        private void StartGame(object sender, EventArgs e)
        {
            SpawnBall(Vector3.zero);
            StartGameUI();
        
            foreach (BreakoutPlayer player in players)
            {
                player.UnlockMove();
            }
        }

        private void SpawnBall(Vector3 position)
        {
            BreakoutBall newBall = Instantiate(ballPrefab, position, Quaternion.identity);
            newBall.OnDisappear += BallOnDisappear;
            ballCounter++;
        }

        private void BallOnDisappear(object sender, EventArgs e)
        {
            ballCounter--;

            if (ballCounter <= 0)
            {
                SpawnBall(Vector3.zero);
            }
        }

        private void StartGameUI()
        {
            
        }
    }
}
