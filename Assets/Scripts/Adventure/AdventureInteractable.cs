using System;
using UnityEngine;

namespace Adventure
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class AdventureInteractable : MonoBehaviour
    {
        [SerializeField] private string name;
        [SerializeField] private Dialogue[] dialogues;
        
        private MovableGuest movableGuest;

        private void Awake()
        {
            movableGuest = GetComponent<MovableGuest>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            AdventurePlayer player = other.GetComponent<AdventurePlayer>();

            if (player != null)
            {
                player.ShowAdviceButton();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            AdventurePlayer player = other.GetComponent<AdventurePlayer>();

            if (player != null)
            {
                player.HideAdviceButton();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            AdventurePlayer player = other.GetComponent<AdventurePlayer>();

            if (player != null && UserInput.IsActionKeyDown(player.GetPlayerId()))
            {
                string content = "";

                if (name != null)
                    content += name + " : ";

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
