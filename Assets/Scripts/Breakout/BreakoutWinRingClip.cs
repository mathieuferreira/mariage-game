using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout
{
    public class BreakoutWinRingClip : MonoBehaviour
    {
        public event EventHandler FadeComplete;
        public event EventHandler AnimationComplete;

        private Animator animator;

        public void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void StartAnimation()
        {
            gameObject.SetActive(true);
            //animator.enabled = true;
        }

        private void OnFadeComplete()
        {
            FadeComplete?.Invoke(this, EventArgs.Empty);
        }

        private void OnAnimationComplete()
        {
            AnimationComplete?.Invoke(this, EventArgs.Empty);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
