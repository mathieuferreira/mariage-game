using System;
using UnityEngine;

namespace Breakout
{
    public class BreakoutEndWindow : MonoBehaviour
    {
        [SerializeField] private PlayerReadyButton[] playerReadyButtons;
        [SerializeField] private GameObject overlay;
        
        private Animator animator;
        private static readonly int Start = Animator.StringToHash("Start");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            
            for (int i = 0; i < playerReadyButtons.Length; i++)
            {
                playerReadyButtons[i].OnPlayerReady += OnPlayerReady;
            }
        }
        
        private void OnPlayerReady(object sender, EventArgs e)
        {
            
            for (int i = 0; i < playerReadyButtons.Length; i++)
            {
                if (!playerReadyButtons[i].IsPlayerReady())
                    return;
            }

            Loader.Load(Loader.Scene.Initial);
        }

        private void OnAnimationComplete()
        {
            overlay.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            animator.SetTrigger(Start);
        }
    }
}
