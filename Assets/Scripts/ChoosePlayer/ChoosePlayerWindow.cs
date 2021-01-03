using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePlayerWindow : MonoBehaviour
{
    private ChoosePlayer[] players;

    private void Awake()
    {
        players = new ChoosePlayer[2];
        for (int i = 0; i < 2; i++)
        {
            players[i] = transform.Find("ChoosePlayer" + (i + 1)).GetComponent<ChoosePlayer>();
            players[i].OnPictureSelected += OnOnPictureSelected;
        }
    }

    private void OnOnPictureSelected(object sender, EventArgs e)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].IsValidated())
            {
                return;
            }
        }
        
        AvatarManager.GetInstance().Save();
        
        FunctionTimer.Create(() =>
        {
            ScoreManager.Initialize();
            PlayerPrefs.SetInt("AdventureStage", 1);
            PlayerPrefs.Save();
            Loader.Load(Loader.Scene.Adventure);
        }, .5f);
    }
}
