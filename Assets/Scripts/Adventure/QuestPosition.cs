using System;
using CodeMonkey;
using UnityEngine;

namespace Adventure
{
    public class QuestPosition : MonoBehaviour
    {
        public event EventHandler<QuestCompleteEvent> OnQuestComplete;

        private float timer;
    
        private void OnTriggerStay2D(Collider2D other)
        {
            BaseRPGPlayer player = other.GetComponent<BaseRPGPlayer>();
            if (player != null && UserInput.IsActionKeyDown(player.GetPlayerId()))
            {
                OnQuestComplete?.Invoke(this, new QuestCompleteEvent() { Player = player });
            }
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            BaseRPGPlayer player = other.GetComponent<BaseRPGPlayer>();
            if (player != null)
            {
                player.ShowAdviceButton();
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            BaseRPGPlayer player = other.GetComponent<BaseRPGPlayer>();
            if (player != null)
            {
                player.HideAdviceButton();
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }
    }

    public class QuestCompleteEvent : EventArgs
    {
        public BaseRPGPlayer Player;
    }
}