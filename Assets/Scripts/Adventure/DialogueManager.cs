using System;
using DefaultNamespace;
using UnityEngine;

namespace Adventure
{
    public class DialogueManager : MonoBehaviour, IUserInputInterceptor
    {
        public event EventHandler OnDialogueStart;
        public event EventHandler OnDialogueEnd;
        
        private static DialogueManager _instance;

        public static DialogueManager GetInstance()
        {
            return _instance;
        }

        [SerializeField] private DialogueBox box = default;
        [SerializeField] private float timePerCharacters = .1f;

        private enum Status
        {
            Inactive,
            Writing,
            WaitingForValidation
        }
        
        private string textToWrite;
        private int charIndex;
        private float timer;
        private PlayerID playerID;
        private Status status = Status.Inactive;
        private Status nextStatus = Status.Inactive;

        private void Awake()
        {
            _instance = this;
            UserInput.AddInterceptor(this);
        }

        private void Update()
        {
            if (status != Status.Inactive)
                UserInput.IsActionKeyDown(playerID);
            
            if (status == Status.Writing && nextStatus == Status.Writing)
                HandleWriting();
        }

        private void LateUpdate()
        {
            status = nextStatus;
        }

        private void OnDestroy()
        {
            UserInput.RemoveInterceptor(this);
        }

        public void StartDialogue(string text, PlayerID dialoguePlayerID)
        {
            textToWrite = text;
            charIndex = 0;
            timer = timePerCharacters;
            playerID = dialoguePlayerID;
            box.Show();
            nextStatus = Status.Writing;
            SoundManager.GetInstance().Play("Talking");
            OnDialogueStart?.Invoke(this, EventArgs.Empty);
        }

        private void HandleWriting()
        {
            timer -= Time.deltaTime;

            while (timer < 0f)
            {
                timer += timePerCharacters;
                charIndex++;

                if (textToWrite.Length >= charIndex)
                {
                    string textToRender = textToWrite.Substring(0, charIndex);
                    textToRender += "<color=#00000000>" + textToWrite.Substring(charIndex) + "</color>";
                    box.SetText(textToRender);
                }

                if (charIndex >= textToWrite.Length)
                {
                    SoundManager.GetInstance().StopPlaying("Talking");
                    box.ShowPlayerButton(playerID);
                    nextStatus = Status.WaitingForValidation;
                    return;
                }
            }
        }

        public void OnUserActionInput(UserActionInputEvent userInputEvent)
        {
            if (status == Status.Inactive)
                return;
            
            userInputEvent.PreventInput(true);

            if (userInputEvent.GetPlayerID() != playerID)
                return;

            if (status == Status.Writing)
            {
                box.SetText(textToWrite);
                SoundManager.GetInstance().StopPlaying("Talking");
                box.ShowPlayerButton(playerID);
                nextStatus = Status.WaitingForValidation;
                return;
            }

            if (status == Status.WaitingForValidation)
            {
                box.Hide();
                nextStatus = Status.Inactive;
                OnDialogueEnd?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnUserDirectionInput(UserDirectionInputEvent userInputEvent)
        {
            if (status == Status.Inactive)
                return;
            
            userInputEvent.SetDirection(UserInput.Direction.None);
        }
    }
}