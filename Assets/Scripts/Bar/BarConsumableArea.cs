using System.Collections.Generic;
using Adventure;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bar
{
    public class BarConsumableArea : InteractableArea<BarPlayer>
    {
        [SerializeField] private Transform[] consumablePositions = default;
        [FormerlySerializedAs("consumableType")] [SerializeField] private BarConsumable.Kind consumableKind = default;
        [SerializeField] private Transform consumable = default;

        private List<GameObject> consumableList; 

        private void Awake()
        {
            consumableList = new List<GameObject>();
        }
        
        protected override bool IsPlayerAccepted(BarPlayer player)
        {
            if (!HasConsumableAvailable() || player.GetConsumableList().IsFull())
                return false;

            return base.IsPlayerAccepted(player);
        }

        protected override void DoAreaAction(BarPlayer player)
        {
            BarConsumable cons = TryConsume();
            player.GetConsumableList().TryAdd(cons);
            SoundManager.GetInstance().Play("Take");
                
            if (player.GetConsumableList().IsFull() || !HasConsumableAvailable())
                player.HideAdviceButton();
        }

        private BarConsumable TryConsume()
        {
            if (!HasConsumableAvailable())
                return null;

            int consumableIndex = consumableList.Count - 1;
            GameObject consumableToDelete = consumableList[consumableIndex];
            consumableList.RemoveAt(consumableIndex);
            Destroy(consumableToDelete);
            return new BarConsumable(consumableKind);
        }

        private bool HasConsumableAvailable()
        {
            return consumableList.Count > 0;
        }
    
        public bool HasConsumablePlace()
        {
            return consumableList.Count < consumablePositions.Length;
        }

        public void AddConsumable()
        {
            Vector3 position = consumablePositions[consumableList.Count].position;
            position.z = 0f;
            Transform newConsumable = Instantiate(consumable, position, Quaternion.identity);
            consumableList.Add(newConsumable.gameObject);

            foreach (BarPlayer player in GetPlayersInArea())
            {
                if (!player.GetConsumableList().IsFull())
                {
                    player.ShowAdviceButton();
                }
            }
        }
    }
}
