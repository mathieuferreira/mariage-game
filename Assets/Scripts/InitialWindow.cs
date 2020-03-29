using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.UI;

public class InitialWindow : MonoBehaviour
{
    private static float ERROR_MAX_TIME = 6f;
    
    public float blinkTime = 1f;
    
    private PlayerButton[] buttons;
    private GameObject errorText;
    private int selectedButton;
    private float blinkTimer;
    private bool isBlinkShown;
    private float errorTimer;
    private InitialWindowPlayer[] players;
    private void Awake()
    {
        selectedButton = 0;
        buttons = new PlayerButton[4];
        buttons[0] = transform.Find("1PlayerButton").GetComponent<PlayerButton>();
        buttons[1] = transform.Find("2PlayerButton").GetComponent<PlayerButton>();
        buttons[2] = transform.Find("3PlayerButton").GetComponent<PlayerButton>();
        buttons[3] = transform.Find("4PlayerButton").GetComponent<PlayerButton>();
        blinkTimer = blinkTime;
        isBlinkShown = true;
        errorTimer = 0f;
        players = new InitialWindowPlayer[2];
        players[0] = new InitialWindowPlayer(0, transform.Find("PressPlayer1Text"));
        players[1] = new InitialWindowPlayer(1, transform.Find("PressPlayer2Text"));
        errorText = transform.Find("ErrorText").gameObject;
    }

    private void Start()
    {
        buttons[selectedButton].Select();
    }

    private void Update()
    {
        HandleInputs();
        HandleBlink();
        HandleError();
    }

    private void HandleError()
    {
        if (errorTimer <= 0f)
            return;

        errorTimer -= Time.deltaTime;

        if (errorTimer <= 0f)
        {
            HideError();
        }
    }

    private void HideError()
    {
        errorText.SetActive(false);
        errorTimer = 0f;
    }

    private void ShowError(String content)
    {
        errorTimer = ERROR_MAX_TIME;
        errorText.GetComponent<Text>().text = content;
        errorText.SetActive(true);
    }

    private void HandleBlink()
    {
        blinkTimer -= Time.deltaTime;

        if (blinkTimer < 0)
        {
            blinkTimer += blinkTime;
            ToggleBlink();
        }
    }

    private void ToggleBlink()
    {
        isBlinkShown = !isBlinkShown;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].BlinkPressToPlay(isBlinkShown);
        }
    }

    private void HandleInputs()
    {
        for (int i = 0; i < 2; i++)
        {
            if (players[i].HasJoinGame())
                continue;
            
            if (UserInput.isKeyDown(i, UserInput.Key.Up))
            {
                SelectButton(selectedButton - 1);
            }
            
            if (UserInput.isKeyDown(i, UserInput.Key.Down))
            {
                SelectButton(selectedButton + 1);
            }
    
            if (UserInput.isKeyDown(i, UserInput.Key.Action))
            {
                buttons[selectedButton].Click();
                ActivePlayer(i);
            }
        }
    }

    private void ActivePlayer(int player)
    {
        String error = GetErrorText();

        if (error == null)
        {
            players[player].ActivatePlayer();
            HideError();

            if (areAllPlayersJoinedTheGame())
            {
                FunctionTimer.Create(() =>
                {
                    Loader.Load(Loader.Scene.ChoosePlayer);
                }, .5f);
            }
        }
        else
        {
            ShowError(error);
        }
    }

    private bool areAllPlayersJoinedTheGame()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].HasJoinGame())
            {
                return false;
            }
        }
        
        return true;
    }

    private String GetErrorText()
    {
        if (selectedButton == 1)
            return null;

        if (selectedButton == 0)
            return "ET NON ! MAINTENANT, TU N'ES PLUS TOUT SEUL !";

        return (selectedButton + 1) + " JOUEURS ?! PAS ENCORE ! PEUT-ETRE DANS QUELQUES MOIS...";
    }

    private void SelectButton(int button)
    {
        if (button < 0)
            button = 0;

        if (button > 3)
            button = 3;

        if (button == selectedButton)
            return;
        
        buttons[selectedButton].Unselect();
        buttons[button].Select();
        selectedButton = button;
    }

    private class InitialWindowPlayer
    {
        private int number;
        private Transform pressToPlayGameObject;
        private bool gameJoined;
        
        public InitialWindowPlayer(int playerNumber, Transform transform)
        {
            number = playerNumber;
            pressToPlayGameObject = transform;
            gameJoined = false;
        }

        public void BlinkPressToPlay(bool active)
        {
            if (!gameJoined)
                pressToPlayGameObject.gameObject.SetActive(active);
        }

        public void ActivatePlayer()
        {
            gameJoined = true;
            pressToPlayGameObject.GetComponent<Text>().text = "JOUEUR PRET !";
            pressToPlayGameObject.gameObject.SetActive(true);
            pressToPlayGameObject.GetComponent<Animator>().SetTrigger("PlayerEnterGame");
        }

        public bool HasJoinGame()
        {
            return gameJoined;
        }
    }
}
