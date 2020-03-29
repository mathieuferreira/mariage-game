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
        players[0] = new ChoosePlayer(0, transform.Find("ChoosePlayer1"));
        players[1] = new ChoosePlayer(1, transform.Find("ChoosePlayer2"));

        players[0].Init(GameAssets.getInstance().Player1PicturesSprites);
        players[1].Init(GameAssets.getInstance().Player2PicturesSprites);
    }

    private void Update()
    {
        bool allPlayerChosen = true;
        for (int i = 0; i < players.Length; i++)
        {
            ChoosePlayer player = players[i];
            player.HandleInputs();

            if (!player.HasPlayerChosen())
                allPlayerChosen = false;
        }

        if (allPlayerChosen)
        {
            FunctionTimer.Create(() =>
            {
                Loader.Load(Loader.Scene.Initial);
            }, .5f);
        }
    }

    private class ChoosePlayer
    {
        private int number;
        private int pictureSelectedX;
        private int pictureSelectedY;
        private bool playerChosen;
        private PlayerPicture[,] pictures;

        public ChoosePlayer(int number, Transform baseTransform)
        {
            this.number = number;
            playerChosen = false;
            pictures = new PlayerPicture[2,2];
            pictures[0,0] = new PlayerPicture(baseTransform.Find("PlayerPicture1"));
            pictures[0,1] = new PlayerPicture(baseTransform.Find("PlayerPicture2"));
            pictures[1,0] = new PlayerPicture(baseTransform.Find("PlayerPicture3"));
            pictures[1,1] = new PlayerPicture(baseTransform.Find("PlayerPicture4"));
        }

        public void SelectPicture(int x, int y)
        {
            int newPictureSelectedX = Math.Abs(x % 2);
            int newPictureSelectedY = Math.Abs(y % 2);
            pictures[pictureSelectedX, pictureSelectedY].Unselect();
            pictures[newPictureSelectedX, newPictureSelectedY].Select();
            pictureSelectedX = newPictureSelectedX;
            pictureSelectedY = newPictureSelectedY;
        }

        public void Init(Sprite[] sprites)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    pictures[i, j].SetSprite(sprites[i*2+j]);
                    if (i == pictureSelectedX && j == pictureSelectedY)
                    {
                        pictures[i, j].Select();
                    }
                    else
                    {
                        pictures[i, j].Unselect();
                    }
                }
            }
        }

        private void Choose()
        {
            pictures[pictureSelectedX, pictureSelectedY].Choose();
            playerChosen = true;
        }

        public void HandleInputs()
        {
            if (playerChosen)
                return;
            
            if (UserInput.isKeyDown(number, UserInput.Key.Up))
                SelectPicture(pictureSelectedX - 1, pictureSelectedY);
            
            if (UserInput.isKeyDown(number, UserInput.Key.Down))
                SelectPicture(pictureSelectedX + 1, pictureSelectedY);
            
            if (UserInput.isKeyDown(number, UserInput.Key.Left))
                SelectPicture(pictureSelectedX, pictureSelectedY - 1);
            
            if (UserInput.isKeyDown(number, UserInput.Key.Right))
                SelectPicture(pictureSelectedX, pictureSelectedY + 1);

            if (UserInput.isKeyDown(number, UserInput.Key.Action))
                Choose();
        }

        public bool HasPlayerChosen()
        {
            return playerChosen;
        }
    }

    private class PlayerPicture
    {
        private bool selected;
        private GameObject overlay;
        private Image image;
        private Text text;
        private Animator anim;

        public PlayerPicture(Transform baseTransform)
        {
            overlay = baseTransform.Find("Overlay").gameObject;
            image = baseTransform.Find("Picture").GetComponent<Image>();
            text = baseTransform.Find("Text").GetComponent<Text>();
            anim = baseTransform.GetComponent<Animator>();
            selected = false;
        }

        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void Select()
        {
            overlay.SetActive(false);
            text.color = Color.black;
            selected = true;
        }

        public void Unselect()
        {
            overlay.SetActive(true);
            text.color = GameColor.COLOR_GREY_BUTTON;
            selected = false;
        }

        public void Choose()
        {
            anim.enabled = true;
            anim.SetTrigger("PlayerPictureSelected");
        }
    }
}
