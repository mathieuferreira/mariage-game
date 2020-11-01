using System;
using UnityEngine;

namespace Breakout
{
    public class BreakoutRing : MonoBehaviour
    {
        public event EventHandler onCollision;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            BreakoutBall ball = other.GetComponent<BreakoutBall>();

            if (ball != null)
            {
                onCollision?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
