using System;
using System.Collections.Generic;
using UnityEngine;

namespace Adventure
{
    public abstract class InteractableArea<T> : MonoBehaviour where T : BaseRPGPlayer
    {
        private readonly List<T> players = new List<T>();

        protected void Update()
        {
            if (players.Count == 0)
                return;

            foreach (T player in players)
            {
                if (UserInput.IsActionKeyDown(player.GetPlayerId()) && IsPlayerAccepted(player))
                {
                    DoAreaAction(player);
                    return;
                }
            }
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            T player = other.GetComponent<T>();

            if (player != null)
            {
                players.Add(player);
                
                if (IsPlayerAccepted(player))
                    player.ShowAdviceButton();
            }
        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            T player = other.GetComponent<T>();

            if (player != null)
            {
                player.HideAdviceButton();
                players.Remove(player);
            }
        }

        protected virtual bool IsPlayerAccepted(T player)
        {
            return true;
        }

        protected abstract void DoAreaAction(T player);

        protected List<T> GetPlayersInArea()
        {
            return players;
        }
    }
}