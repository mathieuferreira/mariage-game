using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Breakout
{
    [RequireComponent(typeof(BreakoutLevel))]
    public class BreakoutBonusManager : MonoBehaviour
    {
        [SerializeField] private List<BreakoutBrick> bricks = default;
        [SerializeField] private BonusConfiguration[] bonuses = default;
    
        private BreakoutLevel level;
        private List<BreakoutBonus> activeBonuses;

        private void Awake()
        {
            level = GetComponent<BreakoutLevel>();
            activeBonuses = new List<BreakoutBonus>();
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
                    bonus.Setup(this);
                    bonus.OnBallMultiply += BonusOnBallMultiply;
                    activeBonuses.Add(bonus);
                    return;
                }
            }
        }

        public void RemoveBonus(BreakoutBonus bonus)
        {
            for (int i = 0; i < activeBonuses.Count; i++)
            {
                if (activeBonuses[i].Equals(bonus))
                {
                    activeBonuses.RemoveAt(i);
                    break;
                }
            }
        }

        public void DestroyAllBonuses()
        {
            while (activeBonuses.Count > 0)
            {
                activeBonuses[0].SelfDestroy();
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
