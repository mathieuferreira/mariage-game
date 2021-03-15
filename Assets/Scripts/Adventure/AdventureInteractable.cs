using System;
using UnityEngine;

namespace Adventure
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class AdventureInteractable : InteractableArea<AdventurePlayer>
    {
        [SerializeField] private string displayedName;
        [SerializeField] private Dialogue[] dialogues;
        
        private MovableGuest movableGuest;

        private void Awake()
        {
            movableGuest = GetComponent<MovableGuest>();
        }

        protected override void DoAreaAction(AdventurePlayer player)
        {
            string content = "";

            if (!string.IsNullOrEmpty(displayedName))
                content += displayedName + " : ";

            foreach (Dialogue dialogue in dialogues)
            {
                if (!dialogue.IsApplicableForPlayer(player.GetPlayerId())) 
                    continue;
                    
                content += dialogue.GetContent();
                break;
            }
                
            if (movableGuest != null)
            {
                movableGuest.SetDirection(player.GetPosition() - transform.position);
                movableGuest.LockMove();
                DialogueManager.GetInstance().OnDialogueEnd += OnDialogueEnd;
            }
                
            DialogueManager.GetInstance().StartDialogue(content, player.GetPlayerId());
        }

        private void OnDialogueEnd(object sender, EventArgs e)
        {
            if (movableGuest != null)
            {
                movableGuest.UnlockMove();
                DialogueManager.GetInstance().OnDialogueEnd -= OnDialogueEnd;
            }
        }

        [Serializable]
        private class Dialogue
        {
            public enum Type
            {
                Player1,
                Player2,
                Both
            }

            [SerializeField] private Type type;
            [SerializeField] private string content;

            public bool IsApplicableForPlayer(PlayerID playerID)
            {
                if (type == Type.Both)
                    return true;

                return type == Type.Player1 && playerID == PlayerID.Player1 ||
                       type == Type.Player2 && playerID == PlayerID.Player2;
            }

            public string GetContent()
            {
                return content;
            }
        }
    }
}
