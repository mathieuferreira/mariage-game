using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AdventureLevel : MonoBehaviour
{
    [SerializeField] private AdventurePlayer[] players;
    [SerializeField] private QuestPointer questPointer;
    [SerializeField] private CameraFollow cameraFollow;

    [SerializeField] private Stage stage1;
    [SerializeField] private Stage stage2;
    [SerializeField] private Stage stage3;
    [SerializeField] private Stage stage4;

    private Stage currentStage;
    private Loader.Scene nextScene;
    private int playerWithQuestComplete = 0;

    private void Awake()
    {
        
    }

    private void Start()
    {
        InitializeCurrentStage();
    }

    private void FixedUpdate()
    {
        
    }

    private void InitializeCurrentStage()
    {
        int stageNumber = PlayerPrefs.GetInt("AdventureStage", 1);

        // TODO : remove this
        stageNumber = 2;
        
        switch (stageNumber)
        {
            case 2:
                currentStage = stage2;
                break;
            case 3 :
                currentStage = stage3;
                break;
            case 4 :
                currentStage = stage4;
                break;
            default:
                currentStage = stage1;
                nextScene = Loader.Scene.School;
                break;
        }
        
        InitializeQuest();
        SetPlayerPosition();
    }

    private void InitializeQuest()
    {
        playerWithQuestComplete = 0;
        questPointer.Show(currentStage.GetQuestPosition());
        currentStage.GetQuestPosition().questComplete += QuestPositionOnQuestComplete;
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
            players[i].SetPosition(currentStage.GetPlayerPosition(i));
        }

        cameraFollow.InitPosition();
    }

    [Serializable]
    private class Stage
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
