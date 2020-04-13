using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AdventureLevel : MonoBehaviour
{
    [SerializeField] private AdventurePlayer[] players;
    [SerializeField] private QuestPointer questPointer;
    
    private enum Stage
    {
        Stage1,
        Stage2,
        Stage3,
        Stage4
    }

    private Stage currentStage;

    private void Awake()
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
                questPointer.Show(new Vector3(0f, 21f, 0f));
                break;
        }
    }
}
