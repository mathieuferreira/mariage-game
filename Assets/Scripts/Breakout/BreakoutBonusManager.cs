using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Breakout
{
    [RequireComponent(typeof(BreakoutLevel))]
    public class BreakoutBonusManager : MonoBehaviour
    {
        [SerializeField] private List<BreakoutBrick> bricks;
        [SerializeField] private BonusConfiguration[] bonuses;
    
        private BreakoutLevel level;

        private void Awake()
        {
            level = GetComponent<BreakoutLevel>();
            foreach (BreakoutBrick brick in bricks)
            {
                brick.OnBreak += BrickOnBreak;
            }
        }

        private void BrickOnBreak(object sender, EventArgs e)
        {
            foreach (BonusConfiguration bonusConfiguration in bonuses)
            {
                float random = Random.Range(0f, 1f);

                if (bonusConfiguration.appearRate < 10)
                {
                    Debug.Log(random + " < " + (1f / bonusConfiguration.appearRate));
                }
                
                if (random < 1f / bonusConfiguration.appearRate)
                {
                    BreakoutBonus bonus = Instantiate(bonusConfiguration.bonus, ((BreakoutBrick)sender).GetPosition(), Quaternion.identity);
                    bonus.OnBallMultiply += BonusOnBallMultiply;
                    return;
                }
            }
        }

        private void BonusOnBallMultiply(object sender, EventArgs e)
        {
            level.MultiplyBalls();
        }

        [Serializable]
        private class BonusConfiguration
        {
            public BreakoutBonus bonus;
            public int appearRate;
        }
    }
}
