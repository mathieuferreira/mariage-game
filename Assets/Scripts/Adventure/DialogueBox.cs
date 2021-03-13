using UnityEngine;
using UnityEngine.UI;

namespace Adventure
{
    public class DialogueBox : MonoBehaviour
    {
        [SerializeField] private Text text;

        public void SetText(string textToDisplay)
        {
            text.text = textToDisplay;
        }

        public void Show()
        {
            text.text = "";
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}