using System;
using UnityEngine;

namespace Adventure
{
    public class QuestPosition : InteractableArea<AdventurePlayer>
    {
        public event EventHandler<QuestCompleteEvent> OnQuestComplete;

        private float timer;

        protected override void DoAreaAction(AdventurePlayer player)
        {
            OnQuestComplete?.Invoke(this, new QuestCompleteEvent() { Player = player });
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