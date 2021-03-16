using UnityEngine;

namespace Adventure
{
    public class WeddingArea : MonoBehaviour
    {
        [SerializeField] private string message;
    
        private int playerInCount;

        private void OnTriggerEnter2D(Collider2D other)
        {
            AdventurePlayer player = other.GetComponent<AdventurePlayer>();

            if (player != null)
            {
                playerInCount++;

                if (playerInCount == 2)
                {
                    DialogueManager.GetInstance().StartDialogue(message, player.GetPlayerId());
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            AdventurePlayer player = other.GetComponent<AdventurePlayer>();

            if (player != null)
            {
                playerInCount--;
            }
        }
    }
}
