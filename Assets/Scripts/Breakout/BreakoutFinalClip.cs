using System;
using UnityEngine;

namespace Breakout
{
    public class BreakoutFinalClip : MonoBehaviour
    {
        [SerializeField] private BreakoutWinRingClip winRingWindow;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private BreakoutEndWindow endWindow;
        
        private Animator animator;
        private static readonly int StartClip = Animator.StringToHash("Start");

        public void Awake()
        {
            animator = GetComponent<Animator>();
            winRingWindow.FadeComplete += OnRingFadeInComplete;
            winRingWindow.AnimationComplete += OnRingAnimationComplete;
        }

        private void OnRingFadeInComplete(object sender, EventArgs e)
        {
            mainCamera.transform.position = new Vector3(0, -100, -10);
        }

        private void OnRingAnimationComplete(object sender, EventArgs e)
        {
            winRingWindow.Disable();
            animator.SetTrigger(StartClip);
        }

        public void StartAnimation()
        {
            winRingWindow.StartAnimation();
        }

        private void OnAnimationComplete()
        {
            endWindow.Enable();
        }
    }
}
