using System;
using Adventure;
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
            
            ScoreManager.Initialize();
            AdventureLevel.SetStage(AdventureLevel.Stage.School);
            Loader.Load(Loader.Scene.Adventure);
        }
    }
}
