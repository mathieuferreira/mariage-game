using System;
using CodeMonkey.Utils;
using UnityEngine;

namespace ChoosePlayer
{
    public class ChoosePlayerWindow : MonoBehaviour
    {
        [SerializeField] private PlayerPictureSelector[] players;

        private void Awake()
        {
            foreach (PlayerPictureSelector player in players)
            {
                player.OnPictureSelected += OnOnPictureSelected;
            }
        }

        private void OnOnPictureSelected(object sender, EventArgs e)
        {
            SoundManager.GetInstance().Play("SelectOK");
            
            foreach (PlayerPictureSelector player in players)
            {
                if (!player.IsValidated())
                {
                    return;
                }
            }
        
            FunctionTimer.Create(() =>
            {
                ScoreManager.Initialize();
                PlayerPrefs.SetInt("AdventureStage", 1);
                PlayerPrefs.Save();
                Loader.Load(Loader.Scene.Adventure);
            }, .5f);
        }
    }
}
