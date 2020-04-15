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

    [SerializeField] private QuestPosition stage1QuestPosition;
    [SerializeField] private Transform[] stage1PlayersPosition;
    
    private enum Stage
    {
        Stage1,
        Stage2,
        Stage3,
        Stage4
    }

    private Stage currentStage;
    private Loader.Scene nextScene;
    private QuestPosition questPosition;
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
        stageNumber = 1;
        
        switch (stageNumber)
        {
            case 2:
                currentStage = Stage.Stage2;
                break;
            case 3 :
                currentStage = Stage.Stage3;
                break;
            case 4 :
                currentStage = Stage.Stage4;
                break;
            default:
                currentStage = Stage.Stage1;
                nextScene = Loader.Scene.School;
                InitializeQuest(stage1QuestPosition);
                SetPlayerPosition(stage1PlayersPosition);
                break;
        }
    }

    private void InitializeQuest(QuestPosition questPosition)
    {
        playerWithQuestComplete = 0;
        questPosition = stage1QuestPosition;
        questPointer.Show(questPosition);
        questPosition.questComplete += QuestPositionOnQuestComplete;
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

    private void SetPlayerPosition(Transform[] playerPositions)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].SetPosition(playerPositions[i].position);
        }

        cameraFollow.InitPosition();
    } 
}
