using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AdventureLevel : MonoBehaviour
{
    [SerializeField] private BaseRPGPlayer[] players;
    [SerializeField] private QuestPointer questPointer;
    [SerializeField] private CameraFollow cameraFollow;

    [SerializeField] private StageInformation stage1;
    [SerializeField] private StageInformation stage2;
    [SerializeField] private StageInformation stage3;
    [SerializeField] private StageInformation stage4;

    private static Stage currentStage = Stage.School;
    private StageInformation currentStageInformation;
    private Loader.Scene nextScene;
    private int playerWithQuestComplete = 0;

    public enum Stage
    {
        School,
        Bar,
        Home,
        Breakout
    }

    private void Awake()
    {
    }

    private void Start()
    {
        InitializeCurrentStage();
    }

    public static void SetStage(Stage stage)
    {
        currentStage = stage;
    }

    private void InitializeCurrentStage()
    {
        switch (currentStage)
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
                nextScene = Loader.Scene.School;
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
        currentStageInformation.GetQuestPosition().questComplete += QuestPositionOnQuestComplete;
    }

    private void QuestPositionOnQuestComplete(object sender, QuestCompleteEvent e)
    {
        playerWithQuestComplete++;
        e.player.Disappear();

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
        [SerializeField] private QuestPosition questPosition;
        [SerializeField] private Transform[] playersPosition;

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
