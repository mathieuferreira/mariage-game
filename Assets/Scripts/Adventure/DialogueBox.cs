using UnityEngine;
using UnityEngine.UI;

namespace Adventure
{
    public class DialogueBox : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private GameObject player1Button;
        [SerializeField] private GameObject player2Button;

        public void SetText(string textToDisplay)
        {
            text.text = textToDisplay;
        }

        public void Show()
        {
            text.text = "";
            player1Button.SetActive(false);
            player2Button.SetActive(false);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            player1Button.SetActive(false);
            player2Button.SetActive(false);
            gameObject.SetActive(false);
        }

        public void ShowPlayerButton(PlayerID playerID)
        {
            if (playerID == PlayerID.Player1)
            {
                player1Button.SetActive(true);
                return;
            }
            
            player2Button.SetActive(true);
        }
    }
}