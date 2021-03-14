using System;
using UnityEngine;

namespace Adventure
{
    public class AdventureLevel : MonoBehaviour
    {
        [SerializeField] private BaseRPGPlayer[] players = default;
        [SerializeField] private QuestPointer questPointer = default;
        [SerializeField] private CameraFollow cameraFollow = default;

        [SerializeField] private StageInformation stage1 = default;
        [SerializeField] private StageInformation stage2 = default;
        [SerializeField] private StageInformation stage3 = default;
        [SerializeField] private StageInformation stage4 = default;

        private static Stage _currentStage = Stage.School;
        private StageInformation currentStageInformation;
        private Loader.Scene nextScene;
        private int playerWithQuestComplete;

        public enum Stage
        {
            School,
            Bar,
            Home,
            Breakout
        }

        private void Awake()
        {
            DialogueManager dialogueManager = GetComponent<DialogueManager>();
            dialogueManager.OnDialogueStart += OnDialogueStart;
            dialogueManager.OnDialogueEnd += OnDialogueEnd;
        }

        private void OnDialogueEnd(object sender, EventArgs e)
        {
            questPointer.Show();
        }

        private void OnDialogueStart(object sender, EventArgs e)
        {
            questPointer.Hide();
        }

        private void Start()
        {
            InitializeCurrentStage();
        }

        public static void SetStage(Stage stage)
        {
            _currentStage = stage;
        }

        private void InitializeCurrentStage()
        {
            switch (_currentStage)
            {
                case Stage.Bar:
                    currentStageInformation = stage2;
                    nextScene = Loader.Scene.Bar;
                    break;
                case Stage.Home :
                    currentStageInformation = stage3;
                    nextScene = Loader.Scene.Home;
                    break;
                case Stage.Breakout :
                    currentStageInformation = stage4;
                    nextScene = Loader.Scene.Breakout;
                    break;
                default:
                    currentStageInformation = stage1;
                    nextScene = Loader.Scene.School;
                    break;
            }
        
            InitializeQuest();
            SetPlayerPosition();
        }

        private void InitializeQuest()
        {
            playerWithQuestComplete = 0;
            questPointer.Show(currentStageInformation.GetQuestPosition());
            currentStageInformation.GetQuestPosition().OnQuestComplete += OnQuestPositionOnOnQuestComplete;
            currentStageInformation.GetQuestPosition().Activate();
        }

        private void OnQuestPositionOnOnQuestComplete(object sender, QuestCompleteEvent e)
        {
            SoundManager.GetInstance().Play("QuestOK");
            playerWithQuestComplete++;
            e.Player.Disappear();

            if (playerWithQuestComplete == players.Length)
            {
                Loader.Load(nextScene);
            }
        }

        private void SetPlayerPosition()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].SetPosition(currentStageInformation.GetPlayerPosition(i));
            }

            cameraFollow.InitPosition();
        }

        [Serializable]
        private class StageInformation
        {
            [SerializeField] private QuestPosition questPosition = default;
            [SerializeField] private Transform[] playersPosition = default;

            public QuestPosition GetQuestPosition()
            {
                return questPosition;
            }

            public Vector3 GetPlayerPosition(int player)
            {
                return playersPosition[player].position;
            }
        }
    }
}
